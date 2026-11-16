using Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public bool isHost = false;
    public float mvtSpeed = 5, rotSpeed = .1f;
    public float interactRange = 3;
    public bool debugMode = false;

    Camera cam;
    Rigidbody rb;
    InputAction moveAction, lookAction, InteractAction;
    Vector3 moveInput, lookInput;
    float interactInput;
    RaycastHit hit;

    private void Awake()
    {
        cam = transform.GetChild(0).GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();

        moveAction = InputSystem.actions.FindAction("Player/Move");
        lookAction = InputSystem.actions.FindAction("Player/Look");
        InteractAction = InputSystem.actions.FindAction("Player/Interact");
    }

    // Update is called once per frame
    void Update()
    {
        if (debugMode)
        {
            Debug.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * interactRange, Color.red);
        }

        moveInput = (transform.forward * moveAction.ReadValue<Vector2>().y + transform.right * moveAction.ReadValue<Vector2>().x) * mvtSpeed;
        lookInput = new Vector3(-lookAction.ReadValue<Vector2>().y, lookAction.ReadValue<Vector2>().x, 0) * rotSpeed;
        interactInput = InteractAction.ReadValue<float>();

        transform.Rotate(0, lookInput.y, 0);

        cam.transform.Rotate(lookInput.x, 0, 0);
        cam.transform.localRotation =
            new(Mathf.Clamp(cam.transform.localRotation.x, Quaternion.Euler(-90, 0, 0).x, Quaternion.Euler(90, 0, 0).x),
            cam.transform.localRotation.y,
            cam.transform.localRotation.z,
            cam.transform.localRotation.w);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactRange))
        {
            //Debug.Log(hit.collider.gameObject, hit.collider.gameObject);
            if (hit.collider.TryGetComponent<InteractionObject>(out InteractionObject interObj))
            {
                //Debug.Log(hit.collider.gameObject, hit.collider.gameObject);
                if (interObj.interaction == Interaction.BoutonPlateforme)
                {
                    MarioParty.boutonHighlight = hit.collider.gameObject;
                }

                if (interactInput > .1f) interObj.Interact();
            }
            else
            {
                if (isHost)
                {
                    MarioParty.boutonHighlight = null;
                }
            }
        }
        else
        {
            if (isHost)
            {
                MarioParty.boutonHighlight = null;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveInput.x, rb.linearVelocity.y, moveInput.z);
    }
}
