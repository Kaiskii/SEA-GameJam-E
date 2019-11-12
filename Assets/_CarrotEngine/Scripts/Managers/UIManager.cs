using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using CarrotEngine;

namespace CarrotEngine
{
    public class UIManager : SerializedMonoBehaviour, IManager
    {
        [SerializeField] private List<GameObject> UIPrefabList = new List<GameObject>();

        Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();
        Dictionary<string, GameObject> instantiatedPrefab = new Dictionary<string, GameObject>();

        public void InitializeManager()
        {
            for (int i = 0; i < UIPrefabList.Count; ++i)
            {
                GameObject prefab = UIPrefabList[i];
                prefabDictionary.Add(prefab.name, prefab);
            }
        }

        public GameObject GetPanel(string prefabName)
        {
            if (instantiatedPrefab.ContainsKey(prefabName) && instantiatedPrefab[prefabName] != null)
            {
                return instantiatedPrefab[prefabName];
            }
            else if (prefabDictionary.ContainsKey(prefabName))
            {
                GameObject instantiatedObject = Instantiate(prefabDictionary[prefabName]);
                instantiatedPrefab.Add(prefabName, instantiatedObject);
                return instantiatedObject;
            }
            else
            {
                return null;
            }
        }

        public T GetPanel<T>(string prefabName) where T : MonoBehaviour
        {
            GameObject panel = GetPanel(prefabName);
            if (panel != null)
            {
                return panel.GetComponent<T>();
            }
            else
            {
                return null;
            }
        }
    }
}