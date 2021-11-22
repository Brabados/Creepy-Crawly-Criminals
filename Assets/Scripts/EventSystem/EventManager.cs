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

    public void AddPower(ColouredPeices.Colour colour)
    {
        if(onAddPower != null)
        {
            onAddPower(colour);
        }
    }
}
