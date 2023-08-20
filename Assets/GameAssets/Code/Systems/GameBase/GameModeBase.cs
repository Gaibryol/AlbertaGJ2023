using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeBase : MonoBehaviour
{
    protected EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private SceneUtility sceneUtility;

    [SerializeField] protected Image backroundFadePanel;

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
        eventBrokerComponent.Subscribe<GameModeEvents.PlayerDeath>(PlayerDeathHandler);

    }

    protected virtual void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<GameModeEvents.ChangeScene>(ChangeSceneHandler);
        eventBrokerComponent.Unsubscribe<GameModeEvents.PlayerDeath>(PlayerDeathHandler);

    }

    private void ChangeSceneHandler(BrokerEvent<GameModeEvents.ChangeScene> inEvent)
    {
        Color endColor = backroundFadePanel.color;
        endColor.a = 1;
        StartCoroutine(Fade(backroundFadePanel.color, endColor, 1f, () =>
        {
            sceneUtility.ChangeScene(inEvent.Payload.SceneName);
            StartCoroutine(sceneUtility.WaitForSceneLoad(() =>
            {
                sceneUtility.SetActiveScene(inEvent.Payload.SceneName);
                Color endColor = backroundFadePanel.color;
                endColor.a = 0;
                StartCoroutine(Fade(backroundFadePanel.color, endColor, 1f));
            }));
        }));
    }

    private void LoadMainMenu()
    {
        sceneUtility.LoadScene(Constants.SceneNames.Level3);
        StartCoroutine(sceneUtility.WaitForSceneLoad(() =>
        {
            sceneUtility.SetActiveScene(Constants.SceneNames.Level3);
            Color endColor = backroundFadePanel.color;
            endColor.a = 0;
            StartCoroutine(Fade(backroundFadePanel.color, endColor, 1f));
        }));
    }

    protected virtual void PlayerDeathHandler(BrokerEvent<GameModeEvents.PlayerDeath> inEvent)
    {
        
    }

    protected IEnumerator Fade(Color startColor, Color endColor, float fadeDuration, Action callback=null)
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startColor.a, endColor.a, elapsedTime / fadeDuration);
            backroundFadePanel.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        backroundFadePanel.color = endColor;
        callback?.Invoke();
    }
}
