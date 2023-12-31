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
        if (parent == null)
        {
            parent = GetComponent<EnemyAttack>();
        }
        parent.InRange(out direction);
        if (direction != Vector2.zero)
        {
            spawnedProjectile.transform.right = direction;
            spawnedProjectile.GetComponent<Rigidbody2D>().AddForce(spawnedProjectile.transform.right * projectileSpeed, ForceMode2D.Impulse);
        } else
        {
            Destroy(spawnedProjectile);
        }
        OnAttackPatternFinished.Invoke();
    }
}
