using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public void Interact()
    {
        eventBrokerComponent.Publish(this, new InteractionEvents.OpenDoor());
    }
}
