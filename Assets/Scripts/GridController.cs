using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridController : MonoBehaviour
{
    //Array of game pieces to be manipulated later
    public GamePiece[,] Board;
    public GameObject[,] Spaces;

    public bool Hold;

    //Inspector side board size veriables, may update for prebuilt levels later
    public int Xsize;
    public int Ysize;

    //Prefab for the spaces the peices can sit on. Currently using place holder object.
    public GameObject SpacePrefab;

    public float FillTime;

    //Enum for each basic tile type. Will need to be extended for special tiles
    public enum Type
    {
        ANT,
        GRASSHOPPER,
        FLY,
        APHID,
        EMPTY,
        NONSPACE,
        BARRIER,
        REPLACE,
        ANY,
        COUNT
    }

    private bool inverse = false;

    //Refrence dictonarys for quick lookup
    private Dictionary<Type, GameObject> TypeDictionary;


    //Structs for containing presetup for conversion to dictonary
    [System.Serializable]
    public struct GamePeiceTypePrefabs
    {
        public Type type;
        public GameObject Prefab;
    }


    //Inspector side ver of dictonarys to allow for setup
    public GamePeiceTypePrefabs[] _GamePeiceTypePrefabs;
   

    void Start()
    {
        //Initalizing dictonarys for use in code from the modified inspector arrays
        TypeDictionary = new Dictionary<Type, GameObject>();

        for (int i = 0; i < _GamePeiceTypePrefabs.Length; i++)
        {
            if (!TypeDictionary.ContainsKey(_GamePeiceTypePrefabs[i].type))
            {
                TypeDictionary.Add(_GamePeiceTypePrefabs[i].type, _GamePeiceTypePrefabs[i].Prefab);
            }
        }
        if (!Hold)
        {

            AddSpaces();

            //Sets blank spaces for the game board
            for (int i = 0; i < Xsize; i++)
            {
                for (int j = 0; j < Ysize; j++)
                {
                    //Instanciates an instance of each space and puts it in the correct location on screen with offset
                    SpawnPieces(i, j, Type.EMPTY);


                    if (Board[i, j].moveable)
                    {
                        Board[i, j].Move(i, j, FillTime);
                    }

                }
            }

            for (int i = 0; i < Xsize; i++)
            {
                for (int j = 0; j < Ysize; j++)
                {
                    if (Board[i, j].Type == Type.NONSPACE)
                    {
                        Transform check = transform.Find("Space" + i + " " + j);
                        Destroy(check.gameObject);
                    }
                }
            }

            StartCoroutine(Filler());
        }

    }

    public IEnumerator Filler()
    {
        while(FillCheck())
        {
            inverse = (!inverse);
            yield return new WaitForSeconds(FillTime);
        }
    }

    public bool FillCheck()
    {
        bool peiceMoved = false;

        //For reffrence will be moving each section of this to its own function later once I'm sure its workin
        //as intended. I hate how these casscading ifs and loops look.

        for (int y = Ysize - 2; y >= 0; y--)
        {
            for (int xLoop = 0; xLoop < Xsize; xLoop++)
            {
                int x = xLoop;

                if(inverse)
                {
                    x = Xsize - 1 - xLoop;
                }

                GamePiece CurrentPiece = Board[x, y];

                if(CurrentPiece.moveable)
                {
                    GamePiece PieceBellow = Board[x, y + 1];

                    
                    if(PieceBellow.Type == Type.EMPTY)
                    {
                        CurrentPiece.Move(x, y + 1, FillTime);
                        Board[x, y + 1] = CurrentPiece;
                        SpawnPieces(x, y, Type.EMPTY);
                        Destroy(PieceBellow.gameObject);
                        peiceMoved = true;
                    }
                    else
                    {
                        peiceMoved = CheckDiagonal(x, y, CurrentPiece);

                        if (!peiceMoved)
                        {
                            bool SideMoved = CheckSide(x, y, CurrentPiece);
                            peiceMoved = SideMoved;
                        }
                        
                        
                    }
                }
            }

        }

        for(int x = 0; x < Xsize; x++)
        {
            GamePiece PieceBellow = Board[x, 0];
            if (PieceBellow.Type == Type.EMPTY)
            {
                int range = Random.Range(0, 4);
                GameObject Tile = (GameObject)Instantiate(TypeDictionary[(Type)range], Worldposition(x,-1), Quaternion.identity);
                Tile.transform.parent = transform;
                Board[x, 0] = Tile.GetComponent<ColouredPeices>();
                (Board[x, 0] as ColouredPeices).Initalize(x, -1, this, (Type)range, Tile.GetComponent<MeshRenderer>());
                Board[x, 0].Move(x, 0, FillTime);
                (Board[x, 0] as ColouredPeices).AsignColour((ColouredPeices.Colour)Random.Range(0, 4));
                Destroy(PieceBellow.gameObject);
                peiceMoved = true;
            }
        }

        if(!peiceMoved)
        {
            for(int y = 0; y < Ysize; y++)
            {
                for(int x = 0; x < Xsize; x++)
                {
                    if(Board[x,y].Type == Type.EMPTY)
                    {
                        GamePiece CurrentPiece = Board[x, y];
                        for(int XLoop = -1; XLoop <= 1; XLoop++)
                        {
                            if(XLoop !=0)
                            {
                                if(x + XLoop <= Xsize && x + XLoop > 0)
                                {
                                    GamePiece ToMove = Board[x + XLoop, y];
                                    if(ToMove.moveable)
                                    {
                                        ToMove.Move(x, y, FillTime);
                                        Board[x, y] = ToMove;
                                        SpawnPieces(x, y, Type.EMPTY);
                                        Destroy(CurrentPiece.gameObject);
                                        peiceMoved = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return peiceMoved;

    }

    public Vector3 Worldposition(int x, int y)
    {
        return new Vector3(x + (x * 3), y + (y * 3), 0);
    }

    public GamePiece SpawnPieces(int x, int y, Type type)
    {
        GameObject Tile = (GameObject)Instantiate(TypeDictionary[type], Vector3.zero, Quaternion.identity);
        Tile.transform.parent = transform;
        if (type != Type.EMPTY && type != Type.BARRIER && type != Type.NONSPACE)
        {
            Board[x, y] = Tile.GetComponent<ColouredPeices>();
            (Board[x, y] as ColouredPeices).Initalize(x, y, this, type, Tile.GetComponent<MeshRenderer>());
        }
        else 
        {
            Board[x, y] = Tile.GetComponent<GamePiece>();
            Board[x, y].Initalize(x, y, this, type);
        }

        return Board[x, y];
    }

    public bool CheckSide(int x, int y, GamePiece CurrentPiece)
    {
        bool peiceMoved = false;
        for (int Side = -1; Side <= 1; Side++)
        {
            if (Side != 0)
            {
                int SideX = x + Side;
                if (inverse)
                {
                    SideX = x - Side;
                }

                if (SideX >= 0 && SideX < Xsize)
                {
                    GamePiece SidePiece = Board[SideX, y];

                    if (SidePiece.Type == Type.EMPTY)
                    {
                        bool CanBeFilled = true;

                        for (int AboveY = y; AboveY > 0; AboveY--)
                        {
                            GamePiece AboveSide = Board[SideX, AboveY];
                            if (AboveSide.moveable)
                            {
                               
                            }
                            else if (!AboveSide.moveable && AboveSide.Type != Type.EMPTY)
                            {
                                CanBeFilled = false;
                                break;
                            }

                        }
                        if (!CanBeFilled)
                        {
                            CurrentPiece.Move(SideX, y, FillTime);
                            Board[SideX, y] = CurrentPiece;
                            SpawnPieces(x, y, Type.EMPTY);
                            Destroy(SidePiece.gameObject);
                            peiceMoved = true;
                            break;
                        }
                    }
                }
            }
        }
        return peiceMoved;
    }

    public bool CheckDiagonal(int x, int y, GamePiece CurrentPiece)
    {
        bool peiceMoved = false;
        for (int Diagonal = -1; Diagonal <= 1; Diagonal++)
        {
            if (Diagonal != 0)
            {
                int DiagonalX = x + Diagonal;
                if (inverse)
                {
                    DiagonalX = x - Diagonal;
                }

                if (DiagonalX >= 0 && DiagonalX < Xsize)
                {
                    GamePiece DiagonalPiece = Board[DiagonalX, y + 1];

                    if (DiagonalPiece.Type == Type.EMPTY)
                    {
                        bool CanBeFilled = true;

                        for (int AboveY = y; AboveY >= 0; AboveY--)
                        {
                            GamePiece AboveDiag = Board[DiagonalX, AboveY];
                            if (AboveDiag.moveable)
                            {
                              
                            }
                            else if (!AboveDiag.moveable && AboveDiag.Type != Type.EMPTY)
                            {
                                CanBeFilled = false;
                                break;
                            }
                        }

                        if (!CanBeFilled)
                        {
                            CurrentPiece.Move(DiagonalX, y + 1, FillTime);
                            Board[DiagonalX, y + 1] = CurrentPiece;
                            SpawnPieces(x, y, Type.EMPTY);
                            Destroy(DiagonalPiece.gameObject);
                            peiceMoved = true;
                            break;
                        }

                    }
                }
            }
        }
        return peiceMoved;
    }

    public void AddSpaces()
    {
        //Storage for each gamepiece.
        Board = new GamePiece[Xsize, Ysize];
        Spaces = new GameObject[Xsize, Ysize];
        //Sets blank spaces for the game board
        for (int i = 0; i < Xsize; i++)
        {
            for (int j = 0; j < Ysize; j++)
            {
                //Instanciates an instance of each space and puts it in the correct location on screen with offset
                GameObject Space = (GameObject)Instantiate(SpacePrefab, new Vector3(i + (i * 3), j + (j * 3), -1), Quaternion.identity);
                Space.name = "Space" + i + " " + j;
                //Makes space a child of the grid
                Space.transform.parent = transform;
                Spaces[i, j] = Space;
            }
        }
    }

    public void Insansiate(BoardData BoardState)
    {
        Xsize = BoardState.X;
        Ysize = BoardState.Y;
        foreach(GamePiece n in Board)
        {
            Destroy(n.gameObject); 
        }

        Board = new GamePiece[Xsize, Ysize];

        for(int i = 0; i < Xsize; i++)
        {
            for(int j = 0; j< Ysize; j++)
            {              
                SpawnPieces(i, j, (Type)BoardState.Tiles[i,j]);
                if(Board[i,j].coloured)
                {
                    (Board[i, j] as ColouredPeices).AsignColour((ColouredPeices.Colour)BoardState.Colours[i, j]);
                }
                if (Board[i, j].moveable)
                {
                    Board[i, j].Move(i, j, FillTime);
                }

            }
        }

        
    }
}
