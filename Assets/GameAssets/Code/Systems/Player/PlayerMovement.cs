using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
	private Rigidbody2D rbody;

	private bool isDashing;
	private bool isDashAttacking;

	private int dashCooldown;

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	public PlayerMovement(Rigidbody2D rigidbody)
	{
		rbody = rigidbody;
		isDashing = false;
		isDashAttacking = false;
		dashCooldown = 0;

		Subscribe();
	}

	~PlayerMovement()
	{
		Unsubscribe();
	}

	private void Subscribe()
	{
		eventBrokerComponent.Subscribe<PlayerAttackEvents.PlayerHitEnemy>(HandlePlayerHitEnemy);
	}

	private void Unsubscribe()
	{
		eventBrokerComponent.Unsubscribe<PlayerAttackEvents.PlayerHitEnemy>(HandlePlayerHitEnemy);
	}

	private void HandlePlayerHitEnemy(BrokerEvent<PlayerAttackEvents.PlayerHitEnemy> inEvent)
	{
		dashCooldown = dashCooldown - 1;
		if (dashCooldown < 0)
		{
			dashCooldown = 0;
		}
		eventBrokerComponent.Publish(this, new UIEvents.SetDash(dashCooldown));
	}

	public void HandleMovement(Vector2 inputAxis, float movespeed)
	{
		if (isDashing || isDashAttacking) return;

		rbody.velocity = new Vector2(inputAxis.x * movespeed, inputAxis.y * movespeed);
	}

	public IEnumerator HandleDash(Vector3 playerPos, float force, float duration)
	{
		if (isDashing) yield return null;

		isDashing = true;
		dashCooldown = Constants.Player.Attacks.DashCooldown;
		eventBrokerComponent.Publish(this, new UIEvents.SetMaxDash(dashCooldown));

		Vector2 prevVelocity = rbody.velocity;

		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0;

		Vector2 direction = (mousePosition - playerPos).normalized;

		rbody.velocity = direction * force;

		yield return new WaitForSeconds(duration);

		rbody.velocity = prevVelocity;

		isDashing = false;
	}

	public IEnumerator HandleDashAttack(Vector3 playerPos, float force, float duration)
	{
		if (isDashAttacking) yield return null;

		isDashAttacking = true;
		dashCooldown = Constants.Player.Attacks.DashAttackCooldown;
		eventBrokerComponent.Publish(this, new UIEvents.SetMaxDash(dashCooldown));

		Vector2 prevVelocity = rbody.velocity;

		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0;

		Vector2 direction = (mousePosition - playerPos).normalized;

		rbody.velocity = direction * force;

		yield return new WaitForSeconds(duration);

		rbody.velocity = prevVelocity;

		Vector3 distance = rbody.transform.position - playerPos;

		int killedWithDash = 0;

		RaycastHit2D[] hits = Physics2D.BoxCastAll(playerPos, new Vector2(1, 1), 0f, direction, distance.magnitude, 1 << LayerMask.NameToLayer(Constants.Enemy.Tag));
		foreach (RaycastHit2D hit in hits)
		{
			IDamageable damageable = hit.transform.GetComponent<IDamageable>();
			if (damageable != null)
			{
				damageable.TakeDamage(1, (obj) => killedWithDash += 1);
			}
		}

		if (killedWithDash >= Constants.Player.Attacks.DashAttackResetCount)
		{
			// Reset dash cooldown
			dashCooldown = 0;
		}

		isDashAttacking = false;
	}
}
