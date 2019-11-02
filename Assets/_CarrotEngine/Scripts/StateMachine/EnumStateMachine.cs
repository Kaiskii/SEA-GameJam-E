using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace CarrotEngine
{
    public class EnumStateMachine<T> where T : Enum
    {
        public delegate void OnChangeStateDelegate(T prevState, T nextState);
        public event OnChangeStateDelegate OnChangeStateEvent;

        Dictionary<T, Action> OnEnterStateDict = new Dictionary<T, Action>();
        Dictionary<T, Action> OnExitStateDict= new Dictionary<T, Action>();

        Dictionary<T, HashSet<T>> StateRulesDict = new Dictionary<T, HashSet<T>>(); 

        public T currentState { get; protected set; }

        public EnumStateMachine()
        {
            // Initialize Action dictionary
            foreach (T type in Enum.GetValues(typeof(T)))
            {
                OnEnterStateDict.Add(type, () => { });
                OnExitStateDict.Add(type, () => { });
            }
        }

        public EnumStateMachine(T defaultState) : this()
        {
            currentState = defaultState;
        }

        public virtual bool SafeChangeState(T nextState)
        {
            if(StateRulesDict.ContainsKey(currentState))
            {
                if(StateRulesDict[currentState].Contains(nextState))
                {
                    ChangeState(nextState);
                    return true;
                }
                else
                {
                    ConsoleDebugger.LogErrorFormat("Cannot Change to State from {0} to {1}", this, currentState, nextState);
                    return false;
                }
            }
            else
            {
                ConsoleDebugger.LogWarningFormat("{0} state has no rule, assume safe to change state", this, currentState);
                ChangeState(nextState);
                return true;
            }

        }

        public virtual void ForceChangeState(T nextState)
        {
            ChangeState(nextState);
        }

        void ChangeState(T nextState)
        {
            T prevState = currentState;
            currentState = nextState;

            OnExitStateDict[currentState]();
            OnChangeStateEvent?.Invoke(prevState, nextState);
            OnEnterStateDict[currentState]();
        }

        public void AddStateRules(T state, HashSet<T> allowedStates)
        {
            if(!StateRulesDict.ContainsKey(state))
            {
                StateRulesDict.Add(state, allowedStates);
            }
            else
            {
                ConsoleDebugger.LogErrorFormat("{0} state rules already defined!", this, state);
            }
        }

        #region Event Registration
        public void RegisterOnEnter(T state, Action func)
        {
            OnEnterStateDict[state] += func;
        }

        public void RegisterOnExit(T state, Action func)
        {
            OnExitStateDict[state] += func;
        }

        public void Register(T state, Action onEnter, Action onExit)
        {
            RegisterOnEnter(state, onEnter);
            RegisterOnExit(state, onExit);
        }
        #endregion
    }
}