using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardCreator : MonoBehaviour
{
    public GameObject Board;

    private GridController Controller;

    public InputField X;

    public InputField Y;

    public Dropdown Type;

    public Dropdown Colour;

    public void Awake()
    {
        Controller = Board.GetComponent<GridController>();
        Controller.Hold = true;
    }

    public void OnMouseDown()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);


    }

    public void SetArray()
    {
        try
        {
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
                    Change.Type = GridController.Type.ANT;
                    Change.Colour = ColouredPeices.Colour.RED;
                }
            }
        }
        catch
        {
            Debug.LogError("Non-Int in size feild");
        }
    }

    public void GenerateBoard()
    {
        foreach(GameObject n in Controller.Spaces)
        {
            ButtonPress Change;
            Change = n.GetComponent<ButtonPress>();
            Controller.SpawnPieces(Change.X, Change.y, Change.Type);
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
}
