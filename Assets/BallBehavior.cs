using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BallBehavior : MonoBehaviour
{
    private Vector3 direction;
    private float speed = .015f;
    private GameObject Ready;

    private Vector3 ballResetPosition;

    // Use this for initialization
    void Start()
    {
        Ready = GameObject.Find("ReadyPrompt");
        ballResetPosition = transform.position;
        Invoke("RestartBall", 3);
        
    }


    // Update is called once per frame
    void Update()
    {
        this.transform.position += direction * speed;
    }

    void RestartBall()
    {
        speed = .015f;
        Ready.GetComponent<TextMeshProUGUI>().text = "";

        transform.position = ballResetPosition;

        float minAngle = 30f * Mathf.Deg2Rad;
        float maxAngle = Mathf.PI / 2f;

        float angle = Random.Range(0, 2) * Mathf.PI / 2f + Random.Range(minAngle, maxAngle);

        if (Mathf.Abs(angle % (Mathf.PI / 2f)) < minAngle)
        {
            angle += (minAngle - Mathf.Abs(angle % (Mathf.PI / 2f)));
        }

        float randomX = Mathf.Cos(angle);
        float randomY = Mathf.Sin(angle);

        direction = new Vector3(randomX, randomY, 0.0f).normalized;

    }

    private void LastSecondTextChange()
    {
        Ready.GetComponent<TextMeshProUGUI>().text = "Go!";
        Invoke("RestartBall", 1);
    }
    public void StopEntirely()
    {
        // Set the ball's velocity to zero to stop it.
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        // Reset the ball's position and invoke the ResetBall function to restart it.
        transform.position = ballResetPosition;
        this.speed = 0.0f;
    }

    public void StopBall()
    {
        // Set the ball's velocity to zero to stop it.
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        // Reset the ball's position and invoke the ResetBall function to restart it.
        transform.position = Vector3.zero;
        Invoke("LastSecondTextChange", 1);
        this.speed = 0.0f;
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        direction = Vector3.Reflect(direction, normal);
    }
}
