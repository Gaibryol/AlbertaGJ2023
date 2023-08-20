using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentBase
{
    public virtual string Title { get; protected set;}
    public virtual string Description { get; protected set;}

    [SerializeField] protected EnemyMeleeStats EnemyMeleeStats;
    [SerializeField] protected EnemyRangedStats EnemyRangedStats;
    public virtual void Apply()
    {

    }
}
