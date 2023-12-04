using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public AudioClip collisionSound;
    public AudioSource audioSource;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Player" && audioSource != null)
        {
            audioSource.PlayOneShot(collisionSound);
        }
    }
}
