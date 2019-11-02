using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public float fuel;
    public float movementSpeed=10f;
    public float rotationSpeed ;
    public Rigidbody2D rb;
    public GameObject trailGO;
    public float rotationZ;
    Vector2 movement;
    public bool startRecording;
    public bool goToRecording;
    
    List<PositionRecords> allPositionRecords;
    // Start is called before the first frame update
    void Start()
    {
        allPositionRecords = new List<PositionRecords>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if (startRecording) RecordMovements();
        if (goToRecording) GoToRecordMovements();


    }

    public void StartGoingToRecords()
    {
        goToRecording = true;
    }
    private void FixedUpdate()
    {
        Move();
    }

    void RecordMovements()
    {
        PositionRecords rec= new PositionRecords(transform.position, transform.eulerAngles);
        allPositionRecords.Add(rec);
    }

    

    public void GoToRecordMovements()
    {
        if(allPositionRecords.Count>0)
        {

            startRecording = false;
            PositionRecords rec = allPositionRecords[0];
            transform.position = rec.position;
            transform.eulerAngles = rec.rotation;
            allPositionRecords.RemoveAt(0);
        }
        
    }

    void Move()
    {
        
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(hInput, vInput);
        movement.x = hInput;
        movement.y = vInput;
        
      // if (input.magnitude > 0) Instantiate(trailGO, transform.position, Quaternion.identity);
       Vector3 moveDirection= transform.up * 1 * movementSpeed * Time.deltaTime;
        Vector2 rbMove = new Vector2(moveDirection.x, moveDirection.y);
        
         rotationZ -= hInput * rotationSpeed;
         transform.eulerAngles = new Vector3(0.0f, 0.0f, rotationZ);
        rb.MovePosition(rb.position+ rbMove * movementSpeed * Time.deltaTime);
        
    }
}


public class PositionRecords
    {
    
        public Vector3 position;
         public Vector3 rotation;


        public PositionRecords(Vector3 pos,Vector3 rot)
        {
            position = pos;
            rotation = rot;
        }
    }
