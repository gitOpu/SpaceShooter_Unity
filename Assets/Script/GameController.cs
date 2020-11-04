using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
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
