using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{

    [Button]
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
