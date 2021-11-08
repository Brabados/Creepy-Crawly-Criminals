using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridController : MonoBehaviour
{
    //Array of game pieces to be manipulated later
    public GamePiece[,] Board;

    //Inspector side board size veriables, may update for prebuilt levels later
    public int Xsize;
    public int Ysize;

    //Prefab for the spaces the peices can sit on. Currently using place holder object.
    public GameObject SpacePrefab;

    //Enum for each basic tile type. Will need to be extended for special tiles
    public enum Type
    {
        ANT,
        GRASSHOPPER,
        FLY,
        APHID,
        EMPTY,
        ANY,
        COUNT
    }

    //Enum for each tile colour type. 


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
        //Storage for each gamepiece. May modify for prebuilt levels
        Board = new GamePiece[Xsize, Ysize];

        //Sets blank spaces for the game board
        for (int i = 0; i < Xsize; i++)
        {
            for (int j = 0; j < Ysize; j++)
            {
                //Instanciates an instance of each space and puts it in the correct location on screen with offset
                GameObject Space = (GameObject)Instantiate(SpacePrefab, new Vector3(i + (i * 3), j + (j * 3), -1), Quaternion.identity);

                //Makes space a child of the grid
                Space.transform.parent = transform;
            }
        }

        //Initalizing dictonarys for use in code from the modified inspector arrays
        TypeDictionary = new Dictionary<Type, GameObject>();

        for (int i = 0; i < _GamePeiceTypePrefabs.Length; i++)
        {
            if (!TypeDictionary.ContainsKey(_GamePeiceTypePrefabs[i].type))
            {
                TypeDictionary.Add(_GamePeiceTypePrefabs[i].type, _GamePeiceTypePrefabs[i].Prefab);
            }
        }

                //Sets blank spaces for the game board
        for (int i = 0; i < Xsize; i++)
        {
            for (int j = 0; j < Ysize; j++)
            {
                //Instanciates an instance of each space and puts it in the correct location on screen with offset
                SpawnPieces(i, j, Type.EMPTY);
              
                
                if (Board[i, j].moveable)
                {
                    Board[i, j].Move(i, j);
                }
               

                //Makes space a child of the grid
                
            }
        }
        Filler();
    }

    public void Filler()
    {
        while(FillCheck())
        {

        }
    }

    public bool FillCheck()
    {
        bool peiceMoved = false;

        for (int y = Ysize - 2; y >= 0; y--)
        {
            for (int x = 0; x < Xsize; x++)
            {
                GamePiece CurrentPiece = Board[x, y];

                if(CurrentPiece.moveable)
                {
                    GamePiece PieceBellow = Board[x, y + 1];
                    if(PieceBellow.Type == Type.EMPTY)
                    {
                        CurrentPiece.Move(x, y + 1);
                        Board[x, y + 1] = CurrentPiece;
                        SpawnPieces(x, y, Type.EMPTY);
                        Destroy(PieceBellow.gameObject);
                        peiceMoved = true;
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
                Board[x, 0].Move(x, 0);
                (Board[x, 0] as ColouredPeices).AsignColour((ColouredPeices.Colour)Random.Range(0, 4));
                Destroy(PieceBellow.gameObject);
                peiceMoved = true;
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
        if (type != Type.EMPTY)
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
}
