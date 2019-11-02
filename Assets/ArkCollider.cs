using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkCollider : MonoBehaviour
{
    public PolygonCollider2D col;
    public Animator anim;
    public Transform parentPlayer;
    public bool isActivated;

    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(isActivated)
        {
            if (parentPlayer.GetComponent<PlayerController>().playerNumber == PlayerNumber.NUMBER1)
            {
                if (other.gameObject.tag == "Player2") other.GetComponent<PlayerController>().GetDamage(100);
            }
            else if (parentPlayer.GetComponent<PlayerController>().playerNumber == PlayerNumber.NUMBER2)
            {
                if (other.gameObject.tag == "Player1") other.GetComponent<PlayerController>().GetDamage(100);
            }
        }
       
    }

    public void DoFade()
    {
        anim.SetBool("PlayFade", true);
    }
}
