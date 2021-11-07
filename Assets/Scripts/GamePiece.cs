using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{

    private int _Xpos;
    private int _Ypos;
    private GridController _Grid;
    private GridController.Type _Type;
    private GridController.Colour _Colour;

    public int XPos
    {
        get { return _Xpos; }
    }
    public int YPos
    {
        get { return _Ypos; }
    }

    public GridController Grid
    {
        get { return _Grid; }
    }

    public GridController.Type Type
    {
        get { return _Type; }
    }

    public GridController.Colour Colour
    {
        get { return _Colour; }
    }


    public bool moveable;
    public bool clearable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initalize(int x, int y, GridController grid, GridController.Type type ,GridController.Colour colour)
    {
        _Xpos = x;
        _Ypos = y;
        _Grid = grid;
        _Type = type;
        _Colour = colour;
    }
}
