using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform hitSphere;

    Ray ray;
    RaycastHit hitInfo;
    public LayerMask layerMask;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //set up the ray to start shooting from the camera
        ray.origin = Camera.main.transform.position;
        ray.direction = Camera.main.transform.forward;
        float distance = 100;

        //if ray hits something, put the hitSphere at the location ray hit
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
        {
            //We hit something!
            hitSphere.gameObject.SetActive(true);
            hitSphere.position = hitInfo.point;
        }
        else
        {
            //hitSphere.gameObject.SetActive(false);
            hitSphere.position = ray.origin + ray.direction * distance;
        }

        Debug.DrawLine(ray.origin, hitSphere.position, Color.red, 3f);
    }
}
