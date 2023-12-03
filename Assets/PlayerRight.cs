using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerRight : MonoBehaviour
{
    //create private internal references
    private InputActions inputActions;
    private InputAction movement;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); //get rigidbody, responsible for enabling collision with other colliders
        inputActions = new InputActions(); //create new InputActions
        movement = inputActions.PlayerRight.Movement; //get reference to movement action
    }
    //called when script enabled
    private void OnEnable()
    {
        movement.Enable();
    }

    //called when script disabled
    private void OnDisable()
    {
        movement.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 v2 = movement.ReadValue<Vector2>(); //extract 2d input data
        // Debug.Log(v2.x + " " + v2.y );
        Vector3 v3 = new Vector3(0, v2.y, 0); //convert to 3d space
                                                 // transform.Translate(v3); //moves transform, ignoring physics
        rb.AddForce(v3, ForceMode.VelocityChange); //apply instant physics force, ignoring mass
    }
}