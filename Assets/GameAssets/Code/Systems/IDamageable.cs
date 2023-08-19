using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDamageable
{
    public void TakeDamage(int value, Action<Transform> isDeadCallback);
}
