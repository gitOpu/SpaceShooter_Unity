using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
