using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateBase : MonoBehaviour
{
    [field: SerializeField] public GameObject WorldCanvas { get; private set; }
    public string NextSceneName;

    [SerializeField] private int NumberRequiredOrbs;
    private int numberOrbs;

    private int numberEnemies;

    private PlayerController playerController;

    private void Start()
    {
        numberEnemies = FindObjectsOfType<EnemyBase>().Length;
        playerController = FindObjectOfType<PlayerController>();
    }

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<InteractionEvents.IncreaseOrbs>(IncreaseOrbsHandler);
        eventBrokerComponent.Subscribe<EnemyEvents.EnemyDeath>(EnemyDeathHandler);
        eventBrokerComponent.Subscribe<InteractionEvents.OpenDoor>(OpenDoorHandler);
        eventBrokerComponent.Subscribe<GameStateEvents.GetWorldCanvas>(GetWorldCanvasHandler);
        eventBrokerComponent.Subscribe<GameStateEvents.GetNextSceneName>(GetNextSceneNameHandler);
        eventBrokerComponent.Subscribe<GameStateEvents.SetPlayerControllerState>(SetPlayerControllerStateHandler);
    }


    private void OnDisable()
    {
		eventBrokerComponent.Unsubscribe<InteractionEvents.IncreaseOrbs>(IncreaseOrbsHandler);
        eventBrokerComponent.Unsubscribe<EnemyEvents.EnemyDeath>(EnemyDeathHandler);
        eventBrokerComponent.Unsubscribe<InteractionEvents.OpenDoor>(OpenDoorHandler);
        eventBrokerComponent.Unsubscribe<GameStateEvents.GetWorldCanvas>(GetWorldCanvasHandler);
        eventBrokerComponent.Unsubscribe<GameStateEvents.GetNextSceneName>(GetNextSceneNameHandler);
        eventBrokerComponent.Unsubscribe<GameStateEvents.SetPlayerControllerState>(SetPlayerControllerStateHandler);
    }

    private void SetPlayerControllerStateHandler(BrokerEvent<GameStateEvents.SetPlayerControllerState> inEvent)
    {
        playerController.enabled = inEvent.Payload.Active;
    }

    private void GetNextSceneNameHandler(BrokerEvent<GameStateEvents.GetNextSceneName> inEvent)
    {
        inEvent.Payload.Name = NextSceneName;
    }

    private void GetWorldCanvasHandler(BrokerEvent<GameStateEvents.GetWorldCanvas> inEvent)
    {
        inEvent.Payload.WorldCanvas = WorldCanvas;
    }

    private void OpenDoorHandler(BrokerEvent<InteractionEvents.OpenDoor> inEvent)
    {
        if (numberEnemies != 0 /*|| numberOrbs != NumberRequiredOrbs*/) return;
        Debug.Log("Door opened");
        eventBrokerComponent.Publish(this, new AugmentEvents.SetAugmentPanelVisibility(true));
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
