using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.VolumeComponent;

public class GameMode : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    //private CutsceneSystem cutsceneSystem;

    private SceneUtility sceneUtility;
    private void Awake()
    {
        sceneUtility = new SceneUtility();
    }

    private void Start()
    {
        LoadMainMenu();
    }

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<GameModeEvents.ChangeScene>(ChangeSceneHandler);
    }

    private void OnDisable()
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
