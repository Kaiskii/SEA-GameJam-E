using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkController : MonoBehaviour
{
    
    public GameObject player;
    public Vector3 offSet;
    public GameObject childArkCollider;
    public List<GameObject> ArkCollidersCreatedByMe;
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
            Debug.Log("returnCorrect");
            if (obj.GetComponent<ArkCollider>().attackNum == attackNum)
            {
                
                return obj;
               
            }

        }
        return null;
    }

    
}
