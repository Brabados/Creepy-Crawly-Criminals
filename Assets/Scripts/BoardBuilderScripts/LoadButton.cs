using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadButton : MonoBehaviour
{
    public GridController Grid;

    public void Load()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "Data/Boards");
        FileInfo[] info = dir.GetFiles("*.board");
        BoardData NewBoard = SaveSystem.LoadBoard(info[Random.Range(0,info.Length - 1)].FullName);

        Grid.Insansiate(NewBoard);
    }
}
