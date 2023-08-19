using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
	private Animator anim;

	private bool isDashing;
	private bool isDashAttacking;
	private bool canAttack;
	
	private int attackStage;

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void Start()
	{
		anim = GetComponent<Animator>();
		canAttack = true;
		isDashing = false;
		isDashAttacking = false;
		attackStage = 1;
	}

	private void HandleResetPlayerAttackStage(BrokerEvent<PlayerAttackEvents.ResetPlayerAttackStage> inEvent)
	{
		attackStage = 1;
		Debug.Log("reset player attack stage");
	}

	public void HandleMovementAnim(Vector2 inputAxis)
	{
		if (isDashing || isDashAttacking) return;

		anim.SetBool(Constants.Player.Animations.IsMoving, inputAxis != Vector2.zero);
	}

	public IEnumerator HandleDashAnim(float duration)
	{
		if (isDashing) yield return null;

		isDashing = true;

		anim.SetBool(Constants.Player.Animations.IsDashing, true);

		yield return new WaitForSeconds(duration);

		anim.SetBool(Constants.Player.Animations.IsDashing, false);

		isDashing = false;
	}

	public IEnumerator HandleDashAttackAnim(float duration)
	{
		if (isDashAttacking) yield return null;

		isDashAttacking = true;

		anim.SetBool(Constants.Player.Animations.IsDashAttacking, true);

		yield return new WaitForSeconds(duration);

		anim.SetBool(Constants.Player.Animations.IsDashAttacking, false);

		isDashAttacking = false;
	}

	public IEnumerator HandleAttackAnim()
	{
		if (canAttack)
		{
			float duration = 0f;
			canAttack = false;
			switch (attackStage)
			{
				case 1:
					anim.SetBool(Constants.Player.Animations.IsAttack1, true);
					duration = Constants.Player.Attacks.A1RecoveryPeriod;
					attackStage += 1;
					break;

				case 2:
					anim.SetBool(Constants.Player.Animations.IsAttack2, true);
					duration = Constants.Player.Attacks.A2RecoveryPeriod;
					attackStage += 1;
					break;

				case 3:
					anim.SetBool(Constants.Player.Animations.IsAttack3, true);
					duration = Constants.Player.Attacks.A3RecoveryPeriod;
					attackStage = 1;
					break;
			}

			yield return new WaitForSeconds(duration);

			canAttack = true;
			anim.SetBool(Constants.Player.Animations.IsAttack1, false);
			anim.SetBool(Constants.Player.Animations.IsAttack2, false);
			anim.SetBool(Constants.Player.Animations.IsAttack3, false);
		}
	}

	public IEnumerator HandleSpecialAttackAnim()
	{
		if (attackStage == 3)
		{
			anim.SetBool(Constants.Player.Animations.IsSpecialAttack, true);
			canAttack = false;

			yield return new WaitForSeconds(Constants.Player.Attacks.SpecialRecoveryPeriod);

			canAttack = true;
			attackStage = 1;

			anim.SetBool(Constants.Player.Animations.IsSpecialAttack, false);
		}
	}

	private void OnEnable()
	{
		eventBrokerComponent.Subscribe<PlayerAttackEvents.ResetPlayerAttackStage>(HandleResetPlayerAttackStage);
	}

	private void OnDisable()
	{
		eventBrokerComponent.Unsubscribe<PlayerAttackEvents.ResetPlayerAttackStage>(HandleResetPlayerAttackStage);
	}
}
