using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardCreator : MonoBehaviour
{
    public GameObject Board;

    public GridController Controller;

    public InputField X;

    public InputField Y;

    public Dropdown Type;

    public GridController.Type SetType;

    public Dropdown Colour;
    public ColouredPeices.Colour SetColour;

    public Toggle Movement;

    public bool SetMove;

    public void Awake()
    {
        Controller = Board.GetComponent<GridController>();
        Controller.Hold = true;
    }

    public void SetArray()
    {
     //   try
       // {
            Controller.ClearGrid();
            Controller.Xsize = System.Convert.ToInt32(X.text.ToString());
            Controller.Ysize = System.Convert.ToInt32(Y.text.ToString());
            Controller.AddSpaces();

            for (int i = 0; i < Controller.Xsize; i++)
            {
                for (int j = 0; j < Controller.Ysize; j++)
                {
                    ButtonPress Change;
                    Change = Controller.Spaces[i, j].GetComponent<ButtonPress>();
                    Change.X = i;
                    Change.y = j;
                    Change.Type = GridController.Type.EMPTY;
                    Change.Colour = ColouredPeices.Colour.RED;
                }
            }
      //  }
      //  catch
       // {
           // Debug.LogError("Non-Int in size feild");
      //  }
    }

    public void GenerateBoard()
    {
        if(Controller.Board != null)
        {
            for (int i = 0; i < Controller.Xsize; i++)
            {
                for (int j = 0; j < Controller.Ysize; j++)
                {
                    if (Controller.Board[i, j] != null)
                    {
                        Destroy(Controller.Board[i, j].gameObject);
                    }
                }
            }
        }
        foreach(GameObject n in Controller.Spaces)
        {
            ButtonPress Change;
            Change = n.GetComponent<ButtonPress>();
            GamePiece Edit;
            Edit = Controller.SpawnPieces(Change.X, Change.y, Change.Type);
            Edit.moveable = Change.Moveable;
        }
        foreach(GamePiece n in Controller.Board)
        {
            n.Move(n.XPos, n.YPos, 0.1f);
            if(n.coloured)
            {
                (n as ColouredPeices).AsignColour(Controller.Spaces[n.XPos, n.YPos].GetComponent<ButtonPress>().Colour);
            }
        }
    }

    public void TypeChange()
    {
        SetType = (GridController.Type)Type.value;
    }

    public void ColourChange()
    {
        SetColour = (ColouredPeices.Colour)Colour.value;
    }

    public void MovementChanged()
    {
        SetMove = Movement.isOn;
    }
}
