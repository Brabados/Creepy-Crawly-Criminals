using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceTilePower : PowerBase
{
    GridController TheGrid;
    public GridController.Type type;
    void Start()
    {
        TheGrid = FindObjectOfType<GridController>();
        SpecialPiece MyType = (TheGrid.SpawnPieces(0, 0, type) as SpecialPiece);
        MyDiscription = MyType.discription;
        Destroy(MyType.gameObject);
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
        TheGrid.SpawnPieces(x, y, type);
        TheGrid.Board[x, y].Move(x, y, 0.01f);
        (TheGrid.Board[x, y] as ColouredPeices).AsignColour(AssignedColour);
        EventManager.current.onTileReplacement -= Replace;
        EventManager.current.PowerDrain(AssignedColour);

    }
}
