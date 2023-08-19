using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
	private Rigidbody2D rbody;

	private bool isDashing;

	public PlayerMovement(Rigidbody2D rigidbody)
	{
		rbody = rigidbody;
		isDashing = false;
	}

	public void HandleMovement(Vector2 inputAxis, float movespeed)
	{
		if (isDashing) return;

		rbody.velocity = new Vector2(inputAxis.x * movespeed, inputAxis.y * movespeed);
	}

	public IEnumerator HandleDash(Vector3 playerPos, float force, float duration)
	{
		if (isDashing) yield return null;

		isDashing = true;

		Vector2 prevVelocity = rbody.velocity;

		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0;

		Vector2 direction = (mousePosition - playerPos).normalized;

		rbody.velocity = direction * force;

		yield return new WaitForSeconds(duration);

		rbody.velocity = prevVelocity;

		isDashing = false;
	}
}