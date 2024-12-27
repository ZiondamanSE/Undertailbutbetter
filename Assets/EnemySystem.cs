using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    [SerializeField] public GameObject EnemyPrefab;
    [SerializeField] public float Health;
    [SerializeField] public float Damage;
    [SerializeField] public float DodgeRate;
    [SerializeField] public string EnemyName;
}

public class EnemySystem : MonoBehaviour
{
    public List<EnemyData> Enemies = new List<EnemyData>();

    [SerializeField] EncounterSystemScript encounterSystemScript;
    [SerializeField] DialogManager dialogManager;
    [SerializeField] AttackSystem attackSystem;

    private int randomIndex;
    public bool enemyIsDead;
    private bool isDialogDisplaying = false;
    private bool hasCalledName = false; // Flag to ensure NameCalling runs only once
    public GameObject spawnpoint;

    void Start()
    {
        isDialogDisplaying = false;
        enemyIsDead = false;

        if (Enemies == null || Enemies.Count == 0)
        {
            Debug.LogError("No enemies available in the list.");
            return;
        }

        if (encounterSystemScript == null)
            encounterSystemScript = GetComponent<EncounterSystemScript>();
        if (attackSystem == null)
            attackSystem = GetComponent<AttackSystem>();

        if (dialogManager == null)
        {
            Debug.LogError("DialogManager is not assigned.");
            return;
        }

        if (spawnpoint == null)
        {
            Debug.LogError("Spawnpoint is not assigned.");
            return;
        }

        randomIndex = Random.Range(0, Enemies.Count);
        EnemyData randomEnemy = Enemies[randomIndex];
            
        Instantiate(randomEnemy.EnemyPrefab, spawnpoint.transform.position, Quaternion.identity);
    }

    void Update()
    {
        if (attackSystem.isAttacking) // Check if an attack is in progress
        {
            dialogManager.HideDialog(); // Hide dialog when attack starts
            isDialogDisplaying = false; // Ensure no dialog is displayed
            return;
        }

        if (!isDialogDisplaying && encounterSystemScript.isInBattle && !hasCalledName)
        {
            NameCalling();
        }

        if (attackSystem.feedEnemyInfo)
        {
            isDialogDisplaying = false;
            DamageToEnemy(attackSystem.player_Damage, EnemyDodgeRate());
            Debug.Log($"Player damage input: {attackSystem.player_Damage} | Enemy dodged: {EnemyDodgeRate()}");
        }
    }


    void NameCalling()
    {
        if (dialogManager != null)
        {
            dialogManager.DisplayDialog($"Yo, big alert! A wild {Enemies[randomIndex].EnemyName} just rolled up lookin' for trouble!");
            isDialogDisplaying = true;
            hasCalledName = true; // Mark as called
        }
        else
        {
            Debug.LogError("DialogManager is not assigned.");
        }
    }

    void DamageCalling(float damageAmount)
    {
        if (dialogManager != null)
        {
            StartCoroutine(DisplayDamageDialogs(damageAmount));
        }
    }

    private IEnumerator DisplayDamageDialogs(float damageAmount)
    {
        // Display the player's damage
        dialogManager.DisplayDialog($"Smackdown delivered! Enemy took -{damageAmount}HP, ouch!");
        isDialogDisplaying = true;

        yield return new WaitForSeconds(4f); // Wait for dialog processing

        // Display the enemy's remaining HP
        dialogManager.DisplayDialog($"Enemy hangin' on with {Enemies[randomIndex].Health}HP! Keep up the heat!");

        yield return new WaitForSeconds(4f); // Wait for dialog processing
        isDialogDisplaying = false;
    }


    private IEnumerator EnimeyIsRizzed()
    {
        dialogManager.DisplayDialog($"GG! You totally rizzed up {Enemies[randomIndex].EnemyName}. They're down for the count!");
        yield return new WaitForSeconds(4f);
        enemyIsDead = true;
    }

    bool EnemyDodgeRate()
    {
        float monkeymoney = Random.Range(0, Enemies[randomIndex].DodgeRate);

        if (monkeymoney != Enemies[randomIndex].DodgeRate || monkeymoney + 2 >= Enemies[randomIndex].DodgeRate)
            return false;
        else
            return true;
    }

    void DamageToEnemy(float damage, bool resistance)
    {
        if (resistance)
        {
            dialogManager.DisplayDialog($"Whoa! {Enemies[randomIndex].EnemyName} just matrix-dodged that hit. Slick moves!");
            damage = 0;
        }
        else
        {
            Enemies[randomIndex].Health -= damage;
        }

        if (Enemies[randomIndex].Health <= 0)
        {
            StartCoroutine(EnimeyIsRizzed());
        }
        else
        {
            DamageCalling(damage);
        }

        attackSystem.feedEnemyInfo = false;
    }
}
