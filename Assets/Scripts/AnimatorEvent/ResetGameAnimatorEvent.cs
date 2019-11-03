using CarrotEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGameAnimatorEvent : MonoBehaviour
{

    private GameManager gameManager { get { return Toolbox.Instance.FindManager<GameManager>(); } }

    public void OnResetGameEnd()
    {
        gameManager.ResetGame();
        this.gameObject.SetActive(false);
    }
}
