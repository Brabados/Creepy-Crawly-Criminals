using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourKiller : PowerBase
{
    GridController TheGrid;
    void Start()
    {
        Activator.onClick.AddListener(delegate { Select(); });
        TheGrid = FindObjectOfType<GridController>();
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
                        Destroy(n.gameObject);

                    }
                }
            }
            StartCoroutine(TheGrid.Filler());
            EventManager.current.PowerDrain(AssignedColour);
        }
        
    }
}
