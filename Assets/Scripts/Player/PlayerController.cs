using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CarrotEngine;
/*public enum ShipNumber
{
    NUMBER1, NUMBER2, NUMBER3, NUMBER4
}*/
public class PlayerController : MonoBehaviour
{
    public ShipScriptableObject shipScriptableObject;
    private ShipData shipData;

    public PlayerNumber playerNumber;
    
    public Rigidbody2D rb;
    public float rotationZ;
    Vector2 movement;
    public bool startRecording;
    public bool goToRecording;
    public bool isChosenShip;
    private bool canShoot = true;
    public GameObject Ark;
    private GameObject ownArk;
    public ParticleSystem trail;
    private GameObject dummyPlayer;
    private float health;
    private float fuel;
    private float ammo;
    private float fakeAmmo;
    [SerializeField] TurnManager turnManager;

    List<PositionRecords> allPositionRecords;

    void Awake()
    {
        shipData = new ShipData(shipScriptableObject.shipData);
        health = shipData.maxHp;
        fuel = shipData.maxFuel;
        ammo = shipData.maxAmmo;
        fakeAmmo = shipData.maxAmmo;


    }

    public void InitializeManager()
    {
        if (turnManager == null)
        {
            turnManager = Toolbox.Instance.FindManager<TurnManager>();
        }
    }

    void Start()
    {
        allPositionRecords = new List<PositionRecords>();
        Ark.gameObject.SetActive(false);
        trail = transform.Find("templateTrail").GetComponent<ParticleSystem>();
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!isChosenShip) return;
        if (startRecording) RecordMovements();
        if (goToRecording && turnManager.currentState==TurnState.Execution) GoToRecordMovements();
        

        
       if(!goToRecording) RotateDummy();
        CheckForShoot();

    }
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(0.2f);
        canShoot = true;
    }
    void CheckForShoot()
    {
        switch(playerNumber)
        {
            case PlayerNumber.NUMBER1:
                if(Input.GetKeyDown(KeyCode.W))
                {
                    if(canShoot && !goToRecording && ammo>0) Shoot();

                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    if (canShoot&& !goToRecording&& fakeAmmo > 0) FakeShoot();

                }
                break;
            case PlayerNumber.NUMBER2:
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (canShoot && !goToRecording && ammo > 0) Shoot();

                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (canShoot && !goToRecording && fakeAmmo>0) FakeShoot();

                }
                break;
        }
    }

    public void MakeThisChosen()
    {
        
        isChosenShip = true;
        startRecording = true;
        //Dummy
        
        GameObject dumm = new GameObject("DummyPlayer");
        dumm.transform.position = transform.position;
        Rigidbody2D dummRb = dumm.AddComponent<Rigidbody2D>();
        dummRb.gravityScale = 0;
        dumm.transform.rotation = this.transform.rotation;
        dummyPlayer = dumm;
        GameObject newArk = Instantiate(Ark, dumm.transform.position, dumm.transform.rotation);
        ownArk = newArk;
        
        
        

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

           // cltm.color = grad;
        }



        //trail
        trail.Play();
        trail.loop = true;
         trail.transform.SetParent(dumm.transform);
        trail.transform.localPosition = Vector3.zero;

      
        
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
    public void Destroy()
    {
        
    }

     void Shoot()
    {
        Debug.Log("Shoot");
        ownArk.GetComponent<ArkController>().ActivateAttackArea(false);
        canShoot = false;
        ammo--;
        StartCoroutine(ShootDelay());
        
    }

    void FakeShoot()
    {
        Debug.Log("FakeShoot");
        ownArk.GetComponent<ArkController>().ActivateAttackArea(true);
        canShoot = false;
        fakeAmmo--;
        StartCoroutine(ShootDelay());
    }

    public void GetDamage(float damage)
    {
        Debug.Log("GotDamaged");
         health-= damage;
        if (health <= 0) this.gameObject.SetActive(false);
       
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

    public void EndTurn()
    {
        Destroy(ownArk);
        isChosenShip = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
       switch(playerNumber)
        {
            case PlayerNumber.NUMBER1:
                {
                    if (other.gameObject.tag == "Player2") Destroy(this.gameObject);
                    break;
                }
                

            case PlayerNumber.NUMBER2:
                {
                    if (other.gameObject.tag == "Player1") Destroy(this.gameObject);
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
