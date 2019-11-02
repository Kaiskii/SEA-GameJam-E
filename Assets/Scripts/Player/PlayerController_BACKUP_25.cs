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
    public ShipScriptableObject shipScriptableObject;
    private ShipData shipData;

    public PlayerNumber playerNumber;
    //public ShipNumber thisPlayerNumber;
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

    void Awake()
    {
        shipData = new ShipData(shipScriptableObject.shipData);
    }

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
        GameObject xx = new GameObject("xx");
        xx.transform.rotation = this.transform.rotation;
        isChosenShip = true;
        startRecording = true;
        //Dummy
        
        GameObject dumm = new GameObject("DummyPlayer");
        dumm.transform.position = transform.position;
        Rigidbody2D dummRb = dumm.AddComponent<Rigidbody2D>();
        dummRb.gravityScale = 0;
<<<<<<< HEAD
        dumm.transform.rotation = this.transform.rotation;
       dummyPlayer = dumm;
        GameObject newArk = Instantiate(Ark, dumm.transform.position, dumm.transform.rotation);
        
        
        
        
=======
        dummyPlayer = dumm;

        //Assigning ColorOverLifeTimeModule
        ParticleSystem.ColorOverLifetimeModule cltm = trail.colorOverLifetime;

        //Changing to Red if PlayerNumber1
        if (playerNumber == PlayerNumber.NUMBER1) {
            //Gradient Initialization
            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color(0.78f, 0.18f, 0.2f), 0.0f),
                                              new GradientColorKey(new Color(0.5f, 0.5f, 0.5f), 1.0f) },
                                                    new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f),
                                                new GradientAlphaKey(1.0f, 1.0f) });

            cltm.color = grad;
        }

>>>>>>> 220bf8e6ce84a227a2bae25296e722e06d276bb8

        //trail
        //trail.Play();
        //trail.loop = true;
        // trail.transform.SetParent(dumm.transform);
        //trail.transform.localPosition = Vector3.zero;

        //trail
        
        newArk.GetComponent<ArkController>().player = this.gameObject;
        newArk.gameObject.SetActive(true);
        newArk.transform.SetParent(dumm.transform);
        newArk.transform.localPosition = Vector3.zero;

        

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
        switch(playerNumber)
        {
            case PlayerNumber.NUMBER1:
                {
                    rotationZ -= Input.GetAxisRaw("Player1Horizontal") * shipData.turnSpeed;
                    dummyPlayer.transform.eulerAngles = new Vector3(0, 0,transform.eulerAngles.z+ rotationZ);

                    break;
                }
            case PlayerNumber.NUMBER2:
                {
                    rotationZ -= Input.GetAxisRaw("Player2Horizontal") * shipData.turnSpeed;
                    dummyPlayer.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z+rotationZ);

                    break;
                }
        }
        
        
    }

    void MoveDummy()
    {
        
        switch(playerNumber)
        {
            case PlayerNumber.NUMBER1:
                {
                    float hInput = Input.GetAxisRaw("Player1Horizontal");
                    float vInput = Input.GetAxisRaw("Player1Vertical");
                    Vector2 input = new Vector2(hInput, vInput);
                    movement.x = hInput;
                    movement.y = vInput;

                    // if (input.magnitude > 0) Instantiate(trailGO, transform.position, Quaternion.identity);
                    Vector3 moveDirection = dummyPlayer.transform.TransformDirection(Vector3.up) * 1 * shipData.normalSpeed * Time.fixedDeltaTime;
                    Vector2 rbMove = new Vector2(moveDirection.x, moveDirection.y);



                    dummyPlayer.GetComponent<Rigidbody2D>().MovePosition(dummyPlayer.GetComponent<Rigidbody2D>().position + rbMove * shipData.normalSpeed * Time.fixedDeltaTime);
                    break;
                }
            case PlayerNumber.NUMBER2:
                {
                    float hInput = Input.GetAxisRaw("Player2Horizontal");
                    float vInput = Input.GetAxisRaw("Player2Vertical");
                    Vector2 input = new Vector2(hInput, vInput);
                    movement.x = hInput;
                    movement.y = vInput;

                    // if (input.magnitude > 0) Instantiate(trailGO, transform.position, Quaternion.identity);
                    Vector3 moveDirection = dummyPlayer.transform.TransformDirection(Vector3.up) * shipData.normalSpeed * Time.fixedDeltaTime;
                    Vector2 rbMove = new Vector2(moveDirection.x, moveDirection.y);



                    dummyPlayer.GetComponent<Rigidbody2D>().MovePosition(dummyPlayer.GetComponent<Rigidbody2D>().position + rbMove * shipData.normalSpeed * Time.fixedDeltaTime);
                    break;
                }
        }
        
        
        
        
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
