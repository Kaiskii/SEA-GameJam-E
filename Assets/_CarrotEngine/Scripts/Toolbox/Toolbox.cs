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

        private List<IManager> managerList = new List<IManager>();

        /// <summary>
        ///  Calls all the necessary managers upon game start
        /// </summary>
        private void Awake()
        {
            float time = Time.realtimeSinceStartup;

            if (Instance == null) Instance = this;
            else
            {
                if (debugMode) ConsoleDebugger.Log("Destroying second Toolbox instance");
                Destroy(this);
                return;
            }

            //DontDestroyOnLoad(gameObject);

            // Adds all manager to list first
            foreach (IManager manager in gameObject.GetComponentsInChildren<IManager>(true))
            {
                managerList.Add(manager);
            }

            // Separates Init to avoid manager dependency
            for (int i = 0; i < managerList.Count; ++i)
            {
                managerList[i].InitializeManager();
                if (debugMode) ConsoleDebugger.LogFormat("Manager initialize timestamp: {0}", Time.realtimeSinceStartup - time);
            }

            if (debugMode) ConsoleDebugger.LogFormat("Toolbox initialize timestamp: {0}", Time.realtimeSinceStartup - time);
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

            ConsoleDebugger.Log("{0} adding component to ToolBox", typeof(T).ToString());

            GameObject newObject = new GameObject(typeof(T).ToString());
            newObject.transform.parent = this.transform;
            T newManager = newObject.AddComponent<T>();
            managerList.Add(newManager);
            newManager.InitializeManager();
            return newManager;
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

            ConsoleDebugger.LogFormat("{0} doesn't exist", typeof(T).ToString());
            return null;
        }
    }
}