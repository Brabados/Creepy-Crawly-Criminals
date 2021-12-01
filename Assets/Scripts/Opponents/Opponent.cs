using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public string _name;
    public string _AsignedBoard;
    public ColouredPeices.Colour _MatchPref;
    private float[] _ColorRatios;
    private float[] _TypeRatios;
    public int TotalHealth;
    public int CurrentHealth;

    void Start()
    {
        CurrentHealth = TotalHealth;
        EventManager.current.onAddPower += DamageOpponent;
        EventManager.current.onSpecialDamage += ExtraDamage;
    }

    public void DamageOpponent(ColouredPeices.Colour colour)
    {
        if(colour == _MatchPref)
        {
            CurrentHealth = CurrentHealth - 2;
        }
        else 
        {
            CurrentHealth--;
        }
    }

    public void ExtraDamage(int amount, ColouredPeices.Colour colour)
    {
        if(colour == _MatchPref)
        {
            CurrentHealth = CurrentHealth - (2 * amount);
        }
        else
        {
            CurrentHealth = CurrentHealth - amount;
        }
    }
}
