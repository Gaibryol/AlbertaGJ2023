using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameModeAGJ : GameModeBase
{
    List<AugmentBase> possibleAugments = new List<AugmentBase>();
    [SerializeField] private AugmentUI augmentUI;
    [SerializeField] private EnemyRangedStats enemyRangedStats;
    [SerializeField] private EnemyMeleeStats enemyMeleeStats;

    [SerializeField] GameObject ContinueScreen;
    
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
        eventBrokerComponent.Subscribe<EnemyEvents.GetEnemyMeleeStats>(GetEnemyMeleeStatsHandler);
        eventBrokerComponent.Subscribe<EnemyEvents.GetEnemyRangedStats>(GetEnemyRangedStatsHandler);

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        eventBrokerComponent.Unsubscribe<AugmentEvents.SetAugmentPanelVisibility>(SetAugmentPanelVisiblityHandler);
        eventBrokerComponent.Unsubscribe<AugmentEvents.SelectAugment>(SelectAugmentHandler);
        eventBrokerComponent.Unsubscribe<EnemyEvents.GetEnemyMeleeStats>(GetEnemyMeleeStatsHandler);
        eventBrokerComponent.Unsubscribe<EnemyEvents.GetEnemyRangedStats>(GetEnemyRangedStatsHandler);
    }

    private void GetEnemyRangedStatsHandler(BrokerEvent<EnemyEvents.GetEnemyRangedStats> inEvent)
    {
        inEvent.Payload.EnemyRangedStats = enemyRangedStats;
    }

    private void GetEnemyMeleeStatsHandler(BrokerEvent<EnemyEvents.GetEnemyMeleeStats> inEvent)
    {
        inEvent.Payload.EnemyMeleeStats = enemyMeleeStats;
    }

    private void SelectAugmentHandler(BrokerEvent<AugmentEvents.SelectAugment> inEvent)
    {
        // Not storing selected augments for now...
        AugmentBase augment = inEvent.Payload.Augment;
        augment.Apply();
        possibleAugments.Remove(augment);
        augmentUI.gameObject.SetActive(false);
    }

    private void SetAugmentPanelVisiblityHandler(BrokerEvent<AugmentEvents.SetAugmentPanelVisibility> inEvent)
    {
        augmentUI.gameObject.SetActive(inEvent.Payload.Visible);
        if (inEvent.Payload.Visible)
        {
            AugmentBase augment1 = SelectRandomAugment(0, possibleAugments.Count/2);
            AugmentBase augment2 = SelectRandomAugment(possibleAugments.Count/2, possibleAugments.Count);
            augmentUI.SetAugmentCards(augment1, augment2);
        }
        eventBrokerComponent.Publish(this, new GameStateEvents.SetPlayerControllerState(!inEvent.Payload.Visible));
    }


    private AugmentBase SelectRandomAugment(int min, int max)
    {
        return possibleAugments[Random.Range(min, max)];
    }

    protected override void PlayerDeathHandler(BrokerEvent<GameModeEvents.PlayerDeath> inEvent)
    {
        base.PlayerDeathHandler(inEvent);
        Color endColor = backroundFadePanel.color;
        endColor.a = 1;
        StartCoroutine(Fade(backroundFadePanel.color, endColor, 2f, () => {
            ContinueScreen.SetActive(true);
        }));
    }

    public void ContinueScreenContinueButton()
    {
        eventBrokerComponent.Publish(this, new GameModeEvents.ChangeScene(null));
        ContinueScreen.SetActive(false);
    }
}
