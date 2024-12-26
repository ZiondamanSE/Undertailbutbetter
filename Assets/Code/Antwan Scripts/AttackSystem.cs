using UnityEngine;
using UnityEngine.UI;

public class AttackSystem : MonoBehaviour
{
    public GameObject attackBox;         // The attack box UI panel
    public Slider attackSlider;          // The slider representing the timing bar
    public float sliderSpeed = 2f;       // Speed at which the slider moves
    public Button fightButton;           // The Fight button to initiate the attack
    public EnemyScript currentEnemy;     // The current enemy to attack

    private float hitPosition;
    private bool isAttacking = false;    // Is the attack currently happening?

    void Start()
    {
        attackBox.SetActive(false);       // Hide the attack box initially
        fightButton.onClick.AddListener(StartAttack); // Assign the Fight button click event
    }

    void Update()
    {
        if (isAttacking)
        {
            MoveSlider();

            // Stop attack on space press
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log($"[AttackSystem] Space pressed at attackSlider.value = {attackSlider.value}");
                StopAttack();
            }
        }
    }

    // Start the attack process
    void StartAttack()
    {
        if (isAttacking)
        {
            Debug.LogWarning("[AttackSystem] Attempted to start an attack while already attacking!");
            return;
        }

        if (currentEnemy == null)
        {
            Debug.LogError("[AttackSystem] No enemy assigned to attack!");
            return;
        }

        Debug.Log("[AttackSystem] Starting attack...");
        fightButton.interactable = false; // Disable the fight button
        attackBox.SetActive(true);       // Show the attack box
        attackSlider.value = 0;          // Reset slider to start
        isAttacking = true;

        // Deselect the button to prevent keyboard input from triggering it
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    // Move the slider handle
    void MoveSlider()
    {
        float progress = attackSlider.value + sliderSpeed * Time.deltaTime;
        progress = Mathf.Repeat(progress, attackSlider.maxValue);

        attackSlider.value = progress;
        Debug.Log($"[AttackSystem] Moving Slider: {attackSlider.value}");
    }

    // Stop the slider and determine the hit outcome
    void StopAttack()
    {
        isAttacking = false;
        attackBox.SetActive(false);      // Hide the attack box
        fightButton.interactable = true; // Re-enable the fight button

        hitPosition = attackSlider.value;
        Debug.Log($"[AttackSystem] Attack stopped at hitPosition: {hitPosition}");

        // Determine the hit zone and assign damage accordingly
        int damage = 0;
        if (hitPosition >= 0.45f && hitPosition <= 0.55f)
        {
            Debug.Log("[AttackSystem] Weekhit! (Yellow Zone)");
            damage = 5; // High damage for a perfect hit
        }
        else if (hitPosition >= 0.55f && hitPosition <= 0.75f)
        {
            damage = 10; // Medium damage for a good hit

            Debug.Log("[AttackSystem] Good Hit! (Green Zone)");
        }
        else if (hitPosition >= 0.80f && hitPosition <= 0.90f)
        {
            damage = 20; // Medium damage for a good hit

            Debug.Log("[AttackSystem] Amazing Hit! (Blue Zone)");
        }
        else if (hitPosition >= 0.90f && hitPosition <= 0.95f)
        {
            damage = 30; // Medium damage for a good hit

            Debug.Log("[AttackSystem] Perfict Hit! (Perpule Zone)");
        }

        else
        {
            Debug.Log("[AttackSystem] Missed! (Red Zone)");
        }

        Debug.Log($"[AttackSystem] Damage dealt: {damage}");

        // Apply damage to the current enemy
        if (damage > 0)
        {
            currentEnemy.TakeDamage(damage);
        }
        else
        {
            Debug.Log($"[AttackSystem] {currentEnemy.enemyName} dodged the attack!");
        }
    }
}
