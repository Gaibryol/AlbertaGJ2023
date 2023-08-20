using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(AIPath))]
public class EnemyMovement : MonoBehaviour
{
    protected AIPath aiPath;

    protected virtual void Awake()
    {
        aiPath = GetComponent<AIPath>();
    }

    public void SetMovementSpeed(float value)
    {
        aiPath.maxSpeed = value;
    }

    public Vector2 Velocity()
    {
        return aiPath.velocity;
    }

    public void SetCanMove(bool canMove)
    {
        aiPath.canMove = canMove;
    }
}
