using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardData 
{
    public int X;
    public int Y;
    public int[,] Tiles;
    public int[,] Colours;
    public bool[,] Movement;

    public BoardData(GridController Format)
    {
        X = Format.Xsize;
        Y = Format.Ysize;

        Tiles = new int[X, Y];
        Colours = new int[X, Y];
        Movement = new bool[X,Y];

        for(int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                Tiles[i, j] = (int)Format.Board[i, j].Type;
                if (Format.Board[i, j].coloured)
                {
                    Colours[i, j] = (int)(Format.Board[i, j] as ColouredPeices).MyColour;
                }
                Movement[i, j] = Format.Board[i, j].moveable;
            }

        }
    }
}
