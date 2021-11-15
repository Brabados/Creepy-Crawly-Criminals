using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{

    public GridController Grid;

    public InputField Boardname;
    public void save()
    {
        SaveSystem.SaveBoard(Grid,Boardname.text);
    }

}
