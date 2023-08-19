using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartInteractable : MonoBehaviour, IInteractable
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public void Interact()
    {
        Debug.Log("Interacting with Heart");
        eventBrokerComponent.Publish(this, new HealthEvents.IncreasePlayerHealth(1));
        Destroy(gameObject);
    }
}
