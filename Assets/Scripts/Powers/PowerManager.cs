using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{

    public int red, green, blue, yellow = 0;
    public GridController Grid;
    // Start is called before the first frame update
    void Start()
    {
        Grid = FindObjectOfType<GridController>();
        EventManager.current.onAddPower += AddPower;
        EventManager.current.onTileReplacement += UsePower;
    }

    private void AddPower(ColouredPeices.Colour colour)
    {
        switch ((int)colour)
        {
            case 0:
                red++;
                break;
            case 1:
                green++;
                break;
            case 2:
                blue++;
                break;
            case 3:
                yellow++;
                break;
            default:
                break;
        }      
    }

    public int CheckValue(ColouredPeices.Colour colour)
    {
        
        switch ((int)colour)
        {
            case 0:
                return red; ;
            case 1:
                return green; ;
            case 2:
                return blue; ;
            case 3:
                return yellow;
            default:
                return 0;
        }
    }

    public void UsePower(ColouredPeices Pcolour)
    {
        ColouredPeices.Colour colour = Pcolour.MyColour;
        switch ((int)colour)
        {
            case 0:
                red -= Grid.Red.GetComponent<PowerBase>().Cost;
                break;
            case 1:
                green -= Grid.Green.GetComponent<PowerBase>().Cost; ;
                break; 
            case 2:
                blue -= Grid.Blue.GetComponent<PowerBase>().Cost; ;
                break;
            case 3:
                yellow -= Grid.Yellow.GetComponent<PowerBase>().Cost; ;
                break;
            default:
                break;
        }
    }
}

