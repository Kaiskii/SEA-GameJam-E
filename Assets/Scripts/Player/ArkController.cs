using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkController : MonoBehaviour
{
    public GameObject player;
    public Vector3 offSet;
    public float rotationSpeed ;
    public float rotationZ;
    private float rotationZfromArkInput;
    void Update()
    {
        MoveToPlayerPosition();
        RotateArk();
    }
    void MoveToPlayerPosition()
    {
        transform.position = player.transform.position + offSet;
    }

    void RotateArk()
    {
        //Player move left or right it rotate the ark
        float hInput = Input.GetAxisRaw("Horizontal");
        rotationZ -= hInput * rotationSpeed;
        



        //
        float arkInput = Input.GetAxisRaw("ArkRotationInput");
        rotationZfromArkInput -= arkInput * rotationSpeed;

        

        transform.eulerAngles = new Vector3(0.0f, 0.0f, rotationZ);
    }
}
