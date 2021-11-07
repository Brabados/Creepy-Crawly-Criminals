using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridController : MonoBehaviour
{
    //Array of game pieces to be manipulated later
    GamePiece[,] Board;

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
        ANY,
        COUNT
    }

    //Enum for each tile colour type. 
    public enum Colour
    {
        RED,
        GREEN,
        BLUE,
        YELLOW,
        ANY,
        COUNT
    }

    //Refrence dictonarys for quick lookup
    private Dictionary<Type, GameObject> TypeDictionary;
    private Dictionary<Colour, Material> ColourDictionary;

    //Structs for containing presetup for conversion to dictonary
    [System.Serializable]
    public struct GamePeiceTypePrefabs
    {
        public Type type;
        public GameObject Prefab;
    }

    [System.Serializable]
    public struct GamePeiceColourPrefabs
    {
        public Colour colour;
        public Material Prefab;
    }

    //Inspector side ver of dictonarys to allow for setup
    public GamePeiceTypePrefabs[] _GamePeiceTypePrefabs;
    public GamePeiceColourPrefabs[] _GamePeiceColourPrefabs;



    private void Awake()
    {
      
    }

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
                GameObject Space = (GameObject)Instantiate(SpacePrefab, new Vector3(i + (i * 3), j + (j * 3), 0), Quaternion.identity);

                //Makes space a child of the grid
                Space.transform.parent = transform;
            }
        }

        //Initalizing dictonarys for use in code from the modified inspector arrays
        TypeDictionary = new Dictionary<Type, GameObject>();
        ColourDictionary = new Dictionary<Colour, Material>();

        for (int i = 0; i < _GamePeiceColourPrefabs.Length; i++)
        {
            if (!ColourDictionary.ContainsKey(_GamePeiceColourPrefabs[i].colour))
            {
                ColourDictionary.Add(_GamePeiceColourPrefabs[i].colour, _GamePeiceColourPrefabs[i].Prefab);
            }

        }
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
                GameObject Tile = (GameObject)Instantiate(TypeDictionary[Type.FLY], Vector3.zero, Quaternion.identity);
                Tile.name = "Tile(" + i + "," + j + ")";

                Board[i, j] = Tile.GetComponent<GamePiece>();
                Board[i, j].Mat = Tile.GetComponent<MeshRenderer>();
                Board[i, j].Mat.material = ColourDictionary[Colour.RED]; 
                Board[i, j].Initalize(i,j,this, Type.FLY, Colour.RED);
                
                if (Board[i, j].moveable)
                {
                    Board[i, j].Move(i, j);
                }

                //Makes space a child of the grid
                Tile.transform.parent = transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 Worldposition(int x, int y)
    {
        return new Vector3(x + (x * 3), y + (y * 3), 0);
    }
}
