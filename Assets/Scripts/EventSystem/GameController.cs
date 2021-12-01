using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int turncount;
    public Opponent Opponent;
    public Text DisplayTurns;
    public Text WinLose;
    private int _state = -1;


    private void Start()
    {
        Opponent = FindObjectOfType<Opponent>();
        EventManager.current.OnTurnMod += turnchange;
    }
    private void Update()
    {
        if(turncount <=0 && _state == -1)
        {
            StartCoroutine(GameOver(0));


        }
        else if(Opponent.CurrentHealth <=0 && _state == -1)
        {
            StartCoroutine( GameOver(1));
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
    public IEnumerator GameOver(int state)
    {
        if (state == 0)
        {
            WinLose.text = "GAME OVER: You Lose";
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
        else
        {
            WinLose.text = "GAME OVER: You Win";
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
        _state = state;
    }
}
