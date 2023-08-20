using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedProjectile : MonoBehaviour
{

    Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (body.velocity == Vector2.zero)
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
