using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
	private int attackStage;
	private bool canAttack;
	public Coroutine AttackTimerCoroutine;

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void Start()
	{
		attackStage = 1;
		canAttack = true;
		AttackTimerCoroutine = null;
	}

	public void HandleAttack(Transform player)
	{
		if (!canAttack) return;

		eventBrokerComponent.Publish(this, new PlayerAttackEvents.PlayerAttackStateChange(true));

		List<bool> combo = new List<bool>();

		switch(attackStage)
		{
			case 1:
				CheckAttackHit(player, Constants.Player.Attacks.A1Range, Constants.Player.Attacks.A1Angle);
				StartCoroutine(AttackRecoveryPeriod(Constants.Player.Attacks.A1RecoveryPeriod, Constants.Player.Attacks.A1AttackWindow));
				attackStage += 1;
				combo = new List<bool>() { false };
				break;

			case 2:
				CheckAttackHit(player, Constants.Player.Attacks.A2Range, Constants.Player.Attacks.A2Angle);
				StartCoroutine(AttackRecoveryPeriod(Constants.Player.Attacks.A2RecoveryPeriod, Constants.Player.Attacks.A2AttackWindow));
				attackStage += 1;
				combo = new List<bool>() { false, false };
				break;

			case 3:
				CheckAttackHit(player, Constants.Player.Attacks.A3Range, Constants.Player.Attacks.A3Angle);
				StartCoroutine(AttackRecoveryPeriod(Constants.Player.Attacks.A3RecoveryPeriod, 0));
				attackStage = 1;
				combo = new List<bool>() { false, false, false };
				break;
		}

		eventBrokerComponent.Publish(this, new UIEvents.SetCombo(combo));
	}

	public void HandleSpecialAttack(Transform player)
	{
		if (attackStage != 3 || !canAttack) return;

		eventBrokerComponent.Publish(this, new PlayerAttackEvents.PlayerAttackStateChange(true));

		CheckAttackHit(player, Constants.Player.Attacks.SpecialRange, Constants.Player.Attacks.SpecialAngle);
		StartCoroutine(AttackRecoveryPeriod(Constants.Player.Attacks.SpecialRecoveryPeriod, 0));
		attackStage = 1;

		List<bool> combo = new List<bool>() { false, false, true };
		eventBrokerComponent.Publish(this, new UIEvents.SetCombo(combo));
	}

	private void CheckAttackHit(Transform player, float range, float angle)
	{
		Collider2D[] hits = Physics2D.OverlapCircleAll(player.position, range, 1 << 10);
		foreach (Collider2D hit in hits)
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 playerToMousePos = new Vector3(mousePos.x - player.position.x, mousePos.y - player.position.y, 0);

			Vector3 playerToHit = hit.transform.position - player.position;
			float hitAngle = Vector3.Angle(playerToMousePos, playerToHit);
			
			if (hitAngle <= angle)
			{
				// Hit enemy
				IDamageable damageable = hit.gameObject.GetComponent<IDamageable>();
				if (damageable != null)
				{
					damageable.TakeDamage(1, null);
					eventBrokerComponent.Publish(this, new PlayerAttackEvents.PlayerHitEnemy());
				}
			}
		}
	}

	private IEnumerator AttackRecoveryPeriod(float time, float attackWindow)
	{
		canAttack = false;
		yield return new WaitForSeconds(time);
		canAttack = true;
		eventBrokerComponent.Publish(this, new PlayerAttackEvents.PlayerAttackStateChange(false));

		if (AttackTimerCoroutine != null)
		{
			StopCoroutine(AttackTimerCoroutine);
		}

		if (attackWindow != 0)
		{
			AttackTimerCoroutine = StartCoroutine(AttackTimer(attackWindow));
		}
		else
		{
			eventBrokerComponent.Publish(this, new UIEvents.SetCombo(new List<bool>()));
		}
	}

	public IEnumerator AttackTimer(float time)
	{
		yield return new WaitForSeconds(time);
		eventBrokerComponent.Publish(this, new PlayerAttackEvents.ResetPlayerAttackStage());
		eventBrokerComponent.Publish(this, new UIEvents.SetCombo(new List<bool>()));
		attackStage = 1;
	}
}
