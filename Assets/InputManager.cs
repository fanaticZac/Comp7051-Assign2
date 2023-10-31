using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //create private internal references
    private InputActions inputActions;
    private InputAction movement;
    private InputAction look;
    private InputAction walls;
    private CapsuleCollider capsuleCollider;
    Rigidbody rb;
    Animator animator;
    Vector2 input;
    bool IsSprinting = false;
    public float turnSpeed = 15;
    public float speed = 3;

    Camera mainCamera;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); //get rigidbody, responsible for enabling collision with other colliders
        inputActions = new InputActions(); //create new InputActions
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

    }

    //called when script enabled
    private void OnEnable()
    {
        movement = inputActions.Player.movement; //get reference to movement action
        movement.Enable();

        look = inputActions.Player.look;
        look.Enable();

        walls = inputActions.Player.walls;
        walls.Enable();

        //create a DoJump callback function
        //DoJump automatically called when Jump binding performed
        // inputActions.Player.Jump.performed += DoJump;
        // inputActions.Player.Jump.Enable();
    }

    //called when script disabled
    private void OnDisable()
    {
        movement.Disable();
        look.Disable();
        walls.Disable();
    }

    private void Update(){
        if (walls.triggered) { 
           ToggleCollider(); 
        }

        // if(Input.GetKeyDown(KeyCode.LeftShift))
        // {
        //     IsSprinting = !IsSprinting;

        //     animator.SetLayerWeight(1, IsSprinting ? 0 : 1); //lower aiming animation when running. Aimlayer is on layer 1
        //     animator.SetBool("IsSprinting", IsSprinting);
        // }
    }

    private void ToggleCollider()
    {
        capsuleCollider.enabled = !capsuleCollider.enabled;
    }

    //called every physics update
    private void FixedUpdate()
    {
        Vector2 moveInput = movement.ReadValue<Vector2>();

        // Get the camera's forward and right vectors
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // Ensure the vertical component is zero
        cameraForward.y = 0;
        cameraRight.y = 0;

        // Normalize vectors
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the movement direction
        Vector3 movementDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        // Apply movement
        rb.velocity = movementDirection * speed;

        input.x = movementDirection.x;
        input.y = movementDirection.y;

        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
    }

}
