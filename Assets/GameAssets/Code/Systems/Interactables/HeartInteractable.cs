using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartInteractable : MonoBehaviour, IInteractable
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public void Interact()
    {
        eventBrokerComponent.Publish(this, new HealthEvents.IncreasePlayerHealth(1));
		eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.HealthPack));
        Destroy(gameObject);
    }
}
