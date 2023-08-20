using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    #region Damage Numbers
    public static int NormalAttackDamageFirst = 1;
    public static int NormalAttackDamageSecond = 1;
    public static int NormalAttackDamageThird = 1;
    public static int SpecialAttackDamage = 2;
    public static int DashAttackDamage = 1;
    
    public static int DamageTakenFromHit = 1;
    #endregion

    #region Dash
    public static int DashCooldown = 5;
    public static int DashAttackCooldown = 8;
    public static int DashRegenPerHit = 1;
    public static bool DashCanReset = true;
    public static int DashAttackHealAmount = 0;
    #endregion

    #region Movement
    public static float Movespeed = 10f;
    #endregion

    public static int MaxHealth = 3;

    public static float A3RecoveryPeriod = 0.75f;
    public static float A2RecoveryPeriod = 0.5f;
    public static float A1RecoveryPeriod = 0.50f;
    public static int SpecialAttackHealAmount = 0;

    public static int DashAttackResetCount = 3;
    public static int RightClickDashReduceCooldownAmount = 0;

    public static Vector2 DashAttackHitBox = new Vector2(1, 1);

}
