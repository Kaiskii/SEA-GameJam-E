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
    public static GameManager instance;
    private float countDownTimer;
    void SetToAvailableShips()
    {
        tempplayer1Ships = new List<GameObject>(player1Ships);
        tempplayer2Ships = new List<GameObject>(player2Ships);

<<<<<<< HEAD
    }
    private void Awake()
    {
        instance = this;
    }
=======
    AudioManager am;

>>>>>>> 5bcff935c7694cd160b156a20c313077a6b9c96f
    private void Start()
    {
        SetToAvailableShips();


<<<<<<< HEAD
=======
        if(am == null)
            am = Toolbox.Instance.FindManager<AudioManager>();

        am.PlayAudioClip("mainMenuBGM", AudioManager.ClipType.BGM);

        tempplayer1Ships = new List<GameObject>(player1Ships);
        tempplayer2Ships = new List<GameObject>(player2Ships);
>>>>>>> 5bcff935c7694cd160b156a20c313077a6b9c96f


    }
    
    
    
    public void StartGame()
    {
        turnManager.StartGame();
        am.PlayAudioClip("inGameBGM", AudioManager.ClipType.BGM);
    }

    [SerializeField] TurnManager turnManager;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            am.PlayAudioClip("Click", AudioManager.ClipType.SFX);
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
        
        if (prevState == TurnState.Idle && nextState == TurnState.Planning)
        {
            if (tempplayer1Ships.Count != 0 )
            {
                tempplayer1Ships[0].GetComponent<PlayerController>().MakeThisChosen();
            }
            if (tempplayer2Ships.Count != 0)
            {        
                tempplayer2Ships[0].GetComponent<PlayerController>().MakeThisChosen();
              
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
        else if (prevState == TurnState.Planning && nextState == TurnState.Planning)
        {
            if (tempplayer1Ships.Count > 0 )
            {
                tempplayer1Ships[0].GetComponent<PlayerController>().EndTurn();
                tempplayer1Ships.RemoveAt(0);
            }
            if(tempplayer2Ships.Count> 0 )
            {
                tempplayer2Ships[0].GetComponent<PlayerController>().EndTurn();

                tempplayer2Ships.RemoveAt(0);

            }
                
                
                if(tempplayer1Ships.Count!=0 && tempplayer2Ships.Count!=0)
                {
                    tempplayer1Ships[0].GetComponent<PlayerController>().MakeThisChosen();
                    tempplayer2Ships[0].GetComponent<PlayerController>().MakeThisChosen();
                }
                
            

        }
        else if (prevState == TurnState.Planning && nextState == TurnState.Countdown)
        {
            
            if (tempplayer1Ships.Count > 0) tempplayer1Ships[0].GetComponent<PlayerController>().EndTurn();

            if (tempplayer2Ships.Count > 0) tempplayer2Ships[0].GetComponent<PlayerController>().EndTurn();


            if (tempplayer1Ships.Count != 0)
            {
                tempplayer1Ships.RemoveAt(0);
            }
            if ( tempplayer2Ships.Count!=0)
            {
                
                tempplayer2Ships.RemoveAt(0);
            }
            
            foreach (GameObject obj in player1Ships)
            {
                obj.GetComponent<PlayerController>().goToRecording = true;
                Debug.Log("1");
            }
            foreach (GameObject obj in player2Ships)
            {
                obj.GetComponent<PlayerController>().goToRecording = true;
            }


        }
        else if (prevState == TurnState.Countdown && nextState == TurnState.Execution)
        {

            SetToAvailableShips();

            foreach (GameObject obj in tempplayer1Ships)
            {
                obj.GetComponent<PlayerController>().dummyPlayer.SetActive(false);

            }
            foreach (GameObject obj in tempplayer2Ships)
            {
                obj.GetComponent<PlayerController>().dummyPlayer.SetActive(false);
            }

            


        }

        else if (prevState == TurnState.Execution && nextState == TurnState.Planning)
        {

            SetToAvailableShips();
            foreach (GameObject obj in tempplayer1Ships)
            {

                obj.GetComponent<PlayerController>().goToRecording = false;
            }
            foreach (GameObject obj in tempplayer2Ships)
            {
                obj.GetComponent<PlayerController>().goToRecording = false;
            }



            if (tempplayer1Ships.Count != 0)
            {
                tempplayer1Ships[0].GetComponent<PlayerController>().MakeThisChosen();
            }
            if (tempplayer2Ships.Count != 0)
            {
                tempplayer2Ships[0].GetComponent<PlayerController>().MakeThisChosen();

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
    }

    
    
    public GameObject templateTrail;
    void DoAction()
    {
       
        
       
            
       

    }
   

   
}
