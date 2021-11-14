using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SaveBoard(GridController Save)
    {
        BinaryFormatter Formatter = new BinaryFormatter();
        string file = Application.dataPath + "/BoardTest.board";
        FileStream stream = new FileStream(file, FileMode.Create);

        BoardData ToSave = new BoardData(Save);

        Formatter.Serialize(stream,ToSave);
        stream.Close();
    }

    public static BoardData LoadBoard(string filePath)
    {
        if(File.Exists(filePath))
        {
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            BoardData Loaded = (Formatter.Deserialize(stream) as BoardData);

            stream.Close();
            return Loaded;

        }
        else 
        {
            Debug.LogError("File not found at:" + filePath);
            return null;
        }

    }
}
