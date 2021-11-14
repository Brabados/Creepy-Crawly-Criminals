using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SaveBoard(GridController Save)
    {
        BinaryFormatter Formatter = new BinaryFormatter();
        string file = "/BoardTest.board";
        FileStream stream = new FileStream(file, FileMode.Create);

        BoardData ToSave = new BoardData(Save);

        Formatter.Serialize(stream,ToSave);
        stream.Close();
    }
}
