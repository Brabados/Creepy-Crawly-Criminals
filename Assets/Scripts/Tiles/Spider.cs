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
                    Destroy(Grid.Board[XPos + i, YPos].gameObject);
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
                    Destroy(Grid.Board[XPos, YPos + i].gameObject);
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
