using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AugmentUI : MonoBehaviour
{
    [SerializeField] private AugmentCardUI augmentCard1;
    [SerializeField] private AugmentCardUI augmentCard2;
    [SerializeField] private Button confirmButton;

    private AugmentCardUI selectedCard;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void SetAugmentCards(AugmentBase augment1, AugmentBase augment2)
    {
        augmentCard1.SetAugment(augment1);
        augmentCard2.SetAugment(augment2);
    }

    public void SelectAugment(AugmentCardUI augmentCard)
    {
        selectedCard = augmentCard;
        confirmButton.interactable = true;
        Debug.Log("selected " + augmentCard);
    }

    public void Confirm()
    {
        eventBrokerComponent.Publish(this, new AugmentEvents.SelectAugment(selectedCard.Augment));
        confirmButton.interactable = false;
    }
}
