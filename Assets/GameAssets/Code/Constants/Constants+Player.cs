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
		}

		public class Cooldowns
		{
			public const int DashCooldown = 5;
			public const int SuperDashCooldown = 8;
		}
	}
}