using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class Debugging : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(XRController.leftHand);
        Debug.Log(XRController.rightHand);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(XRController.leftHand);
        Debug.Log(XRController.rightHand);
    }
}
