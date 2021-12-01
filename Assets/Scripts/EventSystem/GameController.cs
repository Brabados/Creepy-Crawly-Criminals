using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int turncount;
    public Opponent Opponent;
    public Text DisplayTurns;


    private void Start()
    {
        Opponent = FindObjectOfType<Opponent>();
        EventManager.current.OnTurnMod += turnchange;
    }
    private void Update()
    {
        if(turncount <=0)
        {
            Debug.LogError("GAME OVER: You Lose");
        }
        else if(Opponent.CurrentHealth <=0)
        {
            Debug.LogError("GAME OVER: You Win");
        }
    }

    public void turnchange(int TurnChange)
    {
        int currentTurns = turncount;
        turncount = turncount + TurnChange;
        DisplayTurns.text = turncount.ToString();
        if(turncount < currentTurns)
        {
            EventManager.current.Special();
        }
    }
}
