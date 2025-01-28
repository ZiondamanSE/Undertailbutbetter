using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] BattelChatBox BCB;
    [SerializeField] AttackSystem attackSystem;

    private int randomIndex;
    public bool enemyIsDead;
    private bool isDialogDisplaying = false;
    private bool hasCalledName = false; // Flag to ensure NameCalling runs only once
    public GameObject spawnpoint;
    [HideInInspector] public GameObject enemyUI;

    [Header("Floating Animation Settings")]
    public float floatAmplitude = 10f; // The height of the floating animation
    public float floatFrequency = 1f;  // The speed of the floating animation

    [Header("Shake Animation Settings")]
    public float shakeAmplitude = 5f;  // How far the enemy will shake left and right
    public float shakeDuration = 0.2f; // Duration of the shake effect

    private Vector2 initialPosition;
    private bool isShaking = false;
    private float shakeTimer = 0f;


    public int levle;

    void Start()
    {
        isDialogDisplaying = false;
        enemyIsDead = false;

        if (Enemies == null || Enemies.Count == 0)
        {
            Debug.LogError("No enemies available in the list.");
        }

        encounterSystemScript ??= GetComponent<EncounterSystemScript>();
        attackSystem ??= GetComponent<AttackSystem>();

        if (BCB == null)
        {
            BCB = FindObjectOfType<BattelChatBox>();
            if (BCB == null)
            {
                Debug.LogError("DialogManager is not assigned and could not be found in the scene.");
            }
        }

        if (spawnpoint == null)
        {
            Debug.LogError("Spawnpoint is not assigned.");
        }

        randomIndex = Random.Range(0, Enemies.Count);
        Debug.Log($"Random index generated: {randomIndex}"); // Add debug log here

        EnemyData randomEnemy = Enemies[randomIndex];
        enemyUI = Instantiate(Enemies[randomIndex].EnemyPrefab, spawnpoint.transform);

        // Save the initial position of the enemyUI
        if (enemyUI != null)
        {
            RectTransform rectTransform = enemyUI.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                initialPosition = rectTransform.anchoredPosition;
            }
            else
            {
                Debug.LogError("EnemyPrefab does not have a RectTransform component.");
            }
        }
    }

    void Update()
    {
        AssetAni(); // Call the animation logic in Update

        if (isShaking)
        {
            ShakeEffect(); // Apply shake effect
        }

        if (attackSystem.isAttacking) // Check if an attack is in progress
        {
            isDialogDisplaying = false; // Ensure no dialog is displayed
        }

        if (!isDialogDisplaying && encounterSystemScript.isInBattle && !hasCalledName)
        {
            AssetsSpawning();
            NameCalling();
        }

        if (attackSystem.feedEnemyInfo)
        {
            isDialogDisplaying = false;
            DamageToEnemy(attackSystem.player_Damage, EnemyDodgeRate());
            Debug.Log($"Player damage input: {attackSystem.player_Damage} | Enemy dodged: {EnemyDodgeRate()}");
        }
    }

    void AssetAni()
    {
        if (enemyUI != null)
        {
            RectTransform rectTransform = enemyUI.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Calculate the floating effect using a sine wave
                float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
                rectTransform.anchoredPosition = initialPosition + new Vector2(0, offsetY);
            }
        }
    }

    void ShakeEffect()
    {
        if (enemyUI != null)
        {
            RectTransform rectTransform = enemyUI.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Create the shake effect using a sine wave for smooth wiggle
                float offsetX = Mathf.Sin(Time.time * 50) * shakeAmplitude; // Fast oscillation for a quick shake
                rectTransform.anchoredPosition = initialPosition + new Vector2(offsetX, 0);

                // Decrease the shake timer
                shakeTimer -= Time.deltaTime;
                if (shakeTimer <= 0f)
                {
                    isShaking = false; // Stop shaking after the duration
                    rectTransform.anchoredPosition = initialPosition; // Reset position to the initial one
                }
            }
        }
    }

    // Call this method when the enemy is hit
    public void ApplyShakeFeedback()
    {
        isShaking = true;
        shakeTimer = shakeDuration; // Set the duration for the shake
    }

    void AssetsSpawning()
    {
        // Ensure the prefab has a RectTransform (required for UI elements in Canvas)
        RectTransform rectTransform = enemyUI.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Set the prefab's position relative to the spawnpoint
            rectTransform.anchoredPosition = Vector2.zero; // Center within the parent (spawnpoint)
            rectTransform.localScale = Vector3.one;       // Reset scale to match the UI
        }
        else
            Debug.LogError("EnemyPrefab is not set up with a RectTransform for Canvas use.");
    }

    void NameCalling()
    {
        if (BCB != null)
        {
            BCB.Input($"Yo, big alert! A wild {Enemies[randomIndex].EnemyName} just rolled up lookin' for trouble!");
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
        if (BCB != null)
        {
            StartCoroutine(DisplayDamageDialogs(damageAmount));
        }
    }

    private IEnumerator DisplayDamageDialogs(float damageAmount)
    {
        // Display the player's damage
        BCB.Input($"Smackdown delivered! Enemy took -{damageAmount}HP, ouch!");
        isDialogDisplaying = true;

        yield return new WaitForSeconds(2f); // Wait for dialog processing

        // Display the enemy's remaining HP
        BCB.Input($"Enemy hangin' on with {Enemies[randomIndex].Health}HP! Keep up the heat!");

        yield return new WaitForSeconds(2f); // Wait for dialog processing
        isDialogDisplaying = false;
    }


    private IEnumerator EnimeyIsRizzed()
    {
        BCB.Input($"GG! You totally rizzed up {Enemies[randomIndex].EnemyName}. They're down for the count!");
        enemyUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(levle);
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
            BCB.Input($"Whoa! {Enemies[randomIndex].EnemyName} just matrix-dodged that hit. Slick moves!");
            damage = 0;
        }
        else
        {
            Enemies[randomIndex].Health -= damage;
            ApplyShakeFeedback(); // Trigger the shake feedback on hit
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
