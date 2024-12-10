using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterSystemScript : MonoBehaviour
{
    [SerializeField] private PlayerMovementScript pm;
    
    public GameObject battleUI;       // The Battle UI GameObject
    public GameObject enemyPrefab;    // The Enemy prefab to spawn
    public Transform enemySpawnPoint; // The spawn point for the enemy

    private bool isInBattle = false;  // Flag to prevent multiple encounters at once

    void Awake()
    {
        battleUI.SetActive(false); // Make sure battle UI is hidden initially
        if (pm == null)
            pm = GetComponent<PlayerMovementScript>();
    }

    void Update()
    {
        if (pm.user_Found_Random_Enemy && !isInBattle)
            TriggerBattle();
    }

    void TriggerBattle()
    {
        Debug.Log("Battle triggered!");

        // Show the battle UI
        battleUI.SetActive(true);

        // Spawn the enemy at the specified spawn point
        Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);

        // Set the flag to prevent multiple encounters at once
        isInBattle = true;
    }

    // Method to end the battle and reset the state
    public void EndBattle()
    {
        battleUI.SetActive(false); // Hide the battle UI
        isInBattle = false;        // Allow future encounters
        pm.user_Found_Random_Enemy = false; // Reset encounter flag in PlayerMovementScript
    }
}

