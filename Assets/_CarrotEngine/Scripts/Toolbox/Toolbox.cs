using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CarrotEngine
{
    [DisallowMultipleComponent]
    public class Toolbox : MonoBehaviour
    {
        public static Toolbox Instance
        {
            get; private set;
        }

        [SerializeField] private bool debugMode = false;
        [SerializeField] private bool dontDestroyOnLoad = true;

        private List<IManager> managerList = new List<IManager>();


        [Header("Merge Toolbox")]
        [SerializeField] private bool allowMergeToolbox;

        private enum ToolboxMerge
        {
            ReplaceWithNewManager,
            KeepOldManager,
        }

        [EnableIf("allowMergeToolbox"), SerializeField] private ToolboxMerge mergeMethod;

        /// <summary>
        ///  Calls all the necessary managers upon game start
        /// </summary>
        private void Awake()
        {
            float time = Time.realtimeSinceStartup;


            if (Instance == null)
            {
                Instance = this;
                if (dontDestroyOnLoad) { DontDestroyOnLoad(gameObject); }
            }
            else if (!Instance.allowMergeToolbox)
            {
                DestroyToolbox();
                return;
            }

            // Adds all manager to list first
            foreach (IManager manager in gameObject.GetComponentsInChildren<IManager>(true))
            {
                managerList.Add(manager);
            }

            // Separates Init to avoid manager dependency
            for (int i = 0; i < managerList.Count; ++i)
            {
                managerList[i].InitializeManager();
                if (debugMode) { ConsoleDebugger.LogFormat("Manager initialize timestamp: {0}", Time.realtimeSinceStartup - time); }
            }

            if (debugMode) { ConsoleDebugger.LogFormat("Toolbox initialize timestamp: {0}", Time.realtimeSinceStartup - time); }

            if (this != Instance && Instance.allowMergeToolbox) { MergeToolbox(); }
        }

        private void DestroyToolbox()
        {
            if (Instance.debugMode) { ConsoleDebugger.Log("Destroying second Toolbox instance"); }

            Destroy(this.gameObject);
            return;
        }

        /// <summary>
        /// Tries to get a manager from the toolbox.
        /// If manager does not exist, it will create the manager component,
        /// adds the component to the Toolbox gameobject (instance)
        /// and saves it in managerList.
        /// 
        /// Example Usage:
        /// InputManager manager = Toolbox.Instance.GetManager<InputManager>();
        /// </summary>
        public T GetManager<T>() where T : MonoBehaviour, IManager
        {
            T manager = FindManager<T>();
            if (manager != null) { return manager; }

            return AddManager<T>();
        }

        public T FindManager<T>() where T : MonoBehaviour, IManager
        {
            // Check if exist
            for (int i = 0; i < managerList.Count; ++i)
            {
                if (managerList[i] is T)
                {
                    return managerList[i] as T;
                }
            }

            ConsoleDebugger.LogWarningFormat("{0} doesn't exist", typeof(T).ToString());
            return null;
        }
        private T AddManager<T>() where T : MonoBehaviour, IManager
        {
            ConsoleDebugger.LogFormat("{0} adding component to ToolBox", typeof(T).ToString());

            GameObject newObject = new GameObject(typeof(T).ToString());
            newObject.transform.parent = this.transform;
            T newManager = newObject.AddComponent<T>();
            managerList.Add(newManager);
            newManager.InitializeManager();
            return newManager;
        }

        #region Merge Toolbox
        /// <summary>
        /// Instance: First Instantiated Toolbox
        /// this: Duplicate Toolbox in another scene
        /// </summary>
        private void MergeToolbox()
        {
            if (debugMode) { ConsoleDebugger.LogWarning("Merging Toolbox: " + this.GetInstanceID()); }
            for (int i = 0; i < managerList.Count; ++i)
            {
                if (Instance.mergeMethod != ToolboxMerge.KeepOldManager)
                {
                    Instance.RemoveIfManagerExist(this.managerList[i].GetType());
                }

                Instance.MoveManager(this.managerList[i]);
            }
            if (debugMode) { ConsoleDebugger.Log("Merge Toolbox Complete: " + this.GetInstanceID()); }

            DestroyToolbox();
        }

        private bool RemoveIfManagerExist(Type managerType)
        {
            for(int i = 0; i < managerList.Count; ++i)
            {
                if(managerList[i].GetType() == managerType)
                {
                    Destroy(((MonoBehaviour)managerList[i]).gameObject);
                    managerList.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Move Manager to 'this' Toolbox
        /// </summary>
        /// <param name="manager"></param>
        private void MoveManager(IManager manager)
        {
            GameObject managerObject = ((MonoBehaviour) manager).gameObject;

            managerObject.transform.parent = this.gameObject.transform;
            managerList.Add(manager);
        }
        #endregion Merge Toolbox
    }
}