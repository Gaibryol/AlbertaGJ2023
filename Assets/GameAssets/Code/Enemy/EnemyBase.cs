using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(Animator))]
public class EnemyBase : MonoBehaviour
{
    protected Animator animator;
    protected EnemyMovement movement;
    protected EnemyAttack attack;
    protected Health health;

    [SerializeField] private int startingHealth;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyAttack>();
        health = new Health(startingHealth);
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
            animator.SetFloat("Horizontal", velocity.x);
            animator.SetFloat("Vertical", velocity.y);
        }
        animator.SetBool("IsMoving", velocity.magnitude > 0);
    }

    private void SetAttackAnimationVariable()
    {
        animator.SetTrigger("Attack");
    }
}
