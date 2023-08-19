using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Health
{
    [field: SerializeField] public int value { get; set; }

    public Health(int startingValue)
    {
        value = startingValue;
    }
}
