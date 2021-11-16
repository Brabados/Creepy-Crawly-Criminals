using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{

    public int X;
    public int y;
    public GridController.Type Type;
    public ColouredPeices.Colour Colour;
    public BoardCreator BoardMod;

    public void Start()
    {
        BoardMod = FindObjectOfType<BoardCreator>();
    }

    public void OnMouseDown()
    {
        Type = BoardMod.SetType;
        Colour = BoardMod.SetColour;
    }


}
