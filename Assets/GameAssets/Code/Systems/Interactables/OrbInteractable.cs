using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbInteractable : MonoBehaviour, IInteractable
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public void Interact()
    {
        Debug.Log("Interacting with orb");
        eventBrokerComponent.Publish(this, new InteractionEvents.IncreaseOrbs());
        Destroy(gameObject);
    }
}
