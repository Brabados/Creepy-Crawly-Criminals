using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : SpecialPiece
{
    public int Strength;
    void Awake()
    {
        Strength = 1;
        SetDictonary();

        discription = "Creates a Bomb tile at selected tiles location of same colour as power."
            + System.Environment.NewLine +
            "A Bomb tile destroys all 8 tiles around it when matched. ";
    }

    public override void SpecialAffect()
    {
        for(int i = 0 -Strength; i <= Strength; i++)
        {
            for (int j = 0 - Strength; j <= Strength; j++)
            {
                if (Grid.Xsize != 0 && Grid.Ysize != 0)
                {
                    if (XPos - i >= 0 && YPos - j >= 0 && XPos + i < Grid.Xsize &&  YPos + j < Grid.Ysize)
                    {
                        if (Grid.Board[XPos + i, YPos + j] != null)
                        {
                            if (Grid.Board[XPos + i, YPos + j].Type != GridController.Type.NONSPACE)
                            {
                                Grid.Board[XPos + i, YPos + j].Clear();
                                Grid.SpawnPieces(XPos + i, YPos + j, GridController.Type.EMPTY);
                            }
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
