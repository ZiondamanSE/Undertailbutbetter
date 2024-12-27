using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterSystemScript : MonoBehaviour
{
    [SerializeField] private PlayerMovementScript pm;
    
    public GameObject battleUI;       // The Battle UI GameObject
    public GameObject enemyPrefab;    // The Enemy prefab to spawn (should have a RectTransform)
    public RectTransform enemySpawnPoint; // The spawn point for the enemy (inside the Battle UI)

    public bool isInBattle = false;  // Flag to prevent multiple encounters at once

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

        // Spawn the enemy as a child of the Battle UI
        GameObject enemyInstance = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
        enemyInstance.transform.SetParent(battleUI.transform, false);

        // Set the enemy's anchored position to match the spawn point's anchored position
        enemyInstance.GetComponent<RectTransform>().anchoredPosition = enemySpawnPoint.anchoredPosition;

        // Set the flag to prevent multiple encounters at once
        isInBattle = true;
    }

    // Method to end the battle and reset the state
    public void EndBattle()
    {
        battleUI.SetActive(false); // Hide the battle UI
        isInBattle = false;        // Allow future encounters
        pm.user_Found_Random_Enemy = false; // Reset encounter flag in PlayerMovementScript

        // Optional: Destroy all children of the battle UI to clean up spawned enemies
        foreach (Transform child in battleUI.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
