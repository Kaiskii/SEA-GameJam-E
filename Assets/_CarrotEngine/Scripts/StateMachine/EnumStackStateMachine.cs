using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CarrotEngine
{
    public class EnumStackStateMachine<T> where T : Enum
    {
        EnumStateMachine<T> stateMachine;

        Stack<T> stateStack = new Stack<T>();

        public EnumStackStateMachine()
        {
            stateMachine = new EnumStateMachine<T>();
        }

        public EnumStackStateMachine(T defaultState)
        {
            stateMachine = new EnumStateMachine<T>(defaultState);
        }

        #region Stack Function
        public void ResetStack(T firstState)
        {
            stateStack.Clear();
            stateStack.Push(firstState);
            stateMachine.ForceChangeState(firstState);
        }

        public void SafePushState(T state)
        {
            if(stateMachine.SafeChangeState(state))
            {
                stateStack.Push(state);
            }
        }

        public void ForcePushState(T state)
        {
            stateStack.Push(state);
            stateMachine.ForceChangeState(state);
        }

        public void SafePopState()
        {
            if (stateStack.Count > 1)
            {
                T prevState = stateStack.Pop();
                T nextState = stateStack.Peek();

                if (!stateMachine.SafeChangeState(nextState))
                {
                    stateStack.Push(prevState);
                }
            }
            else
            {
                ConsoleDebugger.LogError("Stack not enough to pop state");
            }
        }

        public void ForcePopState()
        {
            if(stateStack.Count > 1)
            {
                stateStack.Pop();
                T nextState = stateStack.Peek();

                stateMachine.ForceChangeState(nextState);
            }
            else
            {
                ConsoleDebugger.LogError("Stack not enough to pop state");
            }
        }
        #endregion Stack Function

        #region State Machine Functions
        public void AddStateRules(T state, HashSet<T> allowedStates)
        {
            stateMachine.AddStateRules(state, allowedStates);
        }

        public void RegisterOnEnter(T state, Action func)
        {
            stateMachine.RegisterOnEnter(state, func);
        }

        public void RegisterOnExit(T state, Action func)
        {
            stateMachine.RegisterOnExit(state, func);
        }

        public void Register(T state, Action onEnter, Action onExit)
        {
            stateMachine.Register(state, onEnter, onExit);
        }
        #endregion State Machine Functions
    }
}