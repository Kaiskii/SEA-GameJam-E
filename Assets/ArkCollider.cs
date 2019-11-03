using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkCollider : MonoBehaviour
{
    public PolygonCollider2D col;
    public Animator anim;
    public Transform parentPlayer;
    public bool isActivated;
    public bool isUsedAlready = false;

    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isUsedAlready) return;
        if(isActivated)
        {
            if (parentPlayer.GetComponent<PlayerController>().playerNumber == PlayerNumber.NUMBER1)
            {
                if (other.gameObject.tag == "Player2")
                {
                    other.GetComponent<PlayerController>().GetDamage(100);
                    isUsedAlready = true;
                }
               
                
            }
            else if (parentPlayer.GetComponent<PlayerController>().playerNumber == PlayerNumber.NUMBER2)
            {
                if (other.gameObject.tag == "Player1")
                {
                    other.GetComponent<PlayerController>().GetDamage(100);
                    isUsedAlready = true;
                }
                
            }
        }
       
    }

    public void DoFade()
    {
        anim.SetBool("PlayFade", true);
    }
}
