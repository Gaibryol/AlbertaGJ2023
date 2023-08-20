using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeBase : MonoBehaviour
{
    protected EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private SceneUtility sceneUtility;

    protected virtual void Awake()
    {
        sceneUtility = new SceneUtility();
    }

    protected virtual void Start()
    {
        LoadMainMenu();
    }

    protected virtual void OnEnable()
    {
        eventBrokerComponent.Subscribe<GameModeEvents.ChangeScene>(ChangeSceneHandler);
    }

    protected virtual void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<GameModeEvents.ChangeScene>(ChangeSceneHandler);
    }

    private void ChangeSceneHandler(BrokerEvent<GameModeEvents.ChangeScene> inEvent)
    {
        sceneUtility.ChangeScene(inEvent.Payload.SceneName);
        StartCoroutine(sceneUtility.WaitForSceneLoad(() =>
        {
            sceneUtility.SetActiveScene(inEvent.Payload.SceneName);
        }));
    }

    private void LoadMainMenu()
    {
        sceneUtility.LoadScene(Constants.SceneNames.EnemyTest);
        StartCoroutine(sceneUtility.WaitForSceneLoad(() =>
        {
            sceneUtility.SetActiveScene(Constants.SceneNames.EnemyTest);
        }));
    }
}
