using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkController : MonoBehaviour
{
    
    public GameObject player;
    public Vector3 offSet;
    public GameObject childArkCollider;
    public List<GameObject> ArkCollidersCreatedByMe;
    public SpriteRenderer[] arrowList;

    private void Start()
    {
        for(int i=0;i<arrowList.Length;i++)
        {
            switch(player.GetComponent<PlayerController>().playerNumber)
            {
                case PlayerNumber.NUMBER1:
                    arrowList[i].color =  new Color(0.1792453f, 0.5077058f, 0.7169812f);
                    break;
                case PlayerNumber.NUMBER2:
                    arrowList[i].color = new Color(0.6320754f, 0.1818707f, 0.1818707f);
                    break;

            }
            
        }
    }
    private void Awake()
    {
        childArkCollider = transform.Find("HitArea").gameObject;
        ArkCollidersCreatedByMe = new List<GameObject>();
    }

   public void ActivateAttackArea(bool isFake, int attacknum)
    {
        Debug.Log("Activated");
        GameObject newHitArea=Instantiate(childArkCollider, childArkCollider.transform.position, childArkCollider.transform.rotation);

        ArkCollider col = newHitArea.GetComponent<ArkCollider>();
        col.parentPlayer = player.transform;
        col.DoFade();
        col.isActivated = !isFake;
        col.attackNum = attacknum;
        ArkCollidersCreatedByMe.Add(newHitArea);
        

    }


    public GameObject GetCorrectArkCollider(int attackNum)
    {
        foreach (GameObject obj in ArkCollidersCreatedByMe)
        {
            
            if (obj.GetComponent<ArkCollider>().attackNum == attackNum)
            {
                
                return obj;
               
            }

        }
        return null;
    }

    
}
