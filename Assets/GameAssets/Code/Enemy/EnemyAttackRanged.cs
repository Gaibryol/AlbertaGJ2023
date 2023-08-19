using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRanged : EnemyAttack
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawnPosition;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileSpawnDelay;

    public override void Attack()
    {
        base.Attack();
        StartCoroutine(SpawnProjectile());
    }

    private IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(projectileSpawnDelay);
        GameObject spawnedProjectile = Instantiate(projectile, projectileSpawnPosition.position, Quaternion.identity);
        // Implementation detail: Calculate position before or after delay
        Vector2 direction;
        InRange(out direction);
        spawnedProjectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
    }
}
