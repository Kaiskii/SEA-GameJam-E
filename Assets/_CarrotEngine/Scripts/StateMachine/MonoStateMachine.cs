using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarrotEngine
{
    public abstract class MonoStateMachine<T> : MonoBehaviour where T : Enum
    {
        public EnumStateMachine<T> stateMachine;

        protected virtual void Awake()
        {
            stateMachine = new EnumStateMachine<T>();
        }

        #region Inspector Debugger
        [Header("Debug Change State")]
#pragma warning disable 0649
        [SerializeField] T targetState;
#pragma warning restore 0649

        [Button]
        void ForceChangeState()
        {
            stateMachine.ForceChangeState(this.targetState);
        }

        [Button]
        void LogCurrentState()
        {
            ConsoleDebugger.LogFormat("Current State: {0}", this, stateMachine.currentState);
        }
        #endregion Inspector Debugger
    }

}
