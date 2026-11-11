using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public bool isHost = false;
    public float mvtSpeed = 5, rotSpeed = .1f;

    Camera cam;
    Rigidbody rb;
    InputAction moveAction, lookAction;
    Vector3 moveFinal, lookFinal;

    private void Awake()
    {
        cam = transform.GetChild(0).GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();

        moveAction = InputSystem.actions.FindAction("Player/Move");
        lookAction = InputSystem.actions.FindAction("Player/Look");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2
            moveInput = moveAction.ReadValue<Vector2>(),
            lookInput = lookAction.ReadValue<Vector2>();
        moveFinal = (transform.forward * moveInput.y + transform.right * moveInput.x) * mvtSpeed;
        lookFinal = new Vector3(-lookInput.y, lookInput.x, 0) * rotSpeed;

        transform.Rotate(0, lookFinal.y, 0);

        cam.transform.Rotate(lookFinal.x, 0, 0);
        cam.transform.localRotation =
            new(Mathf.Clamp(cam.transform.localRotation.x, Quaternion.Euler(-90, 0, 0).x, Quaternion.Euler(90, 0, 0).x),
            cam.transform.localRotation.y,
            cam.transform.localRotation.z,
            cam.transform.localRotation.w);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveFinal.x, rb.linearVelocity.y, moveFinal.z);
    }
}
