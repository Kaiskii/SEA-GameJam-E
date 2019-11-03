using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkCollider : MonoBehaviour
{
    public int attackNum;
    public PolygonCollider2D col;
    public Animator anim;
    public Transform parentPlayer;
    public bool isActivated;
    public bool isUsedAlready = false;
    private List<PlayerController> shipsInTrigger;

    

    private void Start()
    {
        shipsInTrigger = new List<PlayerController>();
        
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
                    shipsInTrigger.Add(other.GetComponent<PlayerController>());
                    
                    
                   
                }
               
                
            }
            else if (parentPlayer.GetComponent<PlayerController>().playerNumber == PlayerNumber.NUMBER2)
            {
                if (other.gameObject.tag == "Player1")
                {
                    shipsInTrigger.Add(other.GetComponent<PlayerController>());
                    foreach (PlayerController obj in shipsInTrigger)
                    {
                        obj.GetDamage(35);
                    }
                    
                }
                
            }
        }
       
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(parentPlayer)
        {
            if (parentPlayer.GetComponent<PlayerController>().playerNumber == PlayerNumber.NUMBER1)
            {
                if (other.gameObject.tag == "Player2")
                {

                    shipsInTrigger.Remove(other.GetComponent<PlayerController>());
                }
            }
            else if (parentPlayer.GetComponent<PlayerController>().playerNumber == PlayerNumber.NUMBER2)
            {
                if (other.gameObject.tag == "Player1")
                {
                    shipsInTrigger.Remove(other.GetComponent<PlayerController>());
                }
            }
        }
        
    }

    public void DoDamageToList()
    {
        parentPlayer.GetComponent<PlayerController>().RespawnLaser();
        foreach (PlayerController obj in shipsInTrigger)
        {
            obj.GetDamage(51);
            
        }
        isUsedAlready = true;
    }

    public void DoFade()
    {
        anim.SetBool("PlayFade", true);
    }
}
