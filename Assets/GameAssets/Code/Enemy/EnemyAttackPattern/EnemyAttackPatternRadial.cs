using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPatternRadial : EnemyAttackPattern
{
    [SerializeField] private int numberOfProjectiles;

    [SerializeField] public GameObject projectile;
    [SerializeField] public Transform projectileSpawnPosition;
    [SerializeField] public float projectileSpeed;

    private float spawnAngle;

    protected override void Start()
    {
        base.Start();
        spawnAngle = 2 * Mathf.PI / numberOfProjectiles;
    }

    public override void StartAttackPattern()
    {
        base.StartAttackPattern();
        for (float angle = 0; angle < Mathf.PI * 2; angle += spawnAngle)
        {
            GameObject spawnedProjectile = Instantiate(projectile, projectileSpawnPosition.position, Quaternion.identity);
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            if (direction != Vector2.zero)
            {
                spawnedProjectile.transform.right = direction;
                spawnedProjectile.GetComponent<Rigidbody2D>().AddForce(spawnedProjectile.transform.right * projectileSpeed, ForceMode2D.Impulse);
            } else
            {
                Destroy(spawnedProjectile);
            }
        }
        OnAttackPatternFinished.Invoke();
    }
}
