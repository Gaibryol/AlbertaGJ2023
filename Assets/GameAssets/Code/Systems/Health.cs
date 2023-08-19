using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Health
{   
    private int startingHealth;
    [field: SerializeField] public int Value { get { return _value; } set { _value = Mathf.Clamp(value, 0, startingHealth); } }
    private int _value;

    public Health(int startingValue)
    {
        startingHealth = startingValue;
        Value = startingValue;
    }
}
