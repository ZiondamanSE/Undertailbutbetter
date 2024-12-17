using UnityEngine;

public class RandomBattleTrigger : MonoBehaviour
{
    public GameObject battleUI;        // The Battle UI GameObject
    public GameObject enemyPrefab;     // The enemy prefab to spawn
    public RectTransform spawnPoint;   // The spawn point (RectTransform) for the enemy within the Battle UI

    [Range(0, 100)]
    public int encounterChance = 20;   // Chance of battle occurring (20%)

    public float timeBetweenEncounters = 5f; // Time interval between encounter checks (seconds)
    private float timer;               // Timer to track encounter checks

    private bool isInBattle = false;   // Flag to prevent multiple encounters at once

    void Start()
    {
        battleUI.SetActive(false);     // Make sure battle UI is hidden initially
        timer = timeBetweenEncounters; // Set the initial timer
    }

    void Update()
    {
        if (!isInBattle)
        {
            timer -= Time.deltaTime; // Decrease the timer by the time elapsed
            if (timer <= 0)
            {
                // Time to check for a random encounter
                CheckForRandomEncounter();
                timer = timeBetweenEncounters; // Reset the timer
            }
        }
    }

    void CheckForRandomEncounter()
    {
        // Roll for a random number between 0 and 100
        int roll = Random.Range(0, 100);
        Debug.Log($"Encounter Roll: {roll}");

        if (roll < encounterChance)
        {
            TriggerBattle();
        }
    }

    void TriggerBattle()
    {
        Debug.Log("Random battle triggered!");

        // Trigger the battle UI
        battleUI.SetActive(true);

        // Spawn the enemy as a child of the Battle UI
        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        enemyInstance.transform.SetParent(battleUI.transform, false);

        // Optionally reset the enemy's position to the spawn point's local position
        enemyInstance.GetComponent<RectTransform>().anchoredPosition = spawnPoint.anchoredPosition;

        // Set the flag to prevent multiple encounters at once
        isInBattle = true;
    }

    // This method will be called to reset the battle flag once the battle ends
    public void EndBattle()
    {
        battleUI.SetActive(false); // Hide the battle UI
        isInBattle = false;        // Allow future encounters

        // Optional: Destroy the spawned enemy to clean up
        foreach (Transform child in battleUI.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
