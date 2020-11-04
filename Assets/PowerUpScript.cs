using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PowerUpScript : MonoBehaviour
{
    private CircleCollider2D powerupCollider;
    private Renderer powerupRender;
    private AudioSource powerAudioSource;

    void Start()
    {
        powerupCollider = gameObject.GetComponent<CircleCollider2D>();
        powerupRender = gameObject.GetComponent<Renderer>();
        powerAudioSource = gameObject.GetComponent<AudioSource>();

    }

     public void PowerCollected()
    {
        powerupCollider.enabled = false;
        powerupRender.enabled = false;
        powerAudioSource.Play();
        Destroy(gameObject, powerAudioSource.clip.length);
    }
}
