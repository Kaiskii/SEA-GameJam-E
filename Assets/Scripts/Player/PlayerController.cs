using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*public enum ShipNumber
{
    NUMBER1, NUMBER2, NUMBER3, NUMBER4
}*/
public class PlayerController : MonoBehaviour
{
    public float fuel;
    //public ShipNumber thisPlayerNumber;
    public float movementSpeed=10f;
    public float rotationSpeed ;
    public Rigidbody2D rb;
    public float rotationZ;
    Vector2 movement;
    public bool startRecording;
    public bool goToRecording;
    public bool isChosenShip;
    public GameObject Ark;
    public ParticleSystem trail;
    private GameObject dummyPlayer;
    
    List<PositionRecords> allPositionRecords;
    // Start is called before the first frame update
    void Start()
    {
        allPositionRecords = new List<PositionRecords>();
        Ark.gameObject.SetActive(false);
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!isChosenShip) return;
        if (startRecording) RecordMovements();
        if (goToRecording) GoToRecordMovements();


        
       if(!goToRecording) RotateDummy();

    }

    

    public void MakeThisChosen()
    {
        isChosenShip = true;
        startRecording = true;
        //Dummy
        GameObject newArk=Instantiate(Ark);
        newArk.GetComponent<ArkController>().player = this.gameObject;
        GameObject dumm = new GameObject("DummyPlayer");
        dumm.transform.position = transform.position;
        dumm.transform.rotation = transform.rotation;
        Rigidbody2D dummRb=dumm.AddComponent<Rigidbody2D>();
        dummRb.gravityScale = 0;
        dummyPlayer = dumm;
        Ark.gameObject.SetActive(true);
        trail.Play();
        trail.loop = true;
        Ark.transform.SetParent(dumm.transform);
        trail.transform.SetParent(dumm.transform);
        //
        
    }

    public void StartGoingToRecords()
    {
        goToRecording = true;
        
    }
    private void FixedUpdate()
    {
        if (!isChosenShip) return;
        if (!goToRecording) MoveDummy();
    }

    void RecordMovements()
    {
        PositionRecords rec= new PositionRecords(dummyPlayer.transform.position, dummyPlayer.transform.eulerAngles);
        allPositionRecords.Add(rec);
    }

    

    public void GoToRecordMovements()
    {
        if(allPositionRecords.Count>0)
        {
            Destroy(Ark);
            startRecording = false;
            PositionRecords rec = allPositionRecords[0];
            transform.position = rec.position;
            transform.eulerAngles = rec.rotation;
            allPositionRecords.RemoveAt(0);
        }
        
    }

    void RotateDummy()
    {
        rotationZ -= Input.GetAxisRaw("Horizontal") * rotationSpeed;
        dummyPlayer.transform.eulerAngles = new Vector3(0.0f, 0.0f, rotationZ);
    }

    void MoveDummy()
    {
        
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(hInput, vInput);
        movement.x = hInput;
        movement.y = vInput;
        
      // if (input.magnitude > 0) Instantiate(trailGO, transform.position, Quaternion.identity);
       Vector3 moveDirection= dummyPlayer.transform.up * 1 * movementSpeed * Time.deltaTime;
        Vector2 rbMove = new Vector2(moveDirection.x, moveDirection.y);



        dummyPlayer.GetComponent<Rigidbody2D>().MovePosition(dummyPlayer.GetComponent<Rigidbody2D>().position+ rbMove * movementSpeed * Time.deltaTime);
        
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
