using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float destroyTime = 2f;
    public AudioClip wallHitSound;
    public AudioClip floorHitSound;
    public AudioClip enemyHitSound;
    private AudioSource audioSource;
    public GameManager gameManagerObject;
    private GameManager gameManagerComponent;
    private void Start()
    {
        Destroy(gameObject, destroyTime);
        audioSource = GetComponent<AudioSource>();

        if (gameManagerObject != null)
        {
            gameManagerComponent = gameManagerObject.GetComponent<GameManager>();
        }
    }

    private void OnEnable()
    {
        Destroy(gameObject, destroyTime);
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the ball prefab.");
            return;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            PlaySound(wallHitSound);
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            PlaySound(floorHitSound);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            PlaySound(enemyHitSound);
            if (gameManagerComponent != null)
            {
                gameManagerComponent.IncreasePlayerScore();
            }
            Destroy(gameObject); // Destroy immediately
        }
    }

    private void PlaySound(AudioClip sound)
    {
        if (sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }
}