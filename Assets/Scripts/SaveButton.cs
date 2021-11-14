using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public GridController Grid;
    public void save()
    {
        SaveSystem.SaveBoard(Grid);
    }

}
