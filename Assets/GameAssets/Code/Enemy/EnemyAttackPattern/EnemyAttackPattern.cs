using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttackPattern : MonoBehaviour
{
    protected EnemyAttack parent;

    public UnityEvent OnAttackPatternFinished;

    protected virtual void Start () { 
        parent = GetComponent<EnemyAttack>();
    }

    public virtual void StartAttackPattern()
    {

    }
}
