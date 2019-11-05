using CarrotEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGameController : MonoBehaviour
{
    GameManager gameManager { get { return Toolbox.Instance.FindManager<GameManager>(); } }

    public void ContinueGame()
    {
        gameManager.ContinueGame();
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        gameManager.isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
