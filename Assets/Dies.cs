using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dies : MonoBehaviour
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameManagerComponent.RestartGame();
        }
    }
}

