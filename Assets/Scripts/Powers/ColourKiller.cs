using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourKiller : PowerBase
{
    GridController TheGrid;
    void Start()
    {
        TheGrid = FindObjectOfType<GridController>();
        MyDiscription = "Removes all tiles of this colour.";
    }

    public override void Select()
    {
        if (PowerPoints.CheckValue(AssignedColour) >= Cost)
        {
            foreach (GamePiece n in TheGrid.Board)
            {
                if (n.coloured)
                {
                    if ((n as ColouredPeices).MyColour == AssignedColour)
                    {
                        TheGrid.SpawnPieces(n.XPos, n.YPos, GridController.Type.EMPTY);
                        n.Clear();

                    }
                }
            }
            StartCoroutine(TheGrid.Filler());
            EventManager.current.PowerDrain(AssignedColour);
        }
        
    }
}
