using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WaveSystem : MonoBehaviour
{
	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private int currentWave;

	[SerializeField] private List<GameObject> basicEnemies;
	[SerializeField] private List<Transform> spawnPositions;
	[SerializeField] private List<int> numEnemiesPerWave;

	[SerializeField] private GameObject projectile;

	private List<GameObject> enemies;

	// Start is called before the first frame update
	private void Start()
	{
		currentWave = 1;
		enemies = new List<GameObject>();

		StartCoroutine(NextWave(WaveStats.TimeUntilNextWave));
	}

	private void EnemyDeath(GameObject e)
	{
		enemies.Remove(e);

		if (enemies.Count == 0)
		{
			// Next wave
			StartCoroutine(NextWave(WaveStats.TimeUntilNextWave));
		}
	}

	private IEnumerator NextWave(float timer)
	{
		yield return new WaitForSeconds(timer);
		SpawnWave();
	}

	private void SpawnWave()
	{
		for (int i = 0; i < numEnemiesPerWave[currentWave - 1] + WaveStats.ExtraEnemyPerWave; i++)
		{
			int enemyChoice = Random.Range(0, 9);
			GameObject enemy = null;

			if (enemyChoice < WaveStats.SpawnChance)
			{
				// Ranged Enemy
				enemy = Instantiate(basicEnemies[0]);

				if (WaveStats.ChanceForRadialRanged != 0)
				{
					int isRadial = Random.Range(0, 10);
					if (isRadial < WaveStats.ChanceForRadialRanged)
					{
						// Set ranged enemy to radial attacks
						Destroy(enemy.GetComponent<EnemyAttackPatternSingle>());
						EnemyAttackPatternRadial attackScript = enemy.AddComponent<EnemyAttackPatternRadial>();
						attackScript.projectile = projectile;
						attackScript.projectileSpawnPosition = enemy.transform;
						attackScript.projectileSpeed = WaveStats.EnemyProjectileSpeed;
					}
					else
					{
						EnemyAttackPatternSingle attackScript = enemy.GetComponent<EnemyAttackPatternSingle>();
						attackScript.projectileSpeed = WaveStats.EnemyProjectileSpeed;
					}
				}
			}
			else if (enemyChoice >= WaveStats.SpawnChance)
			{
				// Melee Enemy
				enemy = Instantiate(basicEnemies[1]);
			}

			int spawnChoice = Random.Range(0, spawnPositions.Count);
			Vector3 pos = new Vector3(spawnPositions[spawnChoice].position.x + Random.Range(-Constants.Enemy.SpawnOffset, Constants.Enemy.SpawnOffset), spawnPositions[spawnChoice].position.y + Random.Range(-Constants.Enemy.SpawnOffset, Constants.Enemy.SpawnOffset), 0);
			enemy.transform.position = pos;

			enemy.GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag(Constants.Player.Tag).transform;
			enemy.GetComponent<EnemyBase>().onDeathCallback = EnemyDeath;
			enemies.Add(enemy);
		}
	}
}

public static class WaveStats
{
	public static int ExtraEnemyPerWave = 0;
	public static int ChanceForRadialRanged = 0;
	public static float EnemyProjectileSpeed = 5f;
	public static float TimeUntilNextWave = 5f;
	public static int SpawnChance = 5;
}