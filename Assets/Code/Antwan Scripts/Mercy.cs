using UnityEngine;

public class MercyManager : MonoBehaviour
{
    public GameObject runOptionUI; // UI element for the Run option
    public EncounterSystemScript encounterSystem; // Reference to the Encounter System

    void Start()
    {

    }

    public void MercyAction()
    {
    }

    public void RunAction()
    {
        Debug.Log("Run option selected!");

        // Exit the battle and reset the encounter system
        if (encounterSystem != null)
            encounterSystem.EndBattle();
        
        // Hide the Run option after running
        if (runOptionUI != null)
            runOptionUI.SetActive(false);
    }
}
