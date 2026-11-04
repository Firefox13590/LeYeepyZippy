using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class ChargementMIniJeux : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var item in InputSystem.devices)
        {
            Debug.Log(item);
            Debug.Log(item.description);
        }

        Debug.Log(XRController.leftHand);
        Debug.Log(XRController.rightHand);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadDuel()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMario()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadCrusher()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadSniper()
    {
        SceneManager.LoadScene(4);
    }
}
