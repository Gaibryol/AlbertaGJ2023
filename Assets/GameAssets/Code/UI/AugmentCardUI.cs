using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AugmentCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;

    public AugmentBase Augment { get; private set; }

    public void SetAugment(AugmentBase augment)
    {
        Augment = augment;
        title.text = augment.Title;
        description.text = augment.Description;
    }
}
