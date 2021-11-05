using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridController : MonoBehaviour
{
    GamePiece[,] Board;
    int Xsize;
    int Ysize;

    public enum Type
    {
        ANT,
        GRASSHOPPER,
        FLY,
        APHID,
        ANY,
        COUNT
    }
    public enum Colour
    {
        RED,
        GREEN,
        BLUE,
        YELLOW,
        ANY,
        COUNT
    }

    private Dictionary<Type, GameObject> TypeDictionary;
    private Dictionary<Colour, Material> ColourDictionary;

    [System.Serializable]
    public struct GamePeiceTypePrefabs
    {
        public Type type;
        public GameObject Prefab;
    }

    [System.Serializable]
    public struct GamePeiceColourPrefabs
    {
        public Colour type;
        public Material Prefab;
    }

    public GamePeiceTypePrefabs[] _GamePeiceTypePrefabs;
    public GamePeiceColourPrefabs[] _GamePeiceColourPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        Board = new GamePiece[Xsize, Ysize];
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
