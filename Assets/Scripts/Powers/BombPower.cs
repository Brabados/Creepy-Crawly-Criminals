using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPower : PowerBase
{
    public int Strength;
    GridController TheGrid;
    void Start()
    {
        Activator.onClick.AddListener(delegate { Select(); });
        Cost = Cost * Strength;
        TheGrid = FindObjectOfType<GridController>();
    }

    public override void Select()
    {
        if (PowerPoints.CheckValue(AssignedColour) >= Cost)
        {
            TheGrid.Hold = true;
            TheGrid.UsedPower = AssignedColour;
            EventManager.current.onTileReplacement += Replace;
        }
    }

    public void Replace(ColouredPeices Change)
    {
        int x = Change.XPos;
        int y = Change.YPos;
        Destroy(TheGrid.Board[x, y].gameObject);
        TheGrid.SpawnPieces(x, y, GridController.Type.BOMB);
        TheGrid.Board[x, y].Move(x, y, 0.01f);
        (TheGrid.Board[x, y] as ColouredPeices).AsignColour(AssignedColour);
        (TheGrid.Board[x, y] as Bomb).Strength = Strength;
        EventManager.current.onTileReplacement -= Replace;

    }
}
