using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations
{
	private Animator anim;

	private bool isDashing;

	public PlayerAnimations(Animator animator)
	{
		anim = animator;
	}

	public void HandleMovementAnim(Vector2 inputAxis)
	{
		if (isDashing) return;

		anim.SetBool(Constants.Player.Animations.IsMoving, inputAxis == Vector2.zero);
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
}
