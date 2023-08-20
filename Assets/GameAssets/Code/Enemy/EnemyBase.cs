using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(Animator))]
public class EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField] protected EnemyStatsBase EnemyStats;

    protected Animator animator;
    protected EnemyMovement movement;
    protected EnemyAttack attack;
    protected Health health;
    protected EnemyHealthBarUI healthBarUI;

    protected EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	public Action<GameObject> onDeathCallback;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyAttack>();
        healthBarUI = GetComponent<EnemyHealthBarUI>();
        health = new Health(EnemyStats.MaxHealth);
    }

    protected virtual void Start()
    {
        movement.SetMovementSpeed(EnemyStats.MoveSpeed);
        attack.SetAttackRange(EnemyStats.AttackRange);
        attack.SetAttackSpeed(EnemyStats.AttackSpeed);
    }

    public virtual void EnableEnemy()
    {
        attack.enabled = true;
    }

    public virtual void DisableEnemy()
    {
        attack.enabled = false;
    }

    protected virtual void Update()
    {
        if (attack.CanAttack())
        {
            attack.Attack();
            SetAttackAnimationVariable();
        }
        movement.SetCanMove(!attack.InRange() && !attack.IsAttacking());
        Vector2 movementVelocity = movement.Velocity();
        SetMovementAnimationVariables(movementVelocity);
        transform.localScale = new Vector3(Mathf.Sign(movementVelocity.x), 1, 1);
    }

    private void SetMovementAnimationVariables(Vector2 velocity)
    {
        if (velocity != Vector2.zero)
        {
            animator.SetFloat(Constants.Enemy.Animations.Horizontal, velocity.x);
            animator.SetFloat(Constants.Enemy.Animations.Vertical, velocity.y);
        }
        animator.SetBool(Constants.Enemy.Animations.IsMoving, velocity.magnitude > 0);
    }

    private void SetAttackAnimationVariable()
    {
        animator.SetTrigger(Constants.Enemy.Animations.Attack);
    }

    public void TakeDamage(int value, Action<Transform> isDeadCallback)
    {
        health.Value -= value;
        healthBarUI.SetHealth((float)health.Value / EnemyStats.MaxHealth);
        animator.SetTrigger(Constants.Enemy.Animations.Hurt);
        attack.ResetAttackTimer();
        if (health.Value <= 0)
        {
            // Destroy for now, later prob death animation
            Destroy(healthBarUI.gameObject);
            Destroy(gameObject);
			isDeadCallback?.DynamicInvoke(transform);
            eventBrokerComponent.Publish(this, new EnemyEvents.EnemyDeath());
			onDeathCallback.DynamicInvoke(gameObject);
        }
    }
}
