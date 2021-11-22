using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{

    public int red, green, blue, yellow = 0;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.current.onAddPower += AddPower;
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
}

