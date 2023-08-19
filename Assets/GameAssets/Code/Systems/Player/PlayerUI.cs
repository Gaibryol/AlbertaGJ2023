using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private List<Image> healthBoxes;
    [SerializeField] private Slider dashBar;
    [SerializeField] private List<Image> orbBoxes;
    [SerializeField] private List<Image> normalComboBoxes;
    [SerializeField] private Image specialComboBox;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<UIEvents.SetHealth>(SetHealthHandler);
        eventBrokerComponent.Subscribe<UIEvents.SetDash>(SetDashHandler);
        eventBrokerComponent.Subscribe<UIEvents.SetOrbs>(SetOrbsHandler);
        eventBrokerComponent.Subscribe<UIEvents.SetCombo>(SetComboHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<UIEvents.SetHealth>(SetHealthHandler);
        eventBrokerComponent.Unsubscribe<UIEvents.SetDash>(SetDashHandler);
        eventBrokerComponent.Unsubscribe<UIEvents.SetOrbs>(SetOrbsHandler);
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

    private void SetOrbsHandler(BrokerEvent<UIEvents.SetOrbs> inEvent)
    {
        for (int i = 0; i < inEvent.Payload.Value; i++)
        {
            orbBoxes[i].enabled = true;
        }
        for (int i = inEvent.Payload.Value; i < orbBoxes.Count; i++)
        {
            orbBoxes[i].enabled = false;
        }
    }

    private void SetDashHandler(BrokerEvent<UIEvents.SetDash> inEvent)
    {
        dashBar.value = inEvent.Payload.Value;
    }

    private void SetHealthHandler(BrokerEvent<UIEvents.SetHealth> inEvent)
    {
        for (int i = 0; i < inEvent.Payload.Value; i++)
        {
            healthBoxes[i].enabled = true;
        }
        for (int i = inEvent.Payload.Value; i < healthBoxes.Count; i++)
        {
            healthBoxes[i].enabled = false;
        }
    }
}
