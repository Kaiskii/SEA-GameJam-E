using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CarrotEngine;

public class ArkCollider : MonoBehaviour
{
    public int attackNum;
    public PolygonCollider2D col;
    public Animator anim;
    public PlayerController parentPlayer;
    public bool isActivated;
    public bool isUsedAlready = false;
    private List<PlayerController> shipsInTrigger = new List<PlayerController>();
    public SpriteRenderer HitArea;

    AudioManager am;

    private void Start()
    {
        if (am == null) { am = Toolbox.Instance.FindManager<AudioManager>(); }
    }

    void SetColor()
    {
        switch (parentPlayer.playerNumber)
        {
            case PlayerNumber.NUMBER1:
                HitArea.color = new Color(0.1792453f, 0.5077058f, 0.7169812f);
                break;
            case PlayerNumber.NUMBER2:
                HitArea.color = new Color(0.6320754f, 0.1818707f, 0.1818707f);
                break;

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isUsedAlready) return;
        if(isActivated)
        {
            if (!parentPlayer) return;
            if (parentPlayer.playerNumber == PlayerNumber.NUMBER1)
            {
                if (other.gameObject.tag == "Player2")
                {
                    shipsInTrigger.Add(other.GetComponent<PlayerController>());
                    
                    
                   
                }
               
                
            }
            else if (parentPlayer.playerNumber == PlayerNumber.NUMBER2)
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
            if (parentPlayer.playerNumber == PlayerNumber.NUMBER1)
            {
                if (other.gameObject.tag == "Player2")
                {

                    shipsInTrigger.Remove(other.GetComponent<PlayerController>());
                }
            }
            else if (parentPlayer.playerNumber == PlayerNumber.NUMBER2)
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
        if (isUsedAlready) return;
        //am.PlayAudioClip("execFire", AudioManager.ClipType.SFX);
        // parentPlayer.GetComponent<PlayerController>().RespawnLaser();
        
        
        foreach (PlayerController obj in shipsInTrigger)
        {
            obj.GetDamage(105);
            
        }
        isUsedAlready = true;
    }

    public void DoFade()
    {
        SetColor();
        anim.SetBool("PlayFade", true);
    }
}
