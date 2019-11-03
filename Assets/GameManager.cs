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
    public List<PlayerController> player1Ships; //Ship Size
    public List<PlayerController> player2Ships;
    public float SecondsForEachTurn;
    public List<PlayerController> tempplayer1Ships; //Ship Size
    public List<PlayerController> tempplayer2Ships;
    private float eachShipTimer;
    private float countDownTimer;

    [SerializeField] AudioManager audioManager;
    [SerializeField] TurnManager turnManager;

    [Header("Player 1")]
    [SerializeField] GameObject player1Prefab;
    [SerializeField] List<Transform> player1SpawnLocation;

    [Header("Player 2")]
    [SerializeField] GameObject player2Prefab;
    [SerializeField] List<Transform> player2SpawnLocation;

    GameObject mainMenuCanvas;

    #region Initializing
    public void InitializeManager()
    {
        if(turnManager == null)
        {
            turnManager = Toolbox.Instance.FindManager<TurnManager>();
        }
        turnManager.stateMachine.OnChangeStateEvent += CheckChangeStateIdleToPlanning;
        turnManager.stateMachine.OnChangeStateEvent += CheckChangeStateRepeatToPlanning;
        turnManager.stateMachine.OnChangeStateEvent += CheckChangeStatePlanningToCountdown;
        turnManager.stateMachine.OnChangeStateEvent += CheckChangeStateCountdownToExecution;
        turnManager.stateMachine.OnChangeStateEvent += CheckChangeStateExecutionToPlanning;

        turnManager.stateMachine.RegisterOnExit(TurnState.Execution, CheckEndGame);
    }

    private void Start()
    {
        audioManager.PlayAudioClip("mainMenuBGM", AudioManager.ClipType.BGM);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            audioManager.PlayAudioClip("Click", AudioManager.ClipType.SFX);
    }
    #endregion

    void SetToAvailableShips()
    {
        tempplayer1Ships = new List<PlayerController>(player1Ships);
        tempplayer2Ships = new List<PlayerController>(player2Ships);
    }
    
    public void StartGame(GameObject mainMenuCanvas)
    {
        InstantiateShip();
        SetToAvailableShips();

        this.mainMenuCanvas = mainMenuCanvas;
        this.mainMenuCanvas.SetActive(false);

        turnManager.StartGame();
        audioManager.PlayAudioClip("inGameBGM", AudioManager.ClipType.BGM);
    }

    public void ResetGame()
    {
        DestroyAllShip();

        StartGame(mainMenuCanvas);
    }

    public void InstantiateShip()
    {
        player1Ships.Clear();
        player2Ships.Clear();
        foreach(Transform spawnLocation in player1SpawnLocation)
        {
            Instantiate(player1Prefab, spawnLocation).transform.parent = null;
            PlayerController playerController = player1Prefab.GetComponent<PlayerController>();
            player1Ships.Add(playerController);
        }

        foreach(Transform spawnLocation in player2SpawnLocation)
        {
            Instantiate(player1Prefab, spawnLocation).transform.parent = null;
            PlayerController playerController = player2Prefab.GetComponent<PlayerController>();
            player2Ships.Add(playerController);
        }
    }

    public void DestroyAllShip()
    {
        foreach(PlayerController controller in player1Ships)
        {
            Destroy(controller.gameObject);
        }
        foreach(PlayerController controller in player2Ships)
        {
            Destroy(controller.gameObject);
        }

        player1Ships.Clear();
        player2Ships.Clear();
    }


    void CheckChangeStateIdleToPlanning(TurnState prevState, TurnState nextState)
    {
        if (prevState == TurnState.Idle && nextState == TurnState.Planning)
        {
            if (tempplayer1Ships.Count != 0)
            {
                tempplayer1Ships[0].MakeThisChosen();
            }
            if (tempplayer2Ships.Count != 0)
            {
                tempplayer2Ships[0].MakeThisChosen();
            }
            else
            {
                foreach (PlayerController obj in tempplayer1Ships)
                {
                    obj.goToRecording = true;
                }
                foreach (PlayerController obj in tempplayer2Ships)
                {
                    obj.goToRecording = true;
                }
            }
        }
    }

    void CheckChangeStateRepeatToPlanning(TurnState prevState, TurnState nextState)
    {
        if (prevState == TurnState.Planning && nextState == TurnState.Planning)
        {
            if (tempplayer1Ships.Count > 0)
            {
                tempplayer1Ships[0].EndTurn();
                tempplayer1Ships.RemoveAt(0);
            }
            if (tempplayer2Ships.Count > 0)
            {
                tempplayer2Ships[0].EndTurn();

                tempplayer2Ships.RemoveAt(0);

            }


            if (tempplayer1Ships.Count != 0 && tempplayer2Ships.Count != 0)
            {
                tempplayer1Ships[0].MakeThisChosen();
                tempplayer2Ships[0].MakeThisChosen();
            }



        }
    }

    void CheckChangeStatePlanningToCountdown(TurnState prevState, TurnState nextState)
    {
        if (prevState == TurnState.Planning && nextState == TurnState.Countdown)
        {

            if (tempplayer1Ships.Count > 0) tempplayer1Ships[0].EndTurn();

            if (tempplayer2Ships.Count > 0) tempplayer2Ships[0].EndTurn();


            if (tempplayer1Ships.Count != 0)
            {
                tempplayer1Ships.RemoveAt(0);
            }
            if (tempplayer2Ships.Count != 0)
            {

                tempplayer2Ships.RemoveAt(0);
            }

            foreach (PlayerController obj in player1Ships)
            {
                obj.goToRecording = true;
                Debug.Log("1");
            }
            foreach (PlayerController obj in player2Ships)
            {
                obj.goToRecording = true;
            }


        }
    }

    void CheckChangeStateCountdownToExecution(TurnState prevState, TurnState nextState)
    {
        if (prevState == TurnState.Countdown && nextState == TurnState.Execution)
        {
            SetToAvailableShips();

            foreach (PlayerController obj in tempplayer1Ships)
            {
                obj.dummyPlayer.SetActive(false);

            }
            foreach (PlayerController obj in tempplayer2Ships)
            {
                obj.dummyPlayer.SetActive(false);
            }
        }
    }

    void CheckChangeStateExecutionToPlanning(TurnState prevState, TurnState nextState)
    {
        if (prevState == TurnState.Execution && nextState == TurnState.Planning)
        {
            SetToAvailableShips();
            foreach (PlayerController obj in tempplayer1Ships)
            {
                obj.goToRecording = false;
            }
            foreach (PlayerController obj in tempplayer2Ships)
            {
                obj.goToRecording = false;
            }

            if (tempplayer1Ships.Count != 0)
            {
                tempplayer1Ships[0].MakeThisChosen();
            }
            if (tempplayer2Ships.Count != 0)
            {
                tempplayer2Ships[0].MakeThisChosen();
            }
            else
            {   
                foreach (PlayerController obj in tempplayer1Ships)
                {

                    obj.goToRecording = true;
                }
                foreach (PlayerController obj in tempplayer2Ships)
                {
                    obj.goToRecording = true;
                }
            }
        }
    }   

    void CheckEndGame()
    {
        if(player1Ships.Count <= 0)
        {
            turnManager.EndGame();
            Debug.LogWarning("END GAME");
        }
        else if(player2Ships.Count <= 0)
        {
            turnManager.EndGame();
            Debug.LogWarning("END GAME");
        }
    }
}
