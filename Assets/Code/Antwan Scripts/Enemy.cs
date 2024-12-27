using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public string enemyName = "Default Enemy"; // Name of the enemy
    public GameObject enemyPrefab;            // Prefab for the enemy
    public int health = 10;                   // Enemy's health
    public Vector2 damageRange = new Vector2(1, 10); // Min and Max damage
    [HideInInspector] public bool isDead = false;    // Tracks if the enemy is dead
}

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    public List<Enemy> enemies = new List<Enemy>(); // List of enemies
    public Transform spawnPoint;                   // Spawn point for enemies in the Canvas

    void Start()
    {
        SpawnEnemies();
    }

    // Method to spawn all enemies
    private void SpawnEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.enemyPrefab != null && spawnPoint != null)
            {
                GameObject spawnedEnemy = Instantiate(enemy.enemyPrefab, spawnPoint);
                EnemyScript enemyScript = spawnedEnemy.GetComponent<EnemyScript>();

                if (enemyScript != null)
                {
                    enemyScript.Initialize(enemy);
                }
                else
                {
                    Debug.LogWarning($"Enemy prefab {enemy.enemyPrefab.name} does not have an EnemyScript component.");
                }
            }
        }
    }
}

public class EnemyScript : MonoBehaviour
{
    private Enemy enemyData;
    public string curentname;

    public void Initialize(Enemy enemy)
    {
        enemyData = enemy;
        curentname = enemy.enemyName;
        gameObject.name = enemy.enemyName;
        Debug.Log($"{enemy.enemyName} has appeared with {enemy.health} HP!");
    }

    // Method to apply damage to the enemy
    public void TakeDamage(int damage)
    {
        if (enemyData.isDead)
            return;

        enemyData.health -= damage;
        Debug.Log($"{enemyData.enemyName} took {damage} damage! Remaining HP: {enemyData.health}");

        // Check if the enemy's health has dropped to zero or below
        if (enemyData.health <= 0)
        {
            Die();
        }
    }

    // Handle enemy defeat
    private void Die()
    {
        enemyData.isDead = true;
        Debug.Log($"{enemyData.enemyName} has been defeated!");
        Destroy(gameObject);
    }
}
