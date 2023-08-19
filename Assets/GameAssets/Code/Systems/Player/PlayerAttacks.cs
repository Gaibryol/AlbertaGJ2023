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

		switch(attackStage)
		{
			case 1:
				Debug.Log("attack 1");
				CheckAttackHit(player, Constants.Player.Attacks.A1Range, Constants.Player.Attacks.A1Angle);
				StartCoroutine(AttackRecoveryPeriod(Constants.Player.Attacks.A1RecoveryPeriod, Constants.Player.Attacks.A1AttackWindow));
				attackStage += 1;
				break;

			case 2:
				Debug.Log("attack 2");
				CheckAttackHit(player, Constants.Player.Attacks.A2Range, Constants.Player.Attacks.A2Angle);
				StartCoroutine(AttackRecoveryPeriod(Constants.Player.Attacks.A2RecoveryPeriod, Constants.Player.Attacks.A2AttackWindow));
				attackStage += 1;
				break;

			case 3:
				Debug.Log("attack 3");
				CheckAttackHit(player, Constants.Player.Attacks.A3Range, Constants.Player.Attacks.A3Angle);
				StartCoroutine(AttackRecoveryPeriod(Constants.Player.Attacks.A3RecoveryPeriod, 0));
				attackStage = 1;
				break;
		}
	}

	public void HandleSpecialAttack(Transform player)
	{
		if (attackStage != 3 || !canAttack) return;

		Debug.Log("special attack");
		CheckAttackHit(player, Constants.Player.Attacks.SpecialRange, Constants.Player.Attacks.SpecialAngle);
		StartCoroutine(AttackRecoveryPeriod(Constants.Player.Attacks.SpecialRecoveryPeriod, 0));
		attackStage = 1;
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
				Debug.Log("hit");
				IDamageable damageable = hit.gameObject.GetComponent<IDamageable>();
				if (damageable != null)
				{
					damageable.TakeDamage(1);
				}
			}
		}
	}

	private IEnumerator AttackRecoveryPeriod(float time, float attackWindow)
	{
		canAttack = false;
		yield return new WaitForSeconds(time);
		canAttack = true;

		if (AttackTimerCoroutine != null)
		{
			StopCoroutine(AttackTimerCoroutine);
		}

		if (attackWindow != 0)
		{
			AttackTimerCoroutine = StartCoroutine(AttackTimer(attackWindow));
		}
	}

	public IEnumerator AttackTimer(float time)
	{
		yield return new WaitForSeconds(time);
		eventBrokerComponent.Publish(this, new PlayerAttackEvents.ResetPlayerAttackStage());
		attackStage = 1;
	}
}
