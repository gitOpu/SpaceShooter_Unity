**Space Shooter**



### Activities
Background -> Slow moving particle system x 2, galaxy x 3. 
Rocket/Player -> move, shoot, animated tails(particle), collect power, fire, dead, weapon upgrade
Enemy-> Instantiate Randomly with random shape, Move Randomly, Shoot, dead by boundary or player bullet fire, release Powerup with random sprite
GameController-> Update Score & Player lives, GameOver, Restart Game

![](doc/cover1.gif)

**Method**
First of all we need to decide what object we need to make the game, Here GameController, MainCamera, BackgroundManager, SoundManager, GUIManager are always common for all game
# Index
### Make Background particle systems (Visual), 
### Add Ship Tail Flare (Visual)
### Making Boundary of the game (Coding)
*Create 4 GO, Attach the code, Change direction parameter*
1. Boundary at camera bounds, helps to destroy object
2. Create GO->Add Box Collider
### Game Controller to create object on game on demand
1. Its a shareable class, accessible from anywhere
2. Manage GUI say score, Live, create enemy
### ShipController
1. Ship Move Left-Right, Up-Down (In between boundary) 
2. Shoot according to Turret condition. Power catch, Weapon upgrade, Scatter Shoot.
### Enemy Drone Controller
1. Enemy destroy by boundary, player bullet
2. Instantiate enemy Shoot, PowerUp object
**Plug & Play**
### Basic Movement
1. Fall for mass of Rigidbody
### Random Movement
1. Spider hanging movement randomly
### Destroy by Boundary
1.	Attached object will destroy by boundary (Collider must Trigger)
### Particle parent gameObject will be destroy when particalSystem stop emission, for any particle system on UI


# Configuration and Procedure
---
### Make Background particle systems (Visual), 
### Add Ship Tail Flare (Visual)

### Making Boundary of the game (Coding)
*Create 4 GO, Attach the code, Change direction parameter*
1. Boundary at camera bounds, helps to destroy object
2. Create GO->Add Box Collider
*Boundary.cs*

```c#
public class Boundary : MonoBehaviour
{
public enum BoundaryLocation
    {
        LEFT, TOP, RIGHT, BOTTOM
    };
    public BoundaryLocation direction;
    private BoxCollider2D boxCollider;

    public float boundaryWidth = 0.5f;
    public float overHang = 0.5f;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, 0));
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0));
        Vector3 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 lowerRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, 0));
        
        if(direction == BoundaryLocation.TOP)
        {
            boxCollider.size = new Vector2(Mathf.Abs(topLeft.x) + Mathf.Abs(topRight.x) + overHang, boundaryWidth);
            boxCollider.offset = new Vector2(0, boundaryWidth/2);
            transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight, 1));
        }
        if (direction == BoundaryLocation.BOTTOM)
        {
            boxCollider.size = new Vector2(Mathf.Abs(lowerLeft.x) + Mathf.Abs(lowerRight.x) + overHang, boundaryWidth);
            boxCollider.offset = new Vector2(0, -boundaryWidth / 2);
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, 0, 1));
        }
        if (direction == BoundaryLocation.LEFT)
        {
            boxCollider.size = new Vector2(boundaryWidth, Mathf.Abs(lowerLeft.y) + Mathf.Abs(topLeft.y) + overHang);
            boxCollider.offset = new Vector2(-boundaryWidth / 2, 0);
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight / 2, 1));
        }
       if (direction == BoundaryLocation.RIGHT)
        {
            boxCollider.size = new Vector2(boundaryWidth, Mathf.Abs(lowerRight.y) + Mathf.Abs(topRight.y) + overHang);
            boxCollider.offset = new Vector2(boundaryWidth / 2, 0);
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight / 2, 1));
        }
    }

    // Draw Gizmos, To show gizmos, active it from game panel and select boundary object
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.up, Color.red);
        Debug.DrawRay(transform.position, Vector3.left, Color.red);
        Debug.DrawRay(transform.position, Vector3.right, Color.red);
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
    }
}

```

### Game Controller to create object on game on demand
1. Its a shareable class, accessible from anywhere
2. Manage GUI say score, Live, create enemy
*GameController.cs*
```c#
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{

    [Header("Shared Instance")]
    public static GameController shared;
    //[Header("Player Instance")]
    
    [Header("Wave of Enemy Spwan")]
    public GameObject enemyType1;
    public GameObject enemyType2;
    public int enemyPerWave = 5;
    public float startWait = 1.0f;
    public float waveInterval = 2.0f;
    public float spawnInterval = 0.5f;



    [Header("Access Boundary")]
    public GameObject topBoundary;
    public GameObject bottomBoundary;
    public GameObject leftBoundary;
    public GameObject rightBoundary;

    [Header("UI Score")]
    private int score = 000;

    [Header("Enemy Sprite Change")]
    public List<Sprite> enemySprite;
    [Header("Score Update")]
    public Text heartLabel;
    public uint numberOfLife;
    public Text scoreLabel;
    public Text gameOver;
    public Button restartButton;
    // public Text lifeLabel;
    public GameObject minusOne; 
    Coroutine lastRoutine = null;

    void Awake()
    {
        shared = this;
    }
    void Start()
    {
        lastRoutine = StartCoroutine(SpawnEnemyWave());
        //SpriteRenderer spriteRenderer = enemyType1.GetComponent<SpriteRenderer>();
        //Debug.Log(spriteRenderer.sprite.name);
        //scoreLabel.text = score.ToString();
        //restartButton.gameObject.SetActive(false);
        numberOfLife = 3; 
        heartLabel.text = numberOfLife.ToString();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnEnemyWave()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            
            float bla = Random.Range(0.0f, 10.0f); 
            for (int i=1; i <= enemyPerWave; i++){
                Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight + 2, 0));
                Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight + 2, 0));
                Vector3 enemyPositon = new Vector3(Random.Range(topLeft.x, topRight.x), topLeft.y, 0);
                Quaternion enemyRotation = Quaternion.Euler(0, 0, 180);
               
                if (bla >= 5.0f)
                {
                    SpriteRenderer spriteRenderer = enemyType2.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = enemySprite[Random.Range(0, enemySprite.Count - 1)];
                    Instantiate(enemyType1, enemyPositon, enemyRotation);
                }
                else{
                    SpriteRenderer spriteRenderer = enemyType2.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = enemySprite[Random.Range(0, enemySprite.Count - 1)];
                    Instantiate(enemyType2, enemyPositon, enemyRotation);
                }
                yield return new WaitForSeconds(spawnInterval);
            }
            yield return new WaitForSeconds(waveInterval);
        }
    }

    public void ScoreIncrement(int increment)
    {
        score += increment;
       // Debug.Log("Score " + score);
        scoreLabel.text = score.ToString();
    }
    public void ShowGameOver()
    {
        numberOfLife -= 1;
        heartLabel.text = numberOfLife.ToString();
        Instantiate(minusOne, Vector3.zero, Quaternion.identity);
        
        if (numberOfLife <= 0)
        {
           
            gameOver.rectTransform.anchoredPosition3D = new Vector3(0, 60, 0);
            restartButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            StopCoroutine(lastRoutine);
        }
        
        //Debug.Log("Game Over. Add some GUI");
        //gameOver.gameObject.SetActive(true);
        //restartButton.gameObject.SetActive(true);
    }
    public void Restart()
    {
        scoreLabel.text = "00"; 
        SceneManager.LoadScene("SampleScene");
    }
}

```

### ShipController
1. Ship Move Left-Right, Up-Down (In between boundary) 
2. Shoot according to Turret condition. Power catch, Weapon upgrade, Scatter Shoot.
*ShipController.cs*
```C#
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


```

### Enemy Drone Controller
1. Enemy destroy by boundary, player bullet
2. Instantiate enemy Shoot, PowerUp object
*EnemyDroneController.cs*
```C#
 
public class EnemyDroneController : MonoBehaviour
{
    public float minReloadTime = 1.0f;
    public float maxReloatTime = 2.0f;
    public GameObject enemySmallBullet;
    public GameObject powerup;
    public GameObject explosion;
    private AudioSource enemyShootSound;
    public List<Sprite> powerupSprite;
    void Start()
    {
        enemyShootSound = gameObject.GetComponent<AudioSource>();
        StartCoroutine("Shoot");
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(Random.Range(minReloadTime, maxReloatTime));
        while (true)
        {
            Instantiate(enemySmallBullet, gameObject.transform.position, gameObject.transform.rotation);
            enemyShootSound.Play();
            yield return new WaitForSeconds(Random.Range(minReloadTime, maxReloatTime));
        }
    }
    // Enemy Destroy by Boundary
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Boundary") && other.gameObject.name != "Top Boundary")
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("PlayerBullet"))
        {
            GameController.shared.ScoreIncrement(100);
            
            float powerUp = Random.Range(0, 10.0f);
            if(powerUp > 5.0f)
            {
                SpriteRenderer powerupRenderer = powerup.GetComponent<SpriteRenderer>();
                powerupRenderer.sprite = powerupSprite[Random.Range(0, powerupSprite.Count - 1)];

                Instantiate(powerup, gameObject.transform.position, gameObject.transform.rotation);
            }
            Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}

```

### Basic Movement
1. Fall for mass of Rigidbody
BasicMovement.cs
**Plug & Play**
```C#
public class BasicMovement : MonoBehaviour
{

	private Rigidbody2D objectRigidbody;
	public float speed;

	
	void Start()
	{
		objectRigidbody = transform.GetComponent<Rigidbody2D>();
		
		objectRigidbody.velocity = transform.up * speed; // Enemy are rotated at 180
		//print(objectRigidbody.velocity);
	}
}

```

### Random Movement
1. Spider hanging movement randomly
RandomMovement.cs
**Plug & Play**
```C#

public class RandomMovement : MonoBehaviour
{

    public float moveSpeed = 5.0f;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float randomX;
    private float randomY;
    private float tChange = 0f;

    void Start()
    {
        minX = GameController.shared.leftBoundary.transform.position.x;
        maxX = GameController.shared.rightBoundary.transform.position.x;
        minY = GameController.shared.bottomBoundary.transform.position.y;
        maxY = GameController.shared.topBoundary.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= tChange)
        {
            //randomX = Random.Range(minX, maxX);
            //randomY = Random.Range(minY, maxY); 
            randomX = Random.Range(-2.0f, 2.0f);
            randomY = Random.Range(-2.0f, 2.0f);
            tChange = Time.time + Random.Range(0.5f, 1.5f);
        }
        Vector3 newPosition = new Vector3(randomX, randomY, 0);
        transform.Translate(newPosition * moveSpeed * Time.deltaTime);

        if (transform.position.x >= maxX || transform.position.x <= minX)
        {
            randomX = -randomX;
        }
        if (transform.position.y >= maxY || transform.position.y <= minY)
        {
            randomY = -randomY;
        }
        Vector3 transformPosition = transform.position;
        transformPosition.x = Mathf.Clamp(transform.position.x, minX, maxX);
        transformPosition.y = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = transformPosition;
    }
}
```

### Destroy by Boundary
1. Attached object will destroy by boundary (Collider must Trigger)
DestroyByBoundary.cs
**Plug & Play**
```C#
public class DestroyByBoundary : MonoBehaviour
{
   
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Boundary"))
        {
            Destroy(gameObject); // script carrier object will destroy

        }
       
    }
}
```

### Particle parent gameObject will be destroy when particalSystem stop emission, for any particle system on UI
**Plug & Play**
```C#
public class ParticleSystemAutoDestroy : MonoBehaviour
{
private ParticleSystem ps;
   
   void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }
   public void Update()
    {
        if(ps) {
            if (!ps.IsAlive())
            {
               Destroy(gameObject);
            }
        }
    }
}
```

<a href="https://www.raywenderlich.com/847-object-pooling-in-unity" target="_blank">...l</a>
