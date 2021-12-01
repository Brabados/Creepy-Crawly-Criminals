using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTurns : PowerBase
{
    public int AddingTurns;

    private void Start()
    {
        MyDiscription = "Adds 3 extra turns.";
    }
    public override void Select()
    {
        if (PowerPoints.CheckValue(AssignedColour) >= Cost)
        {
            EventManager.current.TurnMod(AddingTurns);
        }
    }

}
