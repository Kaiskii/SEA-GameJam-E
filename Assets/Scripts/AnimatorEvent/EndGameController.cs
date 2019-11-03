using CarrotEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{
    GameManager gameManager { get { return Toolbox.Instance.FindManager<GameManager>(); } }

    [SerializeField] Image winImage;
    [SerializeField] Text player1Text;
    [SerializeField] Text player2Text;

    [SerializeField] Sprite player1Sprite;
    [SerializeField] Sprite player2Sprite;
    [SerializeField] Sprite tieSprite;

    public void setPlayerWin(int playerNum)
    {
        switch (playerNum)
        {
            case 0:
                winImage.sprite = tieSprite;
                player1Text.text = "Tie";
                player2Text.text = "Tie";
                break;

            case 1:
                winImage.sprite = player1Sprite;
                player1Text.text = "Win";
                player2Text.text = "Lose";
                break;

            case 2:
                winImage.sprite = player2Sprite;
                player1Text.text = "Lose";
                player2Text.text = "Win";
                break;
        }

        this.gameObject.SetActive(true);
    }

    public void ResetGame()
    {
        gameManager.ResetGame();
    }

    [Button]
    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
