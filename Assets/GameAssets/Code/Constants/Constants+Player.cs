public partial class Constants
{
	public class Player
	{
		public class Movement
		{
			public const float Movespeed = 10f;
			public const float DashForce = 25f;
			public const float DashDuration = 0.15f;
			public const float DashAttackForce = 25f;
			public const float DashAttackDuration = 0.20f;
		}
		
		public class Animations
		{
			public const string IsDashing = "IsDashing";
			public const string IsDashAttacking = "IsDashAttacking";
			public const string IsMoving = "IsMoving";
			public const string IsAttack1 = "IsAttack1";
			public const string IsAttack2 = "IsAttack2";
			public const string IsAttack3 = "IsAttack3";
			public const string IsSpecialAttack = "IsSpecialAttack";
		}

		public class Attacks
		{
			public const float A1Range = 1f;
			public const float A1Angle = 60f;
			public const float A1AttackWindow = 2f;

			public const float A2Range = 1f;
			public const float A2Angle = 60f;
			public const float A2AttackWindow = 2f;

			public const float A3Range = 1f;
			public const float A3Angle = 60f;

			public const float SpecialRange = 1.5f;
			public const float SpecialAngle = 60f;
			public const float SpecialRecoveryPeriod = 1f;

			public const int DashCooldown = 5;
			public const int DashAttackCooldown = 8;
		}

		public class Cooldowns
		{
			public const int DashCooldown = 5;
			public const int SuperDashCooldown = 8;
		}
	}
}