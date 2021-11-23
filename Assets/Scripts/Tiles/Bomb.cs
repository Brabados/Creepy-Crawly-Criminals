using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : SpecialPiece
{

    void Awake()
    {
        SetDictonary();
        if (isPasive)
        {
            EventManager.current.onSpecial += SpecialAffect;
        }
    }

    public override void SpecialAffect()
    {
        for(int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if(Grid.Board[XPos + i,YPos + j] != null)
                {
                    Destroy(Grid.Board[XPos + i, YPos + j].gameObject);
                    Grid.SpawnPieces(XPos + i, YPos + j, GridController.Type.EMPTY);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if(isActive)
        {
            SpecialAffect();
        }
    }

}
