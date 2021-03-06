using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBase : MonoBehaviour
{
    private string _Name;
    public string MyDiscription;
    public int Cost;
    public PowerManager PowerPoints;

    public string Name
    {
        get { return _Name; }
    }


    public bool SelectTile;
    public bool Activate;
    public ColouredPeices.Colour AssignedColour;
    public Button Activator;

    public void Awake()
    {
        PowerButton power = Activator.gameObject.GetComponent<PowerButton>();
        AssignedColour = power.Power;
        PowerPoints = FindObjectOfType<PowerManager>();
        Activator.onClick.AddListener(delegate { Select(); });
    }
    public virtual void Select()
    {

    }


}
