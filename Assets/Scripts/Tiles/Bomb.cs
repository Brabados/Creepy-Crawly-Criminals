using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : SpecialPiece
{
    public int Strength;
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
        for(int i = 0 -Strength; i <= Strength; i++)
        {
            for (int j = 0 - Strength; j <= Strength; j++)
            {
                if (i - XPos < 0 || j - YPos < 0 || i + XPos >= Grid.Xsize || j + YPos >= Grid.Ysize)
                {
                    if (Grid.Board[XPos + i, YPos + j] != null)
                    {
                        if (Grid.Board[XPos + i, YPos + j].Type != GridController.Type.NONSPACE)
                        {
                            Destroy(Grid.Board[XPos + i, YPos + j].gameObject);
                            Grid.SpawnPieces(XPos + i, YPos + j, GridController.Type.EMPTY);
                        }
                    }
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
