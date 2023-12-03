using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDoor : MonoBehaviour
{
    public GameManager gameManagerObject;
    private GameManager gameManagerComponent;

    private void Start()
    {
        if (gameManagerObject != null)
        {
            gameManagerComponent = gameManagerObject.GetComponent<GameManager>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with : " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManagerComponent.LoadMiniGameScene();
        }
    }
}
