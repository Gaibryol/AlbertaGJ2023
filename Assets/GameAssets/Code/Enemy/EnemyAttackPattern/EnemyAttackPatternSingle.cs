using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPatternSingle : EnemyAttackPattern
{
    [SerializeField] public GameObject projectile;
    [SerializeField] public Transform projectileSpawnPosition;
    [SerializeField] public float projectileSpeed;

    public override void StartAttackPattern()
    {
        base.StartAttackPattern();

        GameObject spawnedProjectile = Instantiate(projectile, projectileSpawnPosition.position, Quaternion.identity);
        Vector2 direction;
        parent.InRange(out direction);
        if (direction != Vector2.zero)
        {
            spawnedProjectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
        }
        OnAttackPatternFinished.Invoke();
    }
}
