using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CarrotEngine;

public class PlayerController : MonoBehaviour
{
    public ShipScriptableObject shipScriptableObject;
    private ShipData shipData;
    private int attackNum=0;
    private int arkAttackNum = 0;
    public PlayerNumber playerNumber;
    public GameObject laserPrefab;
    public SpriteRenderer spriteRend;
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
    public GameObject dummyPlayer;
    private ParticleSystem ownTrail;
    public float health;
    private float fuel;
    private float ammo;
    private float fakeAmmo;

    [SerializeField] GameManager gameManager { get { return Toolbox.Instance.FindManager<GameManager>(); } }
    [SerializeField] TurnManager _turnManager;

    bool inputFakeShot = false;
    bool inputRealShot = false;

    AudioManager am;

    TurnManager turnManager
    {
        get
        {
            return (_turnManager) ?? (_turnManager = Toolbox.Instance.FindManager<TurnManager>());
        }
        set
        {
            _turnManager = value;
        }
    }

    List<PositionRecords> allPositionRecords;

    void Awake()
    {
        shipData = new ShipData(shipScriptableObject.shipData);
        health = shipData.maxHp;
        fuel = shipData.maxFuel;
        ammo = shipData.maxAmmo;
        fakeAmmo = shipData.maxAmmo;
       


    }

    public void InitializeController()
    {

    }

    void Start()
    {
        if (am == null)
            am = Toolbox.Instance.FindManager<AudioManager>();

        allPositionRecords = new List<PositionRecords>();
        Ark.gameObject.SetActive(false);
        
        
        InitializeController();
       // SetCorrectHPLayout();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!isChosenShip && !goToRecording) return;
        else if (goToRecording && turnManager.currentState==TurnState.Execution) GoToRecordMovements();
        
        if (startRecording) RecordMovements();


        if (!goToRecording) RotateDummy();
        CheckForShoot();

    }
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(0.4f);
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
                else
                {
                    inputFakeShot = false;
                    inputRealShot = false;
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
                else
                {
                    inputFakeShot = false;
                    inputRealShot = false;
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
        dumm.transform.rotation = Quaternion.Euler(transform.eulerAngles);
        dummyPlayer = dumm;
        GameObject newArk = Instantiate(Ark, dumm.transform.position, dumm.transform.rotation);
        
        ownArk = newArk;
        dummyPlayer = dumm;

        gameManager.AddDummyPlayer(dummyPlayer);
        //Assigning ColorOverLifeTimeModule

        //Changing to Red if PlayerNumber1

        //trail
        ParticleSystem newTrail = Instantiate(trail, newArk.transform.position, transform.rotation);
        ParticleSystem.ColorOverLifetimeModule cltm = newTrail.colorOverLifetime;
        

        if (playerNumber == PlayerNumber.NUMBER1)
        {
            //Gradient Initialization
            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color(0.17f, 0.68f, 0.78f), 0.0f),
                                              new GradientColorKey(new Color(0.17f, 0.68f, 0.78f), 1.0f) },
                                                    new GradientAlphaKey[] { new GradientAlphaKey(0.8f, 0.0f),
                                                    new GradientAlphaKey(0.8f, 0.9f),
                                                new GradientAlphaKey(0.0f, 1.0f) });

            cltm.color = grad;
        }
        newTrail.Play();
        newTrail.loop = true;
        newTrail.transform.SetParent(dumm.transform);
        newTrail.transform.localPosition = Vector3.zero;
        ownTrail = newTrail;
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
        PositionRecords rec= new PositionRecords(dummyPlayer.transform.position, dummyPlayer.transform.eulerAngles, inputRealShot, inputFakeShot);
        allPositionRecords.Add(rec);
    }    

    public void GoToRecordMovements()
    {
        if (Time.timeScale == 0) return;
        if(allPositionRecords.Count>0)
        {
            
            startRecording = false;
            PositionRecords rec = allPositionRecords[0];
            transform.position = rec.position;
            transform.eulerAngles = rec.rotation;
            if (allPositionRecords[0].isShot == true)
            {
                Debug.Log("RealShot");
                attackNum++;
                if (ownArk.GetComponent<ArkController>().GetCorrectArkCollider(attackNum)) ownArk.GetComponent<ArkController>().GetCorrectArkCollider(attackNum).GetComponent<ArkCollider>().DoDamageToList();


            }
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
        am.PlayAudioClip("plannedFire", AudioManager.ClipType.SFX);
        arkAttackNum++;
        ownArk.GetComponent<ArkController>().ActivateAttackArea(false, arkAttackNum);
        canShoot = false;
        ammo--;
        inputRealShot = true;
        StartCoroutine(ShootDelay());
        
    }

    void FakeShoot()
    {
        Debug.Log("FakeShoot");
        am.PlayAudioClip("plannedFire", AudioManager.ClipType.SFX);
        arkAttackNum++;
        ownArk.GetComponent<ArkController>().ActivateAttackArea(true, arkAttackNum);
        canShoot = false;
        fakeAmmo--;
        inputFakeShot = true;
        StartCoroutine(ShootDelay());
    }

    public void GetDamage(float damage)
    {
        Debug.Log("GotDamaged");
         health -= damage;
        if (health <= 0)
        {
            this.gameObject.SetActive(false);
            switch (playerNumber)
            {
                case PlayerNumber.NUMBER1:
                    {
                        gameManager.RemovePlayerShip(this, 1);
                        Instantiate(gameManager.explostionPrefab, this.transform.position, Quaternion.identity);
                        //am.PlayAudioClip("shipExplode", AudioManager.ClipType.SFX);
                        Destroy(this);
                        break;
                    }


                case PlayerNumber.NUMBER2:
                    {
                        gameManager.RemovePlayerShip(this, 2);
                        Instantiate(gameManager.explostionPrefab, this.transform.position, Quaternion.identity);
                        //am.PlayAudioClip("shipExplode", AudioManager.ClipType.SFX);
                        Destroy(this);
                        break;
                    }
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

    void SetCorrectHPLayout()
    {
        if(health>=50)
        {
            spriteRend.color = Color.blue;
        }
       else if(health<50)
        {
            spriteRend.color = Color.white;
        }
    }

    public void RespawnLaser()
    {
        Instantiate(laserPrefab, transform.position, transform.rotation);
    }

    public void EndTurn()
    {
        ownArk.SetActive(false);
        startRecording = false;
        isChosenShip = false;
        goToRecording = false;
       //attackNum = 0;
       // arkAttackNum = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            gameManager.RemovePlayerShip(this, (int) this.playerNumber);
            PlayerController collidedPlayer = other.GetComponent<PlayerController>();
            gameManager.RemovePlayerShip(collidedPlayer, (int)collidedPlayer.playerNumber);

                       
            Instantiate(gameManager.explostionPrefab, transform.position, Quaternion.identity);
            Instantiate(gameManager.explostionPrefab, other.transform.position, Quaternion.identity);
            //am.PlayAudioClip("shipExplode", AudioManager.ClipType.SFX);
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }
        else if(other.gameObject.tag=="Wall")
        {
                        
            gameManager.RemovePlayerShip(this, (int)this.playerNumber);
            Instantiate(gameManager.explostionPrefab, transform.position, Quaternion.identity);
            //am.PlayAudioClip("shipExplode", AudioManager.ClipType.SFX);
            Destroy(this.gameObject);
        }
    }
}

public struct PositionRecords
{
    public Vector3 position;
    public Vector3 rotation;
    public bool isShot;
    public bool fakeShot;

    public PositionRecords(Vector3 pos,Vector3 rot, bool isShot, bool fakeShot)
    {
        position = pos;
        rotation = rot;
        this.isShot = isShot;
        this.fakeShot = fakeShot;
    }
}
