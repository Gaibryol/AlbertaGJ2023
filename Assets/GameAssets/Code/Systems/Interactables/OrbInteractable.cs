using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbInteractable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interacting with orb");
        Destroy(gameObject);
    }
}
