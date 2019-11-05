using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotEngine;
using Sirenix.OdinInspector;

public enum PlayerNumber
{
    NUMBER1,NUMBER2,
}


public class GameManager : MonoBehaviour, IManager
{    
    List<PlayerController> player1Ships; //Ship Size
    List<PlayerController> player2Ships;
    [SerializeField] float SecondsForEachTurn;
    [HideInInspector] public List<PlayerController> tempplayer1Ships; //Ship Size
    [HideInInspector] public List<PlayerController> tempplayer2Ships;
    private float eachShipTimer;
    private float countDownTimer;
    public GameObject explostionPrefab;
    public GameObject DamageLaser;
    [SerializeField] AudioManager audioManager;
    [SerializeField] TurnManager turnManager;
    [SerializeField] GameObject resetGameCanvas;
    [SerializeField] EndGameController endGameController;
    [SerializeField] PauseGameController pauseGameController;

    List<GameObject> removeList;
    GameObject mainMenuCanvas;

    [Header("Player 1")]
    [SerializeField] GameObject player1Prefab;
    [SerializeField] List<Transform> player1SpawnLocation;

    [Header("Player 2")]
    [SerializeField] GameObject player2Prefab;
    [SerializeField] List<Transform> player2SpawnLocation;


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

        turnManager.stateMachine.RegisterOnEnter(TurnState.Planning, CheckEndGame);
        turnManager.stateMachine.RegisterOnExit(TurnState.Execution, CheckEndGame);
    }

    private void Start()
    {
        audioManager.PlayAudioClip("mainMenuBGM", AudioManager.ClipType.BGM);
        removeList = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            audioManager.PlayAudioClip("Click", AudioManager.ClipType.SFX);

        if(Input.GetKeyDown(KeyCode.Escape) && turnManager.currentState != TurnState.Idle)
        {
            PauseGame();
        }
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

    [Button]
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
            PlayerController instantiatedObject = Instantiate(player1Prefab, spawnLocation).GetComponent<PlayerController>();
            instantiatedObject.transform.parent = null;
            PlayerController playerController = instantiatedObject.GetComponent<PlayerController>();
            player1Ships.Add(playerController);
        }

        foreach(Transform spawnLocation in player2SpawnLocation)
        {
            PlayerController instantiatedObject = Instantiate(player2Prefab, spawnLocation).GetComponent<PlayerController>();
            instantiatedObject.transform.parent = null;
            PlayerController playerController = instantiatedObject.GetComponent<PlayerController>();
            player2Ships.Add(playerController);
        }
    }


    public void TriggerTutorial(GameObject canvas) {
        canvas.GetComponent<Animator>().SetTrigger("Play");
    }

    public void ExitTutorial(GameObject canvas) {
        canvas.GetComponent<Animator>().SetTrigger("End");
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
        foreach (GameObject gj in removeList)
        {
            Destroy(gj.gameObject);
        }

        removeList.Clear();
        player1Ships.Clear();
        player2Ships.Clear();
    }

    public void RemovePlayerShip(PlayerController controller, int playerNum)
    {
        switch(playerNum)
        {
            case 1:
                if (player1Ships.Contains(controller))
                { player1Ships.Remove(controller); }
                break;

            case 2:
                if (player2Ships.Contains(controller))
                { player2Ships.Remove(controller); }
                break;
        }
    }

    public void AddDummyPlayer(GameObject dummy)
    {
        removeList.Add(dummy);
    }

    #region Change State Functions
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
    #endregion

    void CheckEndGame()
    {
        if(player1Ships.Count <= 0 && player2Ships.Count <= 0)
        {
            turnManager.EndGame();
            EndGame(0);
        }
        else if(player1Ships.Count <= 0)
        {
            turnManager.EndGame();
            EndGame(2);
        }
        else if(player2Ships.Count <= 0)
        {
            turnManager.EndGame();
            EndGame(1);
        }
    }

    void EndGame(int playerWinNum)
    {
        Debug.LogWarning("END GAME");
        DestroyAllShip();

        switch(playerWinNum)
        {
            case 0:
                Debug.LogWarning("TIE GAME");
                endGameController.setPlayerWin(0);
                break;

            case 1:
                Debug.LogWarning("PLAYER 1 WIN");
                endGameController.setPlayerWin(1);
                break;

            case 2:
                Debug.LogWarning("PLAYER 2 WIN");
                endGameController.setPlayerWin(2);
                break;
        }

    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseGameController.gameObject.SetActive(true);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        pauseGameController.gameObject.SetActive(false);
    }
}
