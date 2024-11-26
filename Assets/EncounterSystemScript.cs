using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterSystemScript : MonoBehaviour
{
    [SerializeField] private PlayerMovementScript pm;

    public GameObject battleUI; // The Battle UI GameObject

    private bool isInBattle = false; // Flag to prevent multiple encounters at once

    void Awake()
    {
        battleUI.SetActive(false); // Make sure battle UI is hidden initially
        if (pm == null)
            pm = GetComponent<PlayerMovementScript>();
    }

    void Update()
    {
        if (pm.user_Found_Random_Enemy)
            TriggerBattle();
    }

    void TriggerBattle()
    {
        // Trigger the battle UI
        battleUI.SetActive(true);

        // Set the flag to prevent multiple encounters at once
        isInBattle = true;
    }

    // This method will be called to reset the battle flag once the battle ends
    public void EndBattle()
    {
        battleUI.SetActive(false); // Hide the battle UI
        isInBattle = false; // Allow future encounters
    }
}
