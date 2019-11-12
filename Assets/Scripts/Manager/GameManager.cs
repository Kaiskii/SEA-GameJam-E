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
    [SerializeField] float SecondsForEachTurn;
    private float eachShipTimer;
    private float countDownTimer;
    public GameObject explosionPrefab;
    public GameObject DamageLaser;
    public bool isPaused { get; private set; }

    // Managers
    [SerializeField] UIManager uiManager;
    [SerializeField] AudioManager audioManager;
    [SerializeField] TurnManager turnManager;


    List<PlayerController> player1Ships = new List<PlayerController>(); //Ship Size
    List<PlayerController> player2Ships = new List<PlayerController>();
    List<PlayerController> tempplayer1Ships; //Ship Size
    List<PlayerController> tempplayer2Ships;
    public int getMaxShipPerPlayer { get {return Mathf.Max(player1Ships.Count, player2Ships.Count); } }

    List<GameObject> removeList;
    GameObject mainMenuCanvas;

    [Header("Player 1")]
    [SerializeField] GameObject player1Prefab;
    [SerializeField] List<Transform> player1SpawnLocation;

    [Header("Player 2")]
    [SerializeField] GameObject player2Prefab;
    [SerializeField] List<Transform> player2SpawnLocation;

    ResetGameController resetGameController { get { return uiManager.GetPanel<ResetGameController>("ResetGame"); } }
    EndGameController endGameController { get { return uiManager.GetPanel<EndGameController>("EndGame"); } }
    PauseGameController pauseGameController { get { return uiManager.GetPanel<PauseGameController>("PauseGame"); } }

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

    public void RemovePlayerShip(PlayerController controller)
    {
        if (player1Ships.Contains(controller))
        {
            Debug.Log("Remove P1");
            player1Ships.Remove(controller);
        }

        if (player2Ships.Contains(controller))
        {
            Debug.Log("Remove P2");
            player2Ships.Remove(controller);
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

            if (tempplayer1Ships.Count != 0)
            {
                tempplayer1Ships[0].MakeThisChosen();
            }

            if(tempplayer2Ships.Count != 0)
            {
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
            EndGame(0);
        }
        else if(player1Ships.Count <= 0)
        {
            EndGame(2);
        }
        else if(player2Ships.Count <= 0)
        {
            EndGame(1);
        }
    }

    void EndGame(int playerWinNum)
    {
        turnManager.EndGame();
        Debug.LogWarning("END GAME");
        DestroyAllShip();

        endGameController.setPlayerWin(playerWinNum);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseGameController.gameObject.SetActive(true);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseGameController.gameObject.SetActive(false);
    }
}
