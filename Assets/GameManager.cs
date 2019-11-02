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
    
    public GameObject[] player1Ships; //Ship Size
    public GameObject[] player2Ships;
    private void Awake()
    {
        instance = this;

        
    }

    private void Start()
    {
        player1Ships[0].GetComponent<PlayerController>().MakeThisChosen();
        player2Ships[0].GetComponent<PlayerController>().MakeThisChosen();
    }


}
