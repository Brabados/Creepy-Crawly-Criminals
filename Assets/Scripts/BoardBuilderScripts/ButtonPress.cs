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
    public MeshRenderer Mesh;

    public void Start()
    {
        BoardMod = FindObjectOfType<BoardCreator>();
        Mesh = transform.gameObject.GetComponent<MeshRenderer>();
    }

    public void OnMouseDown()
    {
        Type = BoardMod.SetType;
        if (Type != GridController.Type.EMPTY && Type != GridController.Type.NONSPACE)
        {
            Colour = BoardMod.SetColour;
        }
    }


}
