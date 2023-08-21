using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttackRanged : EnemyAttack
{
    [SerializeField] private float projectileSpawnDelay;    // Anticipation


    private List<EnemyAttackPattern> attackPatterns;

    private void Start()
    {
        attackPatterns = GetComponents<EnemyAttackPattern>().ToList();
    }

    public override void Attack()
    {
        base.Attack();

        StartCoroutine(WaitForAnticipation());
    }

    private IEnumerator WaitForAnticipation()
    {
        yield return new WaitForSeconds(projectileSpawnDelay);
        attackPatterns = GetComponents<EnemyAttackPattern>().ToList();
        foreach (EnemyAttackPattern pattern in attackPatterns)
        {
            pattern.StartAttackPattern();
        }
    }
}
