using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

   public float movementSpeed=10f;
    public float rotationSpeed ;
    public Rigidbody2D rb;
    public GameObject trailGO;
    public float rotationZ;
    Vector2 movement;
    
    List<PositionRecords> allPositionRecords;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
    }
    private void FixedUpdate()
    {
        
    }

    void Move()
    {
        
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(hInput, vInput);
        movement.x = hInput;
        movement.y = vInput;
       if (input.magnitude > 0) Instantiate(trailGO, transform.position, Quaternion.identity);
       Vector3 moveDirection= transform.up * vInput * movementSpeed * Time.deltaTime;
        Vector2 rbMove = new Vector2(moveDirection.x, moveDirection.y);
        
         rotationZ -= hInput * rotationSpeed;
         transform.eulerAngles = new Vector3(0.0f, 0.0f, rotationZ);
        rb.MovePosition(rb.position+ rbMove * movementSpeed * Time.deltaTime);

    }
}


public class PositionRecords
    {
        Vector3 position;
    }
