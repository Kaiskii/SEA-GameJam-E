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

        player1Ships = new GameObject[3];
        player2Ships = new GameObject[3];
    }

   
}
