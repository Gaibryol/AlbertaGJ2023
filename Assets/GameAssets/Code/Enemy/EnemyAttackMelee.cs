using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackMelee : EnemyAttack
{

    public override void Attack()
    {
        base.Attack();
        CheckAttackHit(1.5f, 60f);
    }

    private void CheckAttackHit(float range, float angle)
    {
        Vector2 direction;
        InRange(out direction);
        Collider2D hit = Physics2D.OverlapCircle(transform.position, range, 1 << 3);
        if (hit == null) return;
        Vector3 playerToHit = new Vector3(hit.transform.position.x - transform.position.x, hit.transform.position.y - transform.position.y, 0f).normalized;
        float hitAngle = Vector3.Angle(direction, playerToHit);

        if (hitAngle <= angle)
        {
            // Hit enemy
            PlayerController pC = hit.gameObject.GetComponent<PlayerController>();
            if (pC != null)
            {
                pC.TakeDamage(PlayerStats.DamageTakenFromHit);
            }
        }
    }
}
