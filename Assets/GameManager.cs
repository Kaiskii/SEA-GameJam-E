using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotEngine;

public enum PlayerNumber
{
    NUMBER1,NUMBER2,
}


public class GameManager : MonoBehaviour, IManager
{    
    public List<GameObject> player1Ships; //Ship Size
    public List<GameObject> player2Ships;
    public float SecondsForEachTurn;
    public List<GameObject> tempplayer1Ships; //Ship Size
    public List<GameObject> tempplayer2Ships;
    private float eachShipTimer;
    private float countDownTimer;
    private int roundsPlayed = 0;
    bool didCountDown = false;

    [SerializeField] TurnManager turnManager;

    private void Update()
    {
        CheckForTurns();
    }

    public void InitializeManager()
    {
        if(turnManager == null)
        {
            turnManager = Toolbox.Instance.FindManager<TurnManager>();
        }
    }


    void CheckForTurns()
    {
        if(turnManager.currentState==TurnState.Planning)
        {
            eachShipTimer += Time.deltaTime;
            int seconds = (int)(eachShipTimer % 60);
            
            if (seconds > SecondsForEachTurn)
            {
                eachShipTimer = 0;
                DoAction();
                roundsPlayed++;
            }
        }
       else if (turnManager.currentState == TurnState.Countdown)
        {
           /* countDownTimer += Time.deltaTime;
            int seconds = (int)(countDownTimer % 60);

            if (seconds > 0.5)
            {
                countDownTimer = 0;
                DoAction();
                roundsPlayed++;
            }*/
        }

    }

    public GameObject templateTrail;
    void DoAction()
    {
       
        
        if (player1Ships.Count != 0 && player2Ships.Count != 0)
        {
            player1Ships[0].GetComponent<PlayerController>().EndTurn();
            player2Ships[0].GetComponent<PlayerController>().EndTurn();
            player1Ships.RemoveAt(0);
            player2Ships.RemoveAt(0);
            player1Ships[0].GetComponent<PlayerController>().MakeThisChosen();
            player2Ships[0].GetComponent<PlayerController>().MakeThisChosen();
        }
            
        else
        {
            foreach(GameObject obj in tempplayer1Ships)
            {
                
                obj.GetComponent<PlayerController>().goToRecording = true;
            }
            foreach (GameObject obj in tempplayer2Ships)
            {
                obj.GetComponent<PlayerController>().goToRecording = true;
            }
        }

    }
    
    public void StartGame()
    {
        turnManager.StartGame();
        player1Ships[0].GetComponent<PlayerController>().MakeThisChosen();
        player2Ships[0].GetComponent<PlayerController>().MakeThisChosen();

    }

    private void Start() {
       

       
        
    }
}
