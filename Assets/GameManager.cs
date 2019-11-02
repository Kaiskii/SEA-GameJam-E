using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerNumber
{
    NUMBER1,NUMBER2,
}


public class GameManager : MonoBehaviour
{
    static GameManager instance;
    
    public List<GameObject> player1Ships; //Ship Size
    public List<GameObject> player2Ships;

    public GameObject templateTrail;

    private void Awake()
    {
        instance = this;
    }

    private void Start() {
        player1Ships[0].GetComponent<PlayerController>().MakeThisChosen();

        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color(0.78f, 0.18f, 0.2f), 0.0f),
                                              new GradientColorKey(new Color(0.5f, 0.5f, 0.5f), 1.0f) }, 
                                                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), 
                                                new GradientAlphaKey(1.0f, 1.0f) });

        for(int i = 0; i < player1Ships.Count; ++i) {
            var tgo = Instantiate(templateTrail, player1Ships[i].transform).GetComponent<ParticleSystem>().colorOverLifetime;
            tgo.color = grad;
            Instantiate(templateTrail, player2Ships[i].transform);
        }
    }
}
