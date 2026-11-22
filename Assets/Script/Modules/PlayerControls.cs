using Data;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerControls : MonoBehaviour
{
    public static Action<GameObject> OnPlayerDeath;

    [Header("Affectation dans l'inspecteur")]
    public bool isHost = false;
    public float mvtSpeed = 5, rotSpeed = .1f;
    public float interactRange = 3;
    public bool debugMode = false;

    [Header("Visualisation des variables")]
    [SerializeField, ReadOnly]
    bool canMove = true;

    Camera cam;
    Rigidbody rb;
    InputAction moveAction, lookAction, interactAction;
    Vector3 moveInput, lookInput;
    float interactInput;
    RaycastHit hit;
    bool isDead = false;

    private void Awake()
    {
        cam = transform.GetChild(0).GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();

        moveAction = InputSystem.actions.FindAction("Player/Move");
        lookAction = InputSystem.actions.FindAction("Player/Look");
        interactAction = InputSystem.actions.FindAction("Player/Interact");

        //canMove = isHost;
    }
    private void OnEnable()
    {
        GameManager.OnMinigameEnd += () => canMove = false;
    }
    private void OnDisable()
    {
        GameManager.OnMinigameEnd -= () => canMove = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (debugMode)
        {
            Debug.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * interactRange, Color.red);
        }

        moveInput = (canMove) ?
            (transform.forward * moveAction.ReadValue<Vector2>().y + transform.right * moveAction.ReadValue<Vector2>().x) * mvtSpeed
            : Vector3.zero;
        lookInput =
            new Vector3(-lookAction.ReadValue<Vector2>().y, lookAction.ReadValue<Vector2>().x, 0) * rotSpeed;
        interactInput = interactAction.ReadValue<float>();

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
            if (hit.collider.TryGetComponent(out InteractionObject interObj))
            {
                //Debug.Log(hit.collider.gameObject, hit.collider.gameObject);
                if (interObj.interaction == Interaction.BoutonPlateforme)
                {
                    MarioParty.Instance.boutonHighlight = hit.collider.gameObject;
                }

                if (interactInput > .1f) interObj.Interact();
            }
            else
            {
                if (isHost)
                {
                    MarioParty.Instance.boutonHighlight = null;
                }
            }
        }
        else
        {
            if (isHost)
            {
                MarioParty.Instance.boutonHighlight = null;
            }
        }
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveInput.x, rb.linearVelocity.y, moveInput.z);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            if (!isDead) OnPlayerDeath?.Invoke(gameObject);

            isDead = true;
            canMove = false;

            rb.constraints = RigidbodyConstraints.None;
            cam.gameObject.SetActive(false);

            Vector3 ragdollForce, ragdollTorque;
            ragdollForce = new Vector3(
                Random.Range(1f, 10f),
                Random.Range(1f, 10f),
                Random.Range(1f, 10f));
            //Debug.Log(ragdollForce);
            rb.AddForce(ragdollForce * mvtSpeed, ForceMode.Impulse);
            //rb.AddForce(ragdollForce, ForceMode.Impulse);

            ragdollTorque = new Vector3(
                Random.Range(1f, 10f),
                Random.Range(1f, 10f),
                Random.Range(1f, 10f));
            //Debug.Log(ragdollTorque);
            rb.AddTorque(ragdollTorque * mvtSpeed, ForceMode.Impulse);
            //rb.AddTorque(ragdollTorque);

            Invoke(nameof(Death), 10f);
        }
    }



    void Death()
    {
        Destroy(gameObject);
    }
}
