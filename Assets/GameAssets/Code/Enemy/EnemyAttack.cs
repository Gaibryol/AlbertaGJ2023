using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackDuration;

    private float attackSpeedTimer;
    private float attackDurationTimer;

    private void Update()
    {
        // Don't need to go below zero
        attackSpeedTimer -= attackSpeedTimer < 0 ? 0 : Time.deltaTime;
        attackDurationTimer -= attackDurationTimer < 0 ? 0 : Time.deltaTime;
    }

    public virtual void Attack()
    {
        ResetAttackTimer();
    }

    public void ResetAttackTimer()
    {
        attackSpeedTimer = attackSpeed;
        attackDurationTimer = attackDuration;
    }

    public virtual bool IsAttacking()
    {
        return attackDurationTimer > 0;
    }

    public virtual bool CanAttack()
    {
        return attackSpeedTimer < 0 && !IsAttacking() && InRange();
    }

    public virtual bool InRange(out Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, attackRange, Vector2.zero, 0f, 1 << 3);
        if (hit.collider == null)
        {
            direction = Vector2.zero;
        } else
        {
            direction = (hit.collider.transform.position -transform.position).normalized;
        }
        return hit.collider != null;
    }

    public virtual bool InRange()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, attackRange, Vector2.zero, 0f, 1 << 3);
        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
