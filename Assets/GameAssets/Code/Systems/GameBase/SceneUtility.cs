using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneUtility
{
    // Keep track of Async operations
    private Queue<AsyncOperation> operations;
    private HashSet<string> openedScenes;

    private string currentScene;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public SceneUtility()
    {
        operations = new Queue<AsyncOperation>();
        openedScenes = new HashSet<string>();
        InitializeOpenedScenes();
    }

    public void ChangeScene(string nextSceneName, bool unload=true)
    {
        string currentActiveScene = SceneManager.GetActiveScene().name;
        if (unload && currentScene != null)
        {
            //eventBrokerComponent.Publish(this, new TransitionEvents.FadeScreen(true, 1f));
            //yield return new WaitForSeconds(1f);
            UnloadScene(currentScene);
        }
        LoadScene(nextSceneName == null ? currentActiveScene : nextSceneName);
        
    }

    public void LoadScene(string sceneName)
    {
        if (openedScenes.Contains(sceneName))
        {
            Debug.LogError($"{sceneName} is opened already");
            // TODO: Shouldn't return, it should still set the scene as the active scene
            return;
        }

        AsyncOperation addedScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operations.Enqueue(addedScene);
        openedScenes.Add(sceneName);
        currentScene = sceneName;
    }

    public void UnloadScene(string sceneName)
    {
        if (!openedScenes.Contains(sceneName))
        {
            Debug.LogError($"{sceneName} can not be found");
            return;
        }

        AsyncOperation removedScene = SceneManager.UnloadSceneAsync(sceneName);
        operations.Enqueue(removedScene);
        openedScenes.Remove(sceneName);
    }

    public IEnumerator WaitForSceneLoad(Action onLoadFinished = null)
    {
        while (operations.Count > 0)
        {
            if (operations.Peek().isDone)
            {
                operations.Dequeue();
            }
            yield return null;
        }

        onLoadFinished?.Invoke();
        //eventBrokerComponent.Publish(this, new SceneEvents.SceneLoaded());
    }

    public void SetActiveScene(string sceneName)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName == null ? currentScene : sceneName));
    }


    private void InitializeOpenedScenes()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            openedScenes.Add(SceneManager.GetSceneAt(i).name);
        }
    }
}