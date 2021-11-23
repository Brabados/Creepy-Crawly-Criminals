using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBase : MonoBehaviour
{
    private string _Name;
    private string _Discription;
    private int _Cost;
    public bool SelectTile;
    public bool Activate;
    public ColouredPeices.Colour AssignedColour;
    public Button Activator;

    public virtual void Select()
    {

    }

    public virtual void Activation()
    {

    }
}
