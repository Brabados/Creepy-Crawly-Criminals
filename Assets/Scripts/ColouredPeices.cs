using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredPeices : GamePiece
{
    public MeshRenderer _Mat;
    public enum Colour
    {
        RED,
        GREEN,
        BLUE,
        YELLOW,
        ANY,
        COUNT
    }

    [System.Serializable]
    public struct GamePeiceColourPrefabs
    {
        public Colour colour;
        public Material Prefab;
    }

    public GamePeiceColourPrefabs[] _GamePeiceColourPrefabs;

    private Dictionary<Colour, Material> ColourDictionary;

    private Colour _Colour;

    public Colour MyColour
    {
        get { return _Colour; }
        set { AsignColour(value); }
    }

    public int NumColours
    {
        get { return _GamePeiceColourPrefabs.Length; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        ColourDictionary = new Dictionary<Colour, Material>();

        for (int i = 0; i < _GamePeiceColourPrefabs.Length; i++)
        {
            if (!ColourDictionary.ContainsKey(_GamePeiceColourPrefabs[i].colour))
            {
                ColourDictionary.Add(_GamePeiceColourPrefabs[i].colour, _GamePeiceColourPrefabs[i].Prefab);
            }

        }
        coloured = true;
    }

    public void AsignColour(Colour Asign)
    {
        _Colour = Asign;

        if(ColourDictionary.ContainsKey(Asign))
        {
            _Mat.material = ColourDictionary[Asign];
        }
    }

    public void Initalize(int x, int y, GridController grid, GridController.Type type, MeshRenderer Mat)
    {
        base.Initalize(x, y, grid, type);
        _Mat = Mat;
    }

}
