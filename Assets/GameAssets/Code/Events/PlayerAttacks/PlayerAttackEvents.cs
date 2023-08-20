using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAttackEvents
{
	public class ResetPlayerAttackStage
	{
		public ResetPlayerAttackStage() { }
	}

	public class PlayerAttackStateChange
	{
		public PlayerAttackStateChange(bool isAttacking)
		{
			IsAttacking = isAttacking;
		}

		public readonly bool IsAttacking;
	}

	public class PlayerHitEnemy
	{
		public readonly bool SpecialHit;
		public PlayerHitEnemy(bool specialHit = false) 
		{
			SpecialHit = specialHit;
		}
	}
}
