using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Based on code from TheKiwiCoder: https://www.youtube.com/watch?v=_I8HsTfKep8&t=1s
//Standard assets for animation
//Mixamo for Pistol idle animation
//POLYGON starter pack https://assetstore.unity.com/packages/3d/props/polygon-starter-pack-low-poly-3d-art-by-synty-156819?aid=1011ljjCh&utm_campaign=unity_affiliate&utm_medium=affiliate&utm_source=partnerize-linkmaker
public class CharacterLocomotion : MonoBehaviour
{
    Animator animator;
    Vector2 input;
    bool IsSprinting = false;
    // Start is called before the first frame update  
    public float boundaryXMin = 0f;
    public float boundaryXMax = 4.4f;
    public float boundaryZMin = 0f;
    public float boundaryZMax = 4.4f;

    public AudioClip walkingSound;
    public AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //get horizontal and vertical inputs, pass to animator to drive movement
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        Vector3 currentPosition = transform.position;
        // locks postion within a range
        currentPosition.x = Mathf.Clamp(currentPosition.x, boundaryXMin, boundaryXMax);
        currentPosition.z = Mathf.Clamp(currentPosition.z, boundaryZMin, boundaryZMax);
        transform.position = currentPosition;

        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);

        if (audioSource != null && walkingSound != null && input.x > 0 && input.y > 0)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(walkingSound);
        }

        // if(Input.GetKeyDown(KeyCode.LeftShift))
        // {
        //     IsSprinting = !IsSprinting;

        //     animator.SetLayerWeight(1, IsSprinting ? 0 : 1); //lower aiming animation when running. Aimlayer is on layer 1
        //     animator.SetBool("IsSprinting", IsSprinting);
        // }
    }
}
