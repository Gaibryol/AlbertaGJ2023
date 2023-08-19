using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateBase : MonoBehaviour
{
    [field: SerializeField] public GameObject WorldCanvas { get; private set; }

    [SerializeField] private int NumberRequiredOrbs;
    private int numberOrbs;

    private int numberEnemies;

    private void Start()
    {
        numberEnemies = FindObjectsOfType<EnemyBase>().Length;
    }

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<InteractionEvents.IncreaseOrbs>(IncreaseOrbsHandler);
        eventBrokerComponent.Subscribe<EnemyEvents.EnemyDeath>(EnemyDeathHandler);
        eventBrokerComponent.Subscribe<InteractionEvents.OpenDoor>(OpenDoorHandler);
        eventBrokerComponent.Subscribe<GameStateEvents.GetWorldCanvas>(GetWorldCanvasHandler);
    }


    private void OnDisable()
    {
		eventBrokerComponent.Unsubscribe<InteractionEvents.IncreaseOrbs>(IncreaseOrbsHandler);
        eventBrokerComponent.Unsubscribe<EnemyEvents.EnemyDeath>(EnemyDeathHandler);
        eventBrokerComponent.Unsubscribe<InteractionEvents.OpenDoor>(OpenDoorHandler);
        eventBrokerComponent.Unsubscribe<GameStateEvents.GetWorldCanvas>(GetWorldCanvasHandler);
    }

    private void GetWorldCanvasHandler(BrokerEvent<GameStateEvents.GetWorldCanvas> inEvent)
    {
        inEvent.Payload.WorldCanvas = WorldCanvas;
    }

    private void OpenDoorHandler(BrokerEvent<InteractionEvents.OpenDoor> @event)
    {
        if (numberEnemies != 0 || numberOrbs != NumberRequiredOrbs) return;
        Debug.Log("Door opened");
    }

    private void EnemyDeathHandler(BrokerEvent<EnemyEvents.EnemyDeath> inEvent)
    {
        numberEnemies--;
    }

    private void IncreaseOrbsHandler(BrokerEvent<InteractionEvents.IncreaseOrbs> inEvent)
    {
        numberOrbs += 1;
        eventBrokerComponent.Publish(this, new UIEvents.SetOrbs(numberOrbs));
    }
}
