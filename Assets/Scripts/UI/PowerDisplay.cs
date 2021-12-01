using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerDisplay : MonoBehaviour
{
    public Text RedPowerPoints;
    public Text GreenPowerPoints;
    public Text BluePowerPoints;
    public Text YellowPowerPoints;

    public Text RedPowerDiscription;
    public Text GreenPowerDiscription;
    public Text BluePowerDiscription;
    public Text YellowPowerDiscription;

    public Button red;
    public Button blue;
    public Button green;
    public Button yellow;

    private PowerBase _red;
    private PowerBase _green;
    private PowerBase _blue;
    private PowerBase _yellow;

    public Opponent Opp;
    public GameController con;
    public PowerManager pow;

    public Text RemainingHP;

    // Start is called before the first frame update
    void Start()
    {
        _red = red.GetComponent<PowerBase>();
        _green = green.GetComponent<PowerBase>();
        _blue = blue.GetComponent<PowerBase>();
        _yellow = yellow.GetComponent<PowerBase>();

    }

    // Update is called once per frame
    void Update()
    {
        RedPowerDiscription.text = _red.MyDiscription;
        BluePowerDiscription.text = _blue.MyDiscription;
        GreenPowerDiscription.text = _green.MyDiscription;
        YellowPowerDiscription.text = _yellow.MyDiscription;
        RedPowerPoints.text = "Red Power: " + pow.red + "/" + _red.Cost;
        GreenPowerPoints.text = "Green Power: " + pow.green + "/" + _green.Cost;
        BluePowerPoints.text = "Blue Power: " + pow.blue + "/" + _blue.Cost;
        YellowPowerPoints.text = "Yellow Power: " + pow.yellow + "/" + _yellow.Cost;
        RemainingHP.text = "Enemy Helth: " + Opp.CurrentHealth;
    }
}
