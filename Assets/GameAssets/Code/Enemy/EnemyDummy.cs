using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDummy : MonoBehaviour, IDamageable
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(int value, Action<Transform> isDeadCallback)
    {
        animator.SetTrigger("Hit");
    }

}
