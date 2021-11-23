using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridController : MonoBehaviour
{
    //Array of game pieces to be manipulated later
    public GamePiece[,] Board;
    public GameObject[,] Spaces;

    //For use to restrict the grid generation when being used for a board builder
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
        BOMB,
        REPLACE,
        ANY,
        COUNT
    }

    //For 'randomizing' direction a piece falls down diagonaly
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


    public GamePiece Current;
    public GamePiece Over;

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
            
            SpawnPieces(5, 5,Type.BOMB);
            Board[5, 5].Move(5,5, FillTime);
            (Board[5, 5] as ColouredPeices).AsignColour(ColouredPeices.Colour.RED);
            //Sets blank spaces for the game board
            for (int i = 0; i < Xsize; i++)
            {
                for (int j = 0; j < Ysize; j++)
                {
                    //Instanciates an instance of each space
                    if (Board[i, j] == null)
                    {
                        SpawnPieces(i, j, Type.EMPTY);
                    }


                    if (Board[i, j].moveable)
                    {
                        Board[i, j].Move(i, j, FillTime);
                    }

                }
            }

            StartCoroutine(Filler());
        }

    }

    public IEnumerator Filler()
    {
        bool needsfill = true;
        while (needsfill)
        {
            yield return new WaitForSeconds(FillTime);
            while (FillCheck())
            {
                inverse = (!inverse);
                yield return new WaitForSeconds(FillTime);
            }
            needsfill = ClearAllMatches();
            if(!needsfill)
            {
                EventManager.current.Special();
            }
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
                                if(x + XLoop < Xsize && x + XLoop > 0)
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
        ClearGrid();
        Xsize = BoardState.X;
        Ysize = BoardState.Y;

        AddSpaces();

        for(int i = 0; i < Xsize; i++)
        {
            for(int j = 0; j< Ysize; j++)
            {              
                SpawnPieces(i, j, (Type)BoardState.Tiles[i,j]);
                if(Board[i,j].coloured)
                {
                    (Board[i, j] as ColouredPeices).AsignColour((ColouredPeices.Colour)BoardState.Colours[i, j]);
                }
                Board[i, j].Move(i, j, FillTime);
                Board[i, j].moveable = BoardState.Movement[i, j];
                if (Board[i, j].Type == Type.NONSPACE)
                {
                    Destroy(Spaces[i, j]);
                }
            }
        }
        StartCoroutine(Filler());
    }

    public void ClearGrid()
    {
        if (Board != null)
        {
            for (int i = 0; i < Xsize; i++)
            {
                for (int j = 0; j < Ysize; j++)
                {
                    if (Board[i, j] != null)
                    {
                        Destroy(Board[i, j].gameObject);
                    }
                }
            }
        }

        if (Spaces != null)
        {
            for (int i = 0; i < Xsize; i++)
            {
                for (int j = 0; j < Ysize; j++)
                {
                    if (Spaces[i, j] != null)
                    {
                        Destroy(Spaces[i, j]);
                    }
                }
            }
        }
    }

    public bool AreAjacent(GamePiece TileA, GamePiece TileB)
    {
        return ((int)Mathf.Abs(TileA.YPos - TileB.YPos) == 1 && (TileA.YPos - TileB.YPos >= -1 && TileA.YPos - TileB.YPos <=1) && (TileA.XPos - TileB.XPos >= -1 && TileA.XPos - TileB.XPos <= 1)) 
            || ((int)Mathf.Abs(TileA.XPos - TileB.XPos) == 1 && (TileA.XPos - TileB.XPos >= -1 && TileA.XPos - TileB.XPos <= 1) && (TileA.YPos - TileB.YPos >= -1 && TileA.YPos - TileB.YPos <= 1));
    }

    public int AreDiagonal(GamePiece TileA, GamePiece TileB)
    {
        for (int i = TileA.XPos, j = TileA.YPos; i < Xsize && j < Ysize; i++, j++)
        {
            if(Board[i,j].Type == Type.BARRIER || Board[i, j].Type == Type.NONSPACE)
            {
                break;
            }
            else if(TileB.XPos == i && TileB.YPos == j)
            {
                return 1;
            }          
        }
        for (int i = TileA.XPos, j = TileA.YPos; i >= 0 && j < Ysize; i--, j++)
        {
            if (Board[i, j].Type == Type.BARRIER || Board[i, j].Type == Type.NONSPACE)
            {
                break;
            }
            if (TileB.XPos == i && TileB.YPos == j)
            {
                 return 2;
            }          
        }
        for (int i = TileA.XPos, j = TileA.YPos; i >= 0 && j >= 0; i--, j--)
        {
            if (Board[i, j].Type == Type.BARRIER || Board[i, j].Type == Type.NONSPACE)
            {
                break;
            }
            if (TileB.XPos == i && TileB.YPos == j)
            {
                return 3;
            }
        }
        for (int i = TileA.XPos , j = TileA.YPos; i < Xsize && j >= 0; i++, j--)
        {
            if (Board[i, j].Type == Type.BARRIER || Board[i, j].Type == Type.NONSPACE)
            {
                break;
            }
            if (TileB.XPos == i && TileB.YPos == j)
            {
                return 4;
            }            
        }
        return 0;
    }

    public bool AreHorizontalOrVertical(GamePiece TileA, GamePiece TileB)
    {
        return (TileA.YPos == TileB.YPos || TileA.XPos == TileB.XPos);
    }

    public void SwapPieces(GamePiece TileA, GamePiece TileB)
    {
        int diag = AreDiagonal(TileA, TileB);
        if(TileA.moveable && TileB.moveable)
        {

            int TileAX = TileB.XPos;
            int TileAY = TileB.YPos;
            List<GamePiece> MovingTiles = new List<GamePiece>();

            if(AreAjacent(TileA,TileB))
            {
                TileAX = TileA.XPos;
                TileAY = TileA.YPos;
                Board[TileB.XPos, TileB.YPos] = TileA;
                Board[TileAX, TileAY] = TileB;
                if (CheckMatch(TileA, TileB.XPos, TileB.YPos) != null || CheckMatch(TileB, TileA.XPos, TileA.YPos) != null)
                {
                    TileA.Move(TileB.XPos, TileB.YPos, FillTime);
                    TileB.Move(TileAX, TileAY, FillTime);
                    ClearAllMatches();
                    StartCoroutine(Filler());
                }
                else
                {
                    Board[TileA.XPos, TileA.YPos] = TileB;
                    Board[TileAX, TileAY] = TileA;
                }

            }
            else if(AreHorizontalOrVertical(TileA, TileB))
            {
                if (CheckMatch(TileA, TileB.XPos, TileB.YPos) != null)
                {
                    if (TileA.XPos == TileB.XPos)
                    {
                        int Direction = TileA.YPos - TileB.YPos;
                        if (Direction < 0)
                        {
                            for (int i = TileA.YPos + 1; i <= TileB.YPos; i++)
                            {
                                MovingTiles.Add(Board[TileA.XPos, i]);
                            }
                            for (int i = 0; i < MovingTiles.Count; i++)
                            {
                                Board[MovingTiles[i].XPos, MovingTiles[i].YPos - 1] = MovingTiles[i];
                                MovingTiles[i].Move(MovingTiles[i].XPos, MovingTiles[i].YPos - 1, FillTime);
                            }
                            Board[TileAX, TileAY] = TileA;
                            TileA.Move(TileAX, TileAY, FillTime);
                            ClearAllMatches();
                            StartCoroutine(Filler());
                        }
                        else
                        {
                            for (int i = TileA.YPos - 1; i >= TileB.YPos; i--)
                            {
                                MovingTiles.Add(Board[TileA.XPos, i]);
                            }
                            for (int i = 0; i < MovingTiles.Count; i++)
                            {
                                Board[MovingTiles[i].XPos, MovingTiles[i].YPos + 1] = MovingTiles[i];
                                MovingTiles[i].Move(MovingTiles[i].XPos, MovingTiles[i].YPos + 1, FillTime);

                            }
                            Board[TileAX, TileAY] = TileA;
                            TileA.Move(TileAX, TileAY, FillTime);
                            ClearAllMatches();
                            StartCoroutine(Filler());
                        }
                    }
                    else
                    {
                        int Direction = TileA.XPos - TileB.XPos;
                        if (Direction < 0)
                        {
                            for (int i = TileA.XPos + 1; i <= TileB.XPos; i++)
                            {
                                MovingTiles.Add(Board[i, TileA.YPos]);
                            }
                            for (int i = 0; i < MovingTiles.Count; i++)
                            {
                                Board[MovingTiles[i].XPos - 1, MovingTiles[i].YPos] = MovingTiles[i];
                                MovingTiles[i].Move(MovingTiles[i].XPos - 1, MovingTiles[i].YPos, FillTime);

                            }
                            Board[TileAX, TileAY] = TileA;
                            TileA.Move(TileAX, TileAY, FillTime);
                            ClearAllMatches();
                            StartCoroutine(Filler());
                        }
                        else
                        {
                            for (int i = TileA.XPos - 1; i >= TileB.XPos; i--)
                            {
                                MovingTiles.Add(Board[i, TileA.YPos]);
                            }
                            for (int i = 0; i < MovingTiles.Count; i++)
                            {
                                Board[MovingTiles[i].XPos + 1, MovingTiles[i].YPos] = MovingTiles[i];
                                MovingTiles[i].Move(MovingTiles[i].XPos + 1, MovingTiles[i].YPos, FillTime);

                            }
                            Board[TileAX, TileAY] = TileA;
                            TileA.Move(TileAX, TileAY, FillTime);
                            ClearAllMatches();
                            StartCoroutine(Filler());
                        }
                    }
                }
            }
            else if(diag != 0)
            {
                if (CheckMatch(TileA, TileB.XPos, TileB.YPos) != null)
                {
                    if (diag == 1)
                    {
                        for (int i = TileA.XPos + 1, j = TileA.YPos + 1; i <= TileB.XPos && j <= TileB.YPos; i++, j++)
                        {
                            MovingTiles.Add(Board[i, j]);
                        }
                        for (int i = 0; i < MovingTiles.Count; i++)
                        {
                            Board[MovingTiles[i].XPos - 1, MovingTiles[i].YPos - 1] = MovingTiles[i];
                            MovingTiles[i].Move(MovingTiles[i].XPos - 1, MovingTiles[i].YPos - 1, FillTime);
                        }
                        Board[TileAX, TileAY] = TileA;
                        TileA.Move(TileAX, TileAY, FillTime);
                        ClearAllMatches();
                        StartCoroutine(Filler());
                    }
                    else if (diag == 2)
                    {
                        for (int i = TileA.XPos - 1, j = TileA.YPos + 1; i >= TileB.XPos && j <= TileB.YPos; i--, j++)
                        {
                            MovingTiles.Add(Board[i, j]);
                        }
                        for (int i = 0; i < MovingTiles.Count; i++)
                        {
                            Board[MovingTiles[i].XPos + 1, MovingTiles[i].YPos - 1] = MovingTiles[i];
                            MovingTiles[i].Move(MovingTiles[i].XPos + 1, MovingTiles[i].YPos - 1, FillTime);
                        }
                        Board[TileAX, TileAY] = TileA;
                        TileA.Move(TileAX, TileAY, FillTime);
                        ClearAllMatches();
                        StartCoroutine(Filler());
                    }
                    else if (diag == 3)
                    {
                        for (int i = TileA.XPos - 1, j = TileA.YPos - 1; i >= TileB.XPos && j >= TileB.YPos; i--, j--)
                        {
                            MovingTiles.Add(Board[i, j]);
                        }
                        for (int i = 0; i < MovingTiles.Count; i++)
                        {
                            Board[MovingTiles[i].XPos + 1, MovingTiles[i].YPos + 1] = MovingTiles[i];
                            MovingTiles[i].Move(MovingTiles[i].XPos + 1, MovingTiles[i].YPos + 1, FillTime);
                        }
                        Board[TileAX, TileAY] = TileA;
                        TileA.Move(TileAX, TileAY, FillTime);
                        ClearAllMatches();
                        StartCoroutine(Filler());
                    }
                    else
                    {
                        for (int i = TileA.XPos + 1, j = TileA.YPos - 1; i <= TileB.XPos && j >= TileB.YPos; i++, j--)
                        {
                            MovingTiles.Add(Board[i, j]);
                        }
                        for (int i = 0; i < MovingTiles.Count; i++)
                        {
                            Board[MovingTiles[i].XPos - 1, MovingTiles[i].YPos + 1] = MovingTiles[i];
                            MovingTiles[i].Move(MovingTiles[i].XPos - 1, MovingTiles[i].YPos + 1, FillTime);
                        }
                        Board[TileAX, TileAY] = TileA;
                        TileA.Move(TileAX, TileAY, FillTime);
                        ClearAllMatches();
                        StartCoroutine(Filler());
                    }
                }
            }
        }
    }

    public void ClickAndHold(GamePiece Clicked)
    {
        Current = Clicked;
    }

    public void NewSpace(GamePiece Entered)
    {
        Over = Entered;
    }

    public void Release()
    {
        if (Current.Type == Type.APHID)
        {
            if (AreAjacent(Current, Over))
            {
                SwapPieces(Current, Over);
            }
        }
        else if (Current.Type == Type.ANT)
        {
            if (AreDiagonal(Current, Over) != 0)
            {
                SwapPieces(Current, Over);
            }
        }
        else if (Current.Type == Type.GRASSHOPPER)
        {
            if(AreHorizontalOrVertical(Current,Over))
            {
                SwapPieces(Current, Over);
            }
        }
        else if (Current.Type == Type.FLY)
        {
            if(AreHorizontalOrVertical(Current,Over) || AreDiagonal(Current,Over) != 0)
            {
                SwapPieces(Current, Over);
            }
        }
    }

    //bug with ajacent peices moving sometimes even though no match. Trying to figure out where the logic fails.
    public List<GamePiece> CheckMatch(GamePiece ToCheck, int NewX, int NewY)
    {
        if(ToCheck.coloured)
        {
            ColouredPeices.Colour Compare = (ToCheck as ColouredPeices).MyColour;
            List<GamePiece> Horizontal = new List<GamePiece>();
            List<GamePiece> Vertical = new List<GamePiece>();
            List<GamePiece> Matches = new List<GamePiece>();

            Horizontal.Add(ToCheck);

            for(int Direction = 0; Direction <= 1; Direction++)
            {
                for(int Offset = 1; Offset < Xsize; Offset++)
                {
                    int x;
                    if(Direction == 0)
                    {
                        x = NewX - Offset;
                    }
                    else
                    {
                        x = NewX + Offset;
                    }

                    if(x < 0 || x >= Xsize)
                    {
                        break;
                    }
                    if(Board[x,NewY].coloured)
                    {
                        if((Board[x, NewY] as ColouredPeices).MyColour == Compare)
                        {
                            if (!Horizontal.Contains(Board[x, NewY]))
                            {
                                Horizontal.Add(Board[x, NewY]);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if(Horizontal.Count >= 3)
                {
                    for(int i = 0; i < Horizontal.Count; i++)
                    {
                        for (int Offset = 1; Offset < Ysize; Offset++)
                        {
                            int y;
                            if (Direction == 0)
                            {
                                y = NewY - Offset;
                            }
                            else
                            {
                                y = NewY + Offset;
                            }

                            if (y < 0 || y >= Ysize)
                            {
                                break;
                            }
                            if (Board[Horizontal[i].XPos, y].coloured)
                            {
                                if ((Board[Horizontal[i].XPos, y] as ColouredPeices).MyColour == Compare)
                                {
                                    if (!Vertical.Contains(Board[Horizontal[i].XPos, y]))
                                    {
                                        Vertical.Add(Board[Horizontal[i].XPos, y]);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        if (Vertical.Count < 2)
                        {
                            Vertical.Clear();
                        }
                        else
                        {
                            for (int j = 0; j < Vertical.Count; j++)
                            {
                                Matches.Add(Vertical[j]);
                            }
                        }
                        Matches.Add(Horizontal[i]);
                        Vertical.Clear();
                    }
                }

                if (Matches.Count >= 3)
                {
                    return Matches;
                }
            }

            Horizontal.Clear();
            Vertical.Clear();

            Vertical.Add(ToCheck);

            for (int Direction = 0; Direction <= 1; Direction++)
            {
                for (int Offset = 1; Offset < Ysize; Offset++)
                {
                    int y;
                    if (Direction == 0)
                    {
                        y = NewY - Offset;
                    }
                    else
                    {
                        y = NewY + Offset;
                    }

                    if (y < 0 || y >= Ysize)
                    {
                        break;
                    }
                    if (Board[NewX, y].coloured)
                    {
                        if ((Board[NewX, y] as ColouredPeices).MyColour == Compare)
                        {
                            if (!Vertical.Contains(Board[NewX, y]))
                            {
                                Vertical.Add(Board[NewX, y]);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (Vertical.Count >= 3)
                {
                    for (int i = 0; i < Vertical.Count; i++)
                    {
                        for (int Offset = 1; Offset < Ysize; Offset++)
                        {
                            int x;
                            if (Direction == 0)
                            {
                                x = NewX - Offset;
                            }
                            else
                            {
                                x = NewX + Offset;
                            }

                            if (x < 0 || x >= Xsize)
                            {
                                break;
                            }
                            if (Board[x, Vertical[i].YPos].coloured)
                            {
                                if ((Board[x, Vertical[i].YPos] as ColouredPeices).MyColour == Compare)
                                {
                                    if (!Horizontal.Contains(Board[x, Vertical[i].YPos]))
                                    {
                                        Horizontal.Add(Board[x, Vertical[i].YPos]);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        if (Horizontal.Count < 2)
                        {
                            Horizontal.Clear();
                        }
                        else
                        {
                            for (int j = 0; j < Horizontal.Count; j++)
                            {
                                Matches.Add(Horizontal[j]);
                            }
                        }
                        Matches.Add(Vertical[i]);
                        Horizontal.Clear();
                    }
                }
                if (Matches.Count >= 3)
                {
                    return Matches;
                }
            }
        }
        return null;
    }

    public bool ClearAllMatches()
    {
        bool needsfill = false;
        for(int i = 0; i < Xsize; i++)
        {
            for(int j = 0; j < Ysize; j++)
            {
                if (Board[i,j].clearable)
                {
                    List<GamePiece> matches = CheckMatch(Board[i, j], i, j);

                    if(matches != null)
                    {
                        for(int x = 0; x < matches.Count; x++)
                        {
                            if(ClearPeice(matches[x].XPos , matches[x].YPos))
                            {
                                needsfill = true;
                            }
                        }
                    }
                }
            }
        }
        return needsfill;
    }

    public bool ClearPeice(int x, int y)
    {
        if(Board[x,y].clearable && !Board[x,y].BeingCleared)
        {
            Board[x, y].Clear();
            SpawnPieces(x, y, Type.EMPTY);
            return true;
        }
        return false;
    }
}
