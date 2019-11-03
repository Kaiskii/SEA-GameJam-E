using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CarrotEngine;

public class UICountdown : MonoBehaviour
{
    //Public
    public Color darkRed;
    public Color dirtyWhite;

    //Private
    [SerializeField] int turnRed = 3;

    TurnManager tm;
    Text timerText;

    void Start() {
        timerText = GetComponent<Text>();

        if (tm == null)
            tm = Toolbox.Instance.FindManager<TurnManager>();
    }

    // Update is called once per frame
    void Update() {
        switch (tm.currentState) {
            case TurnState.Planning:
                timerText.text = tm.currentCountdown.ToString("0.0");
                Debug.Log(timerText.text);
                if (tm.currentCountdown <= turnRed) {
                    timerText.color = darkRed;
                } else {
                    timerText.color = dirtyWhite;
                }
                break;

            case TurnState.Execution:
                timerText.text = (tm.planningPhaseTime - tm.currentCountdown).ToString("0.0");
                timerText.color = darkRed;
                break;

            default:
                timerText.text = "";
                break;
        }
    }
}
