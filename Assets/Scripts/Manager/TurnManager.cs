﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotEngine;
using Sirenix.OdinInspector;

public enum TurnState
{
    Idle,
    Planning,
    Countdown,
    Execution
}

public class TurnManager : MonoBehaviour, IManager
{
    public EnumStateMachine<TurnState> stateMachine = new EnumStateMachine<TurnState>();
    public TurnState currentState { get { return stateMachine.currentState; } }

    [Header("Serialize Time")]
    [SerializeField] public float planningPhaseTime;
    [SerializeField] public float countdownPhaseTime;
    [SerializeField] public float executionPhaseTime;
    public float currentCountdown { get; private set; }

    GameManager gameManager { get { return Toolbox.Instance.FindManager<GameManager>(); } }
    public int numberOfShips { get { return gameManager.getMaxShipPerPlayer; } }
    private int currentShipAccessed;

    public void InitializeManager()
    {
        stateMachine.AddStateRules(TurnState.Idle, new HashSet<TurnState>() { TurnState.Planning });
        stateMachine.AddStateRules(TurnState.Planning, new HashSet<TurnState>() { TurnState.Countdown, TurnState.Idle, TurnState.Planning });
        stateMachine.AddStateRules(TurnState.Countdown, new HashSet<TurnState>() { TurnState.Execution, TurnState.Idle });
        stateMachine.AddStateRules(TurnState.Execution, new HashSet<TurnState>() { TurnState.Planning, TurnState.Idle });

        stateMachine.OnChangeStateEvent += OnChangeStateCountdownUpdate;
    }

    void OnChangeStateCountdownUpdate(TurnState prevState, TurnState nextState)
    {
        switch(nextState)
        {
            case TurnState.Planning:
                currentCountdown = planningPhaseTime;
                break;

            case TurnState.Countdown:
                currentCountdown = countdownPhaseTime;
                break;

            case TurnState.Execution:
                currentCountdown = executionPhaseTime;
                break;
        }
    }


    [Button]
    public void StartGame()
    {
        stateMachine.SafeChangeState(TurnState.Planning);
    }


    #region Core Game Loop
    /// <summary>
    /// Start Player Countdown
    /// </summary
    [Button]
    public void EndTurn()
    {
        currentShipAccessed++;
        if(currentShipAccessed < numberOfShips)
        {
            stateMachine.SafeChangeState(TurnState.Planning);
        }
        else
        {
            currentShipAccessed = 0;
            stateMachine.SafeChangeState(TurnState.Countdown);
        }
    }

    public void EnterExecution()
    {
        stateMachine.SafeChangeState(TurnState.Execution);
    }

    public void NextTurn()
    {
        stateMachine.SafeChangeState(TurnState.Planning);
    }

    public void EndGame()
    {
        stateMachine.SafeChangeState(TurnState.Idle);
    }
    #endregion Core Game Loop

    private void Update()
    {
        //Debug.Log(currentState);

        if (currentState == TurnState.Idle || gameManager.isPaused) return;

        // Addressing Time up scenario
        currentCountdown -= Time.deltaTime;
        if(currentCountdown <= 0)
        {
            switch(currentState)
            {
                case TurnState.Planning:
                    EndTurn();
                    break;

                case TurnState.Countdown:
                    EnterExecution();
                    break;

                case TurnState.Execution:
                    NextTurn();
                    break;
            }
        }
    }

}
