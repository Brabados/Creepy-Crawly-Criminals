using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    private void Awake()
    {
        current = this;
    }

    public event Action<ColouredPeices.Colour> onAddPower;
    public event Action onSpecial;
    public event Action<ColouredPeices> onTileReplacement;
    public event Action<ColouredPeices.Colour> onPowerDrain;
    public event Action<int, ColouredPeices.Colour> onSpecialDamage;
    public event Action<int> OnTurnMod;

    public void AddPower(ColouredPeices.Colour colour)
    {
        if(onAddPower != null)
        {
            onAddPower(colour);
        }
    }

    public void Special()
    {
        if(onSpecial != null)
        {
            onSpecial();
        }
    }

    public bool TileReplacement(ColouredPeices Change)
    {
        if(onTileReplacement != null)
        {
            onTileReplacement(Change);
            return true;
        }
        return false;
    }

    public void PowerDrain(ColouredPeices.Colour colour)
    {
        if(onPowerDrain != null)
        {
            onPowerDrain(colour);
        }
    }

    public void SpecailDamage(int damage, ColouredPeices.Colour colour)
    {
        if(onSpecialDamage != null)
        {
            onSpecialDamage(damage, colour);
        }
    }

    public void TurnMod(int turn)
    {
        if(OnTurnMod != null)
        {
            OnTurnMod(turn);
        }
    }
}
