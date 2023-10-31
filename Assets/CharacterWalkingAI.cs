using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWalkingAI : MonoBehaviour
{
    public float speed = .1f;
    public float turnAngle = 120.0f;
    Animator animator;
    public float backupDistance = .5f; 
    private bool isColliding = false;
    private Vector3 backupPosition;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isColliding)
        {
            // Move forward
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            animator.SetFloat("InputX", 1.0f); 
            animator.SetFloat("InputY", 0.0f);
        }
        else
        {
            // Back up
            transform.Translate(Vector3.back * speed * Time.deltaTime);

            // Check if the character has backed up sufficiently
            if (Vector3.Distance(transform.position, backupPosition) >= backupDistance)
            {
                isColliding = false;
                transform.Rotate(0, turnAngle, 0);
            }
        }

        // input.x = Input.GetAxis("Horizontal");
        // input.y = Input.GetAxis("Vertical");

        // animator.SetFloat("InputX", input.x);
        // animator.SetFloat("InputY", input.y);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        isColliding = true;

        backupPosition = transform.position;
    }
}
