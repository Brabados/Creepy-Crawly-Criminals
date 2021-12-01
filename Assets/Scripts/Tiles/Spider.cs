using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : SpecialPiece
{
    Opponent Opponent;
    public int Damage;
    private void Awake()
    {
        SetDictonary();
        if (isPasive)
        {
            EventManager.current.onSpecial += SpecialAffect;
        }
        Opponent = FindObjectOfType<Opponent>();

        discription = "Creates a Spider tile at selected tiles location of same colour as power." + System.Environment.NewLine +
            "A spider tile destroys an adjacent fly tile to do damage after each turn until it is made part of a match and removed";

    }

    // Update is called once per frame
    public override void SpecialAffect()
    {
        for(int i = -1; i <= 1; i++)
        {
            if(i != 0)
            {
                if(Grid.Board[XPos + i,YPos].Type == GridController.Type.FLY)
                {
                    Grid.Board[XPos + i, YPos].Clear();
                    Grid.SpawnPieces(XPos + i, YPos, GridController.Type.EMPTY);
                    EventManager.current.SpecailDamage(Damage, MyColour);
                }
            }
        }
        for (int i = -1; i <= 1; i++)
        {
            if (i != 0)
            {
                if (Grid.Board[XPos, YPos + i].Type == GridController.Type.FLY)
                {
                    Grid.Board[XPos, YPos + i].Clear();
                    Grid.SpawnPieces(XPos, YPos + i, GridController.Type.EMPTY);
                    EventManager.current.SpecailDamage(Damage, MyColour);
                }
            }
        }
        StartCoroutine(Grid.Filler());
    }

    private void OnDestroy()
    {
        EventManager.current.onSpecial -= SpecialAffect;
    }
}
