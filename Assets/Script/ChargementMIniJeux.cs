using UnityEngine;
using UnityEngine.SceneManagement;

public class ChargementMIniJeux : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
