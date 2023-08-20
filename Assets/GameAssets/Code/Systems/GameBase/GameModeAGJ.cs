using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameModeAGJ : GameModeBase
{
    List<AugmentBase> possibleAugments = new List<AugmentBase>();
    [SerializeField] private AugmentUI augmentUI;

    protected override void Start()
    {
        base.Start();
        possibleAugments.AddRange(Augments.All());
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        eventBrokerComponent.Subscribe<AugmentEvents.SetAugmentPanelVisibility>(SetAugmentPanelVisiblityHandler);
        eventBrokerComponent.Subscribe<AugmentEvents.SelectAugment>(SelectAugmentHandler);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        eventBrokerComponent.Unsubscribe<AugmentEvents.SetAugmentPanelVisibility>(SetAugmentPanelVisiblityHandler);
        eventBrokerComponent.Unsubscribe<AugmentEvents.SelectAugment>(SelectAugmentHandler);
    }

    private void SelectAugmentHandler(BrokerEvent<AugmentEvents.SelectAugment> inEvent)
    {
        // Not storing selected augments for now...
        AugmentBase augment = inEvent.Payload.Augment;
        augment.Apply();
        possibleAugments.Remove(augment);
        augmentUI.gameObject.SetActive(false);

        GameStateEvents.GetNextSceneName getNextSceneName = new GameStateEvents.GetNextSceneName();
        eventBrokerComponent.Publish(this, getNextSceneName);
        eventBrokerComponent.Publish(this, new GameModeEvents.ChangeScene(getNextSceneName.Name));
    }

    private void SetAugmentPanelVisiblityHandler(BrokerEvent<AugmentEvents.SetAugmentPanelVisibility> inEvent)
    {
        augmentUI.gameObject.SetActive(inEvent.Payload.Visible);
        if (inEvent.Payload.Visible)
        {
            AugmentBase augment1 = SelectRandomAugment();
            AugmentBase augment2 = SelectRandomAugment();
            augmentUI.SetAugmentCards(augment1, augment2);
        }
    }

    private AugmentBase SelectRandomAugment()
    {
        return possibleAugments[Random.Range(0, possibleAugments.Count)];
    }
}
