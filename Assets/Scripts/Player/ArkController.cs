using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkController : MonoBehaviour
{
    
    public GameObject player;
    public Vector3 offSet;
    public GameObject childArkCollider;
    private void Awake()
    {
        childArkCollider = transform.Find("HitArea").gameObject;
    }

   public void ActivateAttackArea(bool isFake)
    {
        Debug.Log("Activated");
        GameObject newHitArea=Instantiate(childArkCollider, childArkCollider.transform.position, childArkCollider.transform.rotation);

        ArkCollider col = newHitArea.GetComponent<ArkCollider>();
        col.parentPlayer = player.transform;
        col.DoFade();
        col.isActivated = !isFake;
        

    }

    
}
