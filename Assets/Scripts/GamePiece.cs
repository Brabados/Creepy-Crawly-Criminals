using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{

    private int _Xpos;
    private int _Ypos;
    private GridController _Grid;
    private GridController.Type _Type;

    public MeshRenderer _Mat;

    public bool moveable;
    public bool clearable;
    public bool coloured;
    public int XPos
    {
        get { return _Xpos; }
        set
        {
            if(moveable)
            {
                _Xpos = value;
            }
        }
    }
    public int YPos
    {
        get { return _Ypos; }
        set
        {
            if (moveable)
            {
                _Ypos = value;
            }
        }
    }

    public GridController Grid
    {
        get { return _Grid; }
    }

    public GridController.Type Type
    {
        get { return _Type; }
    }






    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initalize(int x, int y, GridController grid, GridController.Type type, MeshRenderer Mat)
    {
        _Xpos = x;
        _Ypos = y;
        _Grid = grid;
        _Type = type;
        _Mat = Mat;

    }

    public void Move(int NewX, int NewY)
    {
       
        _Xpos = NewX;
        _Ypos = NewY;
        this.transform.localPosition = Grid.Worldposition(NewX, NewY);
        
    }
}
