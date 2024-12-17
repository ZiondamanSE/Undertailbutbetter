using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public string enemyName = "Default Enemy";
    public int health = 10;

    void Start()
    {
        Debug.Log($"{enemyName} has appeared with {health} HP!");
    }

    // Method to apply damage to the enemy
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{enemyName} took {damage} damage! Remaining HP: {health}");

        // Check if the enemy's health has dropped to zero or below
        if (health <= 0)
        {
            Die();
        }
    }

    // Handle enemy defeat
    void Die()
    {
        Debug.Log($"{enemyName} has been defeated!");
        // Destroy the enemy GameObject
        Destroy(gameObject);
    }
}
