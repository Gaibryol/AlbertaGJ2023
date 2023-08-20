using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentBase
{
    public virtual string Title { get; protected set;}
    public virtual string Description { get; protected set;}

    protected static EnemyMeleeStats EnemyMeleeStats;
    protected static EnemyRangedStats EnemyRangedStats;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public AugmentBase()
    {
        if (EnemyMeleeStats == null)
        {
            EnemyEvents.GetEnemyMeleeStats enemyMeleeStats = new EnemyEvents.GetEnemyMeleeStats();
            eventBrokerComponent.Publish(this, enemyMeleeStats);

            EnemyMeleeStats = enemyMeleeStats.EnemyMeleeStats;
        }

        if (EnemyRangedStats == null)
        {
            EnemyEvents.GetEnemyRangedStats enemyRangedStats = new EnemyEvents.GetEnemyRangedStats();
            eventBrokerComponent.Publish(this, enemyRangedStats);
            EnemyRangedStats = enemyRangedStats.EnemyRangedStats;
        }
    }

    public virtual void Apply()
    {

    }
}
