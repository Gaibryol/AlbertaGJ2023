using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
	private Rigidbody2D rbody;
	private Collider2D gapColl;

	private bool isDashing;
	private bool isDashAttacking;

	private bool isAttacking;

	private int dashCooldown;
	private int maxDashCooldown;

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	public PlayerMovement(Rigidbody2D rigidbody, Collider2D gapCollider)
	{
		rbody = rigidbody;
		gapColl = gapCollider;
		isDashing = false;
		isDashAttacking = false;
		dashCooldown = PlayerStats.DashCooldown;
		maxDashCooldown = PlayerStats.DashCooldown;

		Subscribe();
	}

	~PlayerMovement()
	{
		Unsubscribe();
	}

	private void Subscribe()
	{
		eventBrokerComponent.Subscribe<PlayerAttackEvents.PlayerHitEnemy>(HandlePlayerHitEnemy);
		eventBrokerComponent.Subscribe<PlayerAttackEvents.PlayerAttackStateChange>(HandlePlayerAttackStateChange);
	}

	private void Unsubscribe()
	{
		eventBrokerComponent.Unsubscribe<PlayerAttackEvents.PlayerHitEnemy>(HandlePlayerHitEnemy);
		eventBrokerComponent.Unsubscribe<PlayerAttackEvents.PlayerAttackStateChange>(HandlePlayerAttackStateChange);
	}

	private void HandlePlayerAttackStateChange(BrokerEvent<PlayerAttackEvents.PlayerAttackStateChange> inEvent)
	{
		isAttacking = inEvent.Payload.IsAttacking;
	}

	private void HandlePlayerHitEnemy(BrokerEvent<PlayerAttackEvents.PlayerHitEnemy> inEvent)
	{
		dashCooldown = dashCooldown + PlayerStats.DashRegenPerHit + (inEvent.Payload.SpecialHit ? PlayerStats.RightClickDashReduceCooldownAmount : 0);
		if (dashCooldown > maxDashCooldown)
		{
			dashCooldown = maxDashCooldown;
		}
		eventBrokerComponent.Publish(this, new UIEvents.SetDash(dashCooldown));
	}

	public void HandleMovement(Vector2 inputAxis, float movespeed)
	{
		if (isAttacking)
		{
			rbody.velocity = Vector2.zero;
			return;
		}
		else if (isDashing || isDashAttacking)
		{
			return;
		}

		rbody.velocity = new Vector2(inputAxis.x * movespeed, inputAxis.y * movespeed);
	}

	public IEnumerator HandleDash(Vector3 playerPos, float force, float duration, Vector2 direction)
	{
		if (!isDashing && dashCooldown >= maxDashCooldown)
		{
			isDashing = true;
			maxDashCooldown = Constants.Player.Attacks.DashCooldown;
			dashCooldown = 0;
			eventBrokerComponent.Publish(this, new UIEvents.SetDash(dashCooldown));
			eventBrokerComponent.Publish(this, new UIEvents.SetMaxDash(maxDashCooldown));

			gapColl.isTrigger = true;

			Vector2 prevVelocity = rbody.velocity;

			rbody.velocity = direction * force;

			yield return new WaitForSeconds(duration);

			rbody.velocity = prevVelocity;

			gapColl.isTrigger = false;

			isDashing = false;
		}
	}

	public IEnumerator HandleDashAttack(Vector3 playerPos, float force, float duration, Vector2 direction)
	{
		if (!isDashAttacking && dashCooldown >= maxDashCooldown)
		{
			isDashAttacking = true;
			maxDashCooldown = Constants.Player.Attacks.DashAttackCooldown;
			dashCooldown = 0;
			eventBrokerComponent.Publish(this, new UIEvents.SetDash(dashCooldown));
			eventBrokerComponent.Publish(this, new UIEvents.SetMaxDash(maxDashCooldown));

			gapColl.isTrigger = true;

			Vector2 prevVelocity = rbody.velocity;

			rbody.velocity = direction * force;

			yield return new WaitForSeconds(duration);

			rbody.velocity = prevVelocity;

			Vector3 distance = rbody.transform.position - playerPos;

			int killedWithDash = 0;

			RaycastHit2D[] hits = Physics2D.BoxCastAll(playerPos, PlayerStats.DashAttackHitBox, 0f, direction, distance.magnitude, 1 << LayerMask.NameToLayer(Constants.Enemy.Tag));
			foreach (RaycastHit2D hit in hits)
			{
				IDamageable damageable = hit.transform.GetComponent<IDamageable>();
				if (damageable != null)
				{
					damageable.TakeDamage(1, (obj) => killedWithDash += 1);
				}
			}

			if (hits.Length >= 3)
			{
				eventBrokerComponent.Publish(this, new HealthEvents.IncreasePlayerHealth(PlayerStats.DashAttackHealAmount));
			}

			if (killedWithDash >= PlayerStats.DashAttackResetCount && PlayerStats.DashCanReset)
			{
				// Reset dash cooldown
				dashCooldown = maxDashCooldown;
				eventBrokerComponent.Publish(this, new UIEvents.SetDash(dashCooldown));
				eventBrokerComponent.Publish(this, new UIEvents.SetMaxDash(maxDashCooldown));
			}

			gapColl.isTrigger = false;

			isDashAttacking = false;
		}
	}
}
