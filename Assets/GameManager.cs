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

    private void Start()
    {

        tempplayer1Ships = new List<GameObject>(player1Ships);
        tempplayer2Ships = new List<GameObject>(player2Ships);

        

    }
    public void StartGame()
    {
        turnManager.StartGame();


    }

    [SerializeField] TurnManager turnManager;

    private void Update()
    {
       
    }

    public void InitializeManager()
    {
        if(turnManager == null)
        {
            turnManager = Toolbox.Instance.FindManager<TurnManager>();
        }
        turnManager.stateMachine.OnChangeStateEvent += CheckForChangeState;
    }



    void CheckForChangeState(TurnState prevState, TurnState nextState)
    {
        if(prevState==TurnState.Idle && nextState==TurnState.Planning)
        {
            if (player1Ships.Count != 0 && player2Ships.Count != 0)
            {
                player1Ships[0].GetComponent<PlayerController>().MakeThisChosen();
                player2Ships[0].GetComponent<PlayerController>().MakeThisChosen();
                
                
               

            }
            else
            {
                foreach (GameObject obj in tempplayer1Ships)
                {

                    obj.GetComponent<PlayerController>().goToRecording = true;
                }
                foreach (GameObject obj in tempplayer2Ships)
                {
                    obj.GetComponent<PlayerController>().goToRecording = true;
                }
            }
        }
        else if(prevState == TurnState.Planning && nextState == TurnState.Planning)
        {
            if(player1Ships.Count>0 && player2Ships.Count>0)
            {
                player1Ships[0].GetComponent<PlayerController>().EndTurn();
                player2Ships[0].GetComponent<PlayerController>().EndTurn();
                player1Ships.RemoveAt(0);
                player2Ships.RemoveAt(0);
                player1Ships[0].GetComponent<PlayerController>().MakeThisChosen();
                player2Ships[0].GetComponent<PlayerController>().MakeThisChosen();
            }
            
        }
        if (prevState == TurnState.Countdown && nextState == TurnState.Execution)
        {
            player1Ships[0].GetComponent<PlayerController>().EndTurn();
            player2Ships[0].GetComponent<PlayerController>().EndTurn();
            player1Ships.RemoveAt(0);
            player2Ships.RemoveAt(0);
            foreach (GameObject obj in tempplayer1Ships)
            {
                obj.GetComponent<PlayerController>().goToRecording = true;
            }
            foreach (GameObject obj in tempplayer2Ships)
            {
                obj.GetComponent<PlayerController>().goToRecording = true;
            }


        }


    }

    
    
    public GameObject templateTrail;
    void DoAction()
    {
       
        
       
            
       

    }
    
   

   
}
