using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public void StartGame()
    {
        eventBrokerComponent.Publish(this, new GameModeEvents.ChangeScene(Constants.SceneNames.Level1));
    }
}
