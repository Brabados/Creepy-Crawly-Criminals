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

    public void Awake()
    {
       AssignedColour = (ColouredPeices.Colour)Activator.GetComponent<PowerButton>().Power;
    }
    public virtual void Select()
    {

    }
}
