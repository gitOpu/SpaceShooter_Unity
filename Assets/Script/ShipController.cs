using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Shoot & Turret History")]
    public GameObject startWeapon;
    public List<GameObject> tripleShootTurrets;
    public List<GameObject> wideShootTurrets;
    public List<GameObject> scatterShootTurrets;
    public float scatterShooTurretReloadTime = 2.0f;
    public int weaponUpgradeStatus = 0;

    public GameObject playerBullet; 
    public List<GameObject> activePlayerTurrets;
    private AudioSource shootSoundFx;

    [Header("Player Live Satus")]
    public GameObject explosion;
    public ParticleSystem playerThrust;
    private CircleCollider2D playerCollider;
    private Renderer playerRenderer;
    private Rigidbody2D playerRigidbody;

    [Header("Basic Movement")]
    public float moveSpeed = 20f;
    private GameObject topBoundary;
    private GameObject bottomBoundary;
    private GameObject leftBoundary;
    private GameObject rightBoundary;

    
    



    void Start()
    {
        
        topBoundary = GameController.shared.topBoundary;
        bottomBoundary = GameController.shared.bottomBoundary;
        leftBoundary = GameController.shared.leftBoundary;
        rightBoundary = GameController.shared.rightBoundary;

        shootSoundFx = GetComponent<AudioSource>();
        playerRenderer = GetComponent<Renderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CircleCollider2D>();

        activePlayerTurrets = new List<GameObject>();
        activePlayerTurrets.Add(startWeapon);
        
    }
    void Update()
    {
        PlayerMovement();
        if (Input.GetKeyDown(KeyCode.Space))
       // if (Input.GetKey(KeyCode.Space) || Input.GetButton("Fire1"))
        { 
            Shoot(); 
        }
    }
    void Shoot()
    {
        foreach(GameObject turret in activePlayerTurrets)
        {
            Instantiate(playerBullet, turret.transform.position, turret.transform.rotation);
        }
        shootSoundFx.Play();
    }
   
    void PlayerMovement()
    {
        // Approch 2: Player Horizontal & Vertical Movement
        playerRigidbody.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
        playerRigidbody.position = new Vector2(
           Mathf.Clamp(playerRigidbody.transform.position.x, leftBoundary.transform.position.x, rightBoundary.transform.position.x),
           Mathf.Clamp(playerRigidbody.transform.position.y, bottomBoundary.transform.position.y, topBoundary.transform.position.y)
            );
// Approch 1: Player Horizontal & Vertical Movement
        // transform.position = transform.position + new Vector3(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime, 0);
    }
    void OnTriggerEnter2D(Collider2D other)
    
        {
        if (other.gameObject.CompareTag("Powerup"))
        {
            PowerUpScript powerUpScript = other.gameObject.GetComponent<PowerUpScript>();
            powerUpScript.PowerCollected();
            UpgradeWeapon();
        }
        else if(other.gameObject.CompareTag("Enemy Type 1") || other.gameObject.CompareTag("Enemy Laser"))
        {
            GameController.shared.ShowGameOver();
            uint numberOfLife = GameController.shared.numberOfLife;
            if (numberOfLife <= 0)
            {
                playerRenderer.enabled = false;
                playerCollider.enabled = false;
                playerThrust.Stop();
                
                Destroy(gameObject);        
            }
            Instantiate(explosion, transform.position, transform.rotation);
            for (int i = 0; i < 8; i++)
            {
                Vector3 randomOffset = new Vector3(transform.position.x + Random.Range(-0.6f, 0.6f), transform.position.y + Random.Range(-0.6f, 0.6f), 0.0f);
                Instantiate(explosion, randomOffset, transform.rotation);
            }
        }
    }
    void GameOver()
    {

        
    }
    void UpgradeWeapon()
    {
        if(weaponUpgradeStatus == 0)
        {
            foreach(GameObject turret in tripleShootTurrets)
            {
                activePlayerTurrets.Add(turret);
            }
        }
        else if (weaponUpgradeStatus == 1)
        {
            foreach (GameObject turret in wideShootTurrets)
            {
                activePlayerTurrets.Add(turret);
            }
        }
        else if (weaponUpgradeStatus == 2)
        {
            StartCoroutine(ActiveScatterShootTurret());
        }else
        {
            return;
        }
        weaponUpgradeStatus++;
    }
    IEnumerator ActiveScatterShootTurret()
    {
        while (true)
        {
            foreach(GameObject turret in scatterShootTurrets)
            {
                Instantiate(playerBullet, turret.transform.position, turret.transform.rotation);
            }
            shootSoundFx.Play();
            yield return new WaitForSeconds(scatterShooTurretReloadTime);
        }
    }
}
