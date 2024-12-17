using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;       // Maximum health of the enemy
    private int currentHealth;        // Current health of the enemy

    void Start()
    {
        currentHealth = maxHealth;    // Initialize health
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage! Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Handle enemy defeat
    void Die()
    {
        Debug.Log("Enemy defeated!");
        gameObject.SetActive(false);  // Deactivate the enemy for now (you can add more defeat logic)
    }
}
