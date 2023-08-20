using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Augments
{
    public class Augment_1 : AugmentBase
    {
        /*
         Augment 1 (Tank):
            Player health increased by 2
            Dash and Dash Attack cooldown increased by 2
         */

        public override string Title { get => "Augment 1"; }
        public override string Description { get => "Player health increased by 2\r\nDash and Dash Attack cooldown increased by 2"; }
        public override void Apply()
        {
            base.Apply();
            PlayerStats.MaxHealth += 2;
            PlayerStats.DashCooldown += 2;
            PlayerStats.DashAttackCooldown += 2;
        }
    }

    public class Augment_2 : AugmentBase
    {
        /*
         Augment 2 (DPS):
            Player normal attack speed increased by 0.15
            Dash Attack cooldown increased by 2

         */
        public override string Title { get => "Augment 2"; }
        public override string Description { get => "Player normal attack speed increased by 0.15\r\nDash Attack cooldown increased by 2"; }
        public override void Apply()
        {
            base.Apply();
            PlayerStats.A1RecoveryPeriod -= .15f;
            PlayerStats.A2RecoveryPeriod -= .15f;
            PlayerStats.A3RecoveryPeriod -= .15f;
            PlayerStats.DashAttackCooldown += 2;
        }
    }

    public class Augment_3 : AugmentBase
    {
        /*
         Augment 3 (Assassin):
            Number of enemies for Dash Attack reset decreased by 1
            Player max health decreased by 1


         */
        public override string Title { get => "Augment 3"; }
        public override string Description { get => "Number of enemies for Dash Attack reset decreased by 1\r\nPlayer max health decreased by 1"; }
        public override void Apply()
        {
            base.Apply();
            PlayerStats.DashAttackResetCount -= 1;
            PlayerStats.MaxHealth = Mathf.Clamp(PlayerStats.MaxHealth - 1, 1, PlayerStats.MaxHealth);
        }
    }

    public class Augment_4 : AugmentBase
    {
        /*
         Augment 4 (Assassin):
            Player right click reduces Dash cooldown by 2
            Ranged enemies attack speed increased by 1
            Ranged enemies attack range increased by 1
         */
        public override string Title { get => "Augment 4"; }
        public override string Description { get => "Player right click reduces Dash cooldown by 2\r\nRanged enemies attack speed increased by 1\r\nRanged enemies attack range increased by 1"; }
        public override void Apply()
        {
            base.Apply();
            PlayerStats.RightClickDashReduceCooldownAmount += 2;
            EnemyRangedStats.AttackRange += 1;
            EnemyRangedStats.AttackSpeed += 1;
        }
    }

    public class Augment_5 : AugmentBase
    {
        /*
         Augment 5 (Tank):
            Player normal attack damage increased by 1
            Player movespeed decreased by 1
         */
        public override string Title { get => "Augment 5"; }
        public override string Description { get => "Player normal attack damage increased by 1\r\nPlayer movespeed decreased by 1"; }
        public override void Apply()
        {
            base.Apply();
            PlayerStats.NormalAttackDamageFirst += 1;
            PlayerStats.NormalAttackDamageSecond += 1;
            PlayerStats.NormalAttackDamageThird += 1;
            PlayerStats.Movespeed -= 1;
        }
    }

    public class Augment_6 : AugmentBase
    {
        /*
         Augment 6 (DPS):
            Player movespeed increased by 2
            Dash Attack hitbox width decreased by 0.15
            Dash cooldown increased by 1
         */
        public override string Title { get => "Augment 6"; }
        public override string Description { get => "Player movespeed increased by 2\r\nDash Attack hitbox width decreased by 0.15\r\nDash cooldown increased by 1"; }
        public override void Apply()
        {
            base.Apply();
            PlayerStats.Movespeed += 2;
            PlayerStats.DashCooldown += 1;
            PlayerStats.DashAttackHitBox.x -= 0.15f;
        }
    }

    public class Augment_7 : AugmentBase
    {
        /*
         Augment 7 (Tank):
            Third normal attack and special attack damage increased by 2
            Dash Attack no longer refreshes
         */
        public override string Title { get => "Augment 7"; }
        public override string Description { get => "Third normal attack and special attack damage increased by 2\r\nDash Attack no longer refreshes"; }
        public override void Apply()
        {
            base.Apply();
            PlayerStats.NormalAttackDamageThird += 2;
            PlayerStats.SpecialAttackDamage += 2;
            PlayerStats.DashCanReset = false;
        }
    }

    public class Augment_8 : AugmentBase
    {
        /*
         Augment 8 (Assassin):
            Dash Attack damage increased by 1
            Third normal attack and special attack damage decreased by 1
         */
        public override string Title { get => "Augment 8"; }
        public override string Description { get => "Dash Attack damage increased by 1\r\nThird normal attack and special attack damage decreased by 1"; }
        public override void Apply()
        {
            base.Apply();
            PlayerStats.DashAttackDamage += 1;
            PlayerStats.NormalAttackDamageThird -= 1;
            PlayerStats.SpecialAttackDamage -= 1;
        }
    }

    public class Augment_9 : AugmentBase
    {
        /*
         Augment 9 (DPS):
            Hitting 3 enemies with a Dash Attack heals 1 health
            Dash Attack cooldown increased by 1
            Dash Attack no longer resets

         */
        public override string Title { get => "Augment 9"; }
        public override string Description { get => "Hitting 3 enemies with a Dash Attack heals 1 health\r\nDash Attack cooldown increased by 1\r\nDash Attack no longer resets"; }
        public override void Apply()
        {
            base.Apply();
            PlayerStats.DashAttackHealAmount += 1;
            PlayerStats.DashAttackCooldown += 1;
            PlayerStats.DashCanReset = false;
        }
    }

    public class Augment_10 : AugmentBase
    {
        /*
         Augment 10 (Tank):
            Hitting 3 enemies with a Special Attack heals 1 health
            All enemies damage increased by 1
         */
        public override string Title { get => "Augment 10"; }
        public override string Description { get => "Hitting 3 enemies with a Special Attack heals 1 health\r\nAll enemies damage increased by 1"; }
        public override void Apply()
        {
            base.Apply();
            PlayerStats.SpecialAttackHealAmount += 1;
            PlayerStats.DamageTakenFromHit += 1;
        }
    }

    public class Augment_11 : AugmentBase
    {
        /*
         Augment 11 (Assassin):
            Dash Attack hitbox width increased by 0.15
            Dash Attack cooldown increased by 2
         */
        public override string Title { get => "Augment 11"; }
        public override string Description { get => "Dash Attack hitbox width increased by 0.15\r\nDash Attack cooldown increased by 2"; }
        public override void Apply()
        {
            base.Apply();
            PlayerStats.DashAttackHitBox.x += 0.15f;
            PlayerStats.DashAttackCooldown += 2;
        }
    }

    public class Augment_12 : AugmentBase
    {
        /*
         Augment 12 (DPS):
            All attack damage increased by 1
            Movespeed increased by 1
            Player max health set to 1
         */
        public override string Title { get => "Augment 12"; }
        public override string Description { get => "All attack damage increased by 1\r\nMovespeed increased by 1\r\nPlayer max health set to 1"; }
        public override void Apply()
        {
            base.Apply();
            PlayerStats.NormalAttackDamageFirst += 1;
            PlayerStats.NormalAttackDamageSecond += 1;
            PlayerStats.NormalAttackDamageThird += 1;
            PlayerStats.SpecialAttackDamage += 1;
            PlayerStats.Movespeed += 1;
            PlayerStats.MaxHealth = 1;
        }
    }

	public class Augment_13 : AugmentBase
	{
		/*
         Augment 13:
            Dash cooldown decreased by 2
			Ranged enemies have a 50% chance of spawning with a radial attack pattern
         */
		public override string Title { get => "Augment 13"; }
		public override string Description { get => "Dash cooldown decreased by 2\r\nRanged enemies have a 50% chance of spawning with a radial attack pattern"; }
		public override void Apply()
		{
			base.Apply();
			WaveStats.ChanceForRadialRanged = 5;
			PlayerStats.DashCooldown -= 2;
		}
	}

	public class Augment_14 : AugmentBase
	{
		/*
         Augment 14:
			Enemy projectile speed reduced by 1
			All waves spawn with 1 extra enemy
         */
		public override string Title { get => "Augment 14"; }
		public override string Description { get => "Enemy projectile speed reduced by 1\r\nAll waves spawn with 1 extra enemy"; }
		public override void Apply()
		{
			base.Apply();
			WaveStats.ExtraEnemyPerWave -= 1;
			WaveStats.EnemyProjectileSpeed -= 1;
		}
	}

	public class Augment_15 : AugmentBase
	{
		/*
		 Augment 15:
			Chance to spawn melee enemy decreased by 20%
			Chance to spawn ranged enemy increased by 20%
			Movespeed increased by 1
			Player takes 1 extra damage from all sources
         */
		public override string Title { get => "Augment 15"; }
		public override string Description { get => "Chance to spawn melee enemy decreased by 20%\r\nChance to spawn ranged enemy increased by 20%\r\nMovespeed increased by 1\r\nPlayer takes 1 extra damage from all sources"; }
		public override void Apply()
		{
			base.Apply();
			WaveStats.SpawnChance += 2;
			PlayerStats.Movespeed += 1;
			PlayerStats.DamageTakenFromHit += 1;
		}
	}

	public static List<AugmentBase> All()
    {
        List<AugmentBase> augments = new List<AugmentBase>() 
        { 
            new Augment_1(), new Augment_2(), new Augment_3(), new Augment_4(), 
            new Augment_5(), new Augment_6(), new Augment_7(), new Augment_8(), 
            new Augment_9(), new Augment_10(), new Augment_11(), new Augment_12(),
			new Augment_13(), new Augment_14(), new Augment_15(),
        };
        return augments;
    }
}
