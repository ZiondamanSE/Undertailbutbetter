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

    public GameObject spawnpoint;
    private int randomIndex;
    private bool isDialogDisplaying = false;
    bool enemyIsDead;

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
        if (!isDialogDisplaying && encounterSystemScript.isInBattle)
        {
            NameCalling();
        }

        if (attackSystem.feedEnemyInfo)
        {
            DamageToEnemy(attackSystem.player_Damage, EnemyDodgeRate());
            Debug.Log($"Player damage input: {attackSystem.player_Damage} | Enemy dodged: {EnemyDodgeRate()}");
        }
    }

    void NameCalling()
    {
        if (dialogManager != null)
        {
            dialogManager.DisplayDialog(Enemies[randomIndex].EnemyName);
            isDialogDisplaying = true;
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
            dialogManager.DisplayDialog($"Player dealt: -{damageAmount}");
            isDialogDisplaying = true;
            StartCoroutine(Delay());
            dialogManager.DisplayDialog($"Enemy's health is now: {Enemies[randomIndex].Health}");
        }
    }

    bool EnemyDodgeRate()
    {
        float randomValue = Random.Range(0f, 1f);
        return randomValue < Enemies[randomIndex].DodgeRate;
    }

    void DamageToEnemy(float damage, bool resistance)
    {
        if (resistance)
            damage = 0;
        else
            Enemies[randomIndex].Health -= damage;

        if (Enemies[randomIndex].Health <= 0)
        {
            enemyIsDead = true;
            Debug.Log($"Enemy {Enemies[randomIndex].EnemyName} is dead.");
        }
        else
        {
            DamageCalling(damage);
        }

        attackSystem.feedEnemyInfo = false;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(10f);
        isDialogDisplaying = false;
    }
}
    