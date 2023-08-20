using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    [SerializeField] private List<EnemyBase> enemies;

    private void Start()
    {
        Disable();
    }

    public void Enable()
    {
        foreach (EnemyBase enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.EnableEnemy();
            }
        }
    }

    public void Disable()
    {
        foreach (EnemyBase enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.DisableEnemy();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Enable();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Disable();
        }
    }
}
