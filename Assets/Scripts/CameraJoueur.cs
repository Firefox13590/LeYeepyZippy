using UnityEngine;

public class CameraJoueur : MonoBehaviour
{
    public Transform cameraTransform;

    void Start()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Vector3 targetEuler = new Vector3(0f, cameraTransform.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Euler(targetEuler);
    }
}

