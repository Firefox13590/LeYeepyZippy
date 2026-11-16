using Data;
using System;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    public static Action<Interaction> OnInteract;

    public Interaction interaction;



    public void Interact()
    {
        OnInteract.Invoke(interaction);
    }
}
