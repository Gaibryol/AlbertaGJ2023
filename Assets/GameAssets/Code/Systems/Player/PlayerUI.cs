using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> healthBoxes;
    [SerializeField] private Slider dashBar;
    [SerializeField] private List<Image> normalComboBoxes;
    [SerializeField] private Image specialComboBox;
	[SerializeField] private GameObject heartBox;
	[SerializeField] private Transform heartContainer;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<UIEvents.SetHealth>(SetHealthHandler);
		eventBrokerComponent.Subscribe<UIEvents.SetMaxHealth>(SetMaxHealthHandler);
		eventBrokerComponent.Subscribe<UIEvents.SetDash>(SetDashHandler);
		eventBrokerComponent.Subscribe<UIEvents.SetMaxDash>(SetMaxDashHandler);
        eventBrokerComponent.Subscribe<UIEvents.SetCombo>(SetComboHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<UIEvents.SetHealth>(SetHealthHandler);
		eventBrokerComponent.Unsubscribe<UIEvents.SetMaxHealth>(SetMaxHealthHandler);
		eventBrokerComponent.Unsubscribe<UIEvents.SetDash>(SetDashHandler);
		eventBrokerComponent.Unsubscribe<UIEvents.SetMaxDash>(SetMaxDashHandler);
        eventBrokerComponent.Unsubscribe<UIEvents.SetCombo>(SetComboHandler);
    }

	private void SetComboHandler(BrokerEvent<UIEvents.SetCombo> inEvent)
    {
        // lazy way just disable everything first
        foreach (Image image in normalComboBoxes)
        {
            image.enabled = false;
        }
        specialComboBox.enabled = false;

        // Enable
        for (int i = 0; i < inEvent.Payload.Combo.Count; i++)
        {
            if (i == normalComboBoxes.Count - 1 && inEvent.Payload.Combo[i])
            {
                specialComboBox.enabled = true;
            } else
            {
                normalComboBoxes[i].enabled = true;
            }
        }
    }

    private void SetDashHandler(BrokerEvent<UIEvents.SetDash> inEvent)
    {
        dashBar.value = inEvent.Payload.Value;
    }

	private void SetMaxDashHandler(BrokerEvent<UIEvents.SetMaxDash> inEvent)
	{
		dashBar.maxValue = inEvent.Payload.Value;
	}

	private void SetHealthHandler(BrokerEvent<UIEvents.SetHealth> inEvent)
    {
        for (int i = 0; i < inEvent.Payload.Value; i++)
        {
			healthBoxes[i].transform.GetChild(0).gameObject.SetActive(true);
        }
        for (int i = inEvent.Payload.Value; i < healthBoxes.Count; i++)
        {
			healthBoxes[i].transform.GetChild(0).gameObject.SetActive(false);
		}
    }

	private void SetMaxHealthHandler(BrokerEvent<UIEvents.SetMaxHealth> inEvent)
	{
		foreach(Transform child in heartContainer)
		{
			Destroy(child.gameObject);
		}

		for (int i = 0; i < inEvent.Payload.Value; i++)
		{
			Instantiate(heartBox, heartContainer);
		}
	}
}
