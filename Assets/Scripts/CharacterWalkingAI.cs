using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWalkingAI : MonoBehaviour
{
    private float speed = 1f;
    private float minTurnAngle = 15.0f;
    private float maxTurnAngle = 45.0f;
    private float raycastDistance = 1f;
    private Animator animator;
    public float boundaryXMin = 0f;
    public float boundaryXMax = 4.4f;
    public float boundaryZMin = 0f;
    public float boundaryZMax = 4.4f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (IsBoundry()){
            Vector3 currentPosition = transform.position;
            // locks postion within a range
            currentPosition.x = Mathf.Clamp(currentPosition.x, boundaryXMin, boundaryXMax);
            currentPosition.z = Mathf.Clamp(currentPosition.z, boundaryZMin, boundaryZMax);
            transform.position = currentPosition;

            float turnDirection = Random.Range(0, 2) == 0 ? -1f : 1f; 
            float randomTurnAngle = Random.Range(120, 240);
            transform.Rotate(0, turnDirection * randomTurnAngle, 0);
        }
        else if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            float turnDirection = Random.Range(0, 2) == 0 ? -1f : 1f; 
            float randomTurnAngle = Random.Range(minTurnAngle, maxTurnAngle);
            transform.Rotate(0, turnDirection * randomTurnAngle, 0);
        } 
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            animator.SetFloat("InputX", speed);
            animator.SetFloat("InputY", 0.0f);
        }
    }

    bool IsBoundry()
    {
        Vector3 currentPosition = transform.position;
        return currentPosition.x < boundaryXMin || currentPosition.x > boundaryXMax || currentPosition.z < boundaryZMin || currentPosition.z > boundaryZMax;
    }
}