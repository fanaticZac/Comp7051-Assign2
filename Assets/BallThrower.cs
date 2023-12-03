using UnityEngine;
using UnityEngine.InputSystem;

public class BallThrower : MonoBehaviour
{
    public GameObject ballPrefab;
    public float throwForce = 10f;
    public float upwardForceMultiplier = 1.5f;

    private GameObject lastSpawnedBall;
    private InputActions inputActions;

    public Light flashLightObject;
    private Light flashLightComponent;

    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Player.throwAction.performed += ThrowBall;
        inputActions.Player.flashLight.performed += ToggleFlashLight;


        inputActions.Enable();

        if (flashLightObject != null)
        {
            flashLightComponent = flashLightObject.GetComponent<Light>();
        }
    }
    void Update()
    {

    }

    void ToggleFlashLight(InputAction.CallbackContext context)
    {
        if (flashLightComponent != null)
        {
            if (flashLightComponent.enabled)
            {
                flashLightComponent.enabled = false;
            }
            else
            {
                flashLightComponent.enabled = true;
            }
        }
    }
    void ThrowBall(InputAction.CallbackContext context)
    {
        Debug.Log("ThrowBall method called");


        if (ballPrefab == null)
        {
            Debug.LogError("Ball prefab not assigned in the inspector.");
            return;
        }

        if (lastSpawnedBall != null && lastSpawnedBall.activeSelf)
        {
            return;
        }

        lastSpawnedBall = Instantiate(ballPrefab, transform.position + transform.TransformVector(new Vector3(0f, 0f, 1f)), Quaternion.identity);

        lastSpawnedBall.SetActive(true);

        Rigidbody ballRigidbody = lastSpawnedBall.GetComponent<Rigidbody>();

        if (ballRigidbody != null)
        {
            Camera mainCamera = Camera.main;

            if (mainCamera != null)
            {
                Vector3 throwDirection = mainCamera.transform.forward + Vector3.up * upwardForceMultiplier;
                ballRigidbody.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("Main camera not found.");
            }
        }
        else
        {
            Debug.LogError("Ball prefab does not have a Rigidbody component.");
        }

        lastSpawnedBall = null;
    }
}
