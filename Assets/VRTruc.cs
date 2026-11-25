using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(Application.platform);
        Debug.Log(XRSettings.isDeviceActive);

        if (verifiePresenceVR())
        {
            Debug.Log("Un casque VR est connecté et actif !");
        }
        else
        {
            Debug.Log("Aucun casque VR détecté.");
        }
    }

    bool verifiePresenceVR()
    {
        // Récupère la liste de tous les sous-systèmes XR

        List<XRDisplaySubsystem> displaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetSubsystems(displaySubsystems);
        Debug.Log(displaySubsystems.Count);
        

        foreach (var subsystem in displaySubsystems)
        {
            if (subsystem.running) // Vérifie si le sous-système XR est en marche
            {
                return true;
            }
        }
        return false;
    }
}
