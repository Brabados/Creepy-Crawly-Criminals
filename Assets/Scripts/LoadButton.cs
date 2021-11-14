using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
    public GridController Grid;

    public void Load()
    {
       BoardData NewBoard = SaveSystem.LoadBoard(Application.dataPath + "/BoardTest.board");

        Grid.Insansiate(NewBoard);
    }
}
