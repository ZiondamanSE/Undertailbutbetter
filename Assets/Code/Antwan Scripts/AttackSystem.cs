using UnityEngine;
using UnityEngine.UI;

public class AttackSystem : MonoBehaviour
{
    public Slider attackSlider;          // The slider representing the timing bar
    public Button fightButton;           // The Fight button to initiate the attack
    public GameObject attackBox;         // The attack box UI panel

    public float sliderSpeed = 2f;       // Speed at which the slider moves
    [HideInInspector] public int player_Damage = 0;
    private float hitPosition;
    [HideInInspector] public bool isAttacking = false;    // Is the attack currently happening?
    [HideInInspector] public bool feedEnemyInfo = false;

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

        Debug.Log("[AttackSystem] Starting attack...");
        fightButton.interactable = false; // Disable the fight button
        attackBox.SetActive(true);       // Show the attack box
        attackSlider.value = 0;          // Reset slider to start
        isAttacking = true;
        feedEnemyInfo = false;

        // Deselect the button to prevent keyboard input from triggering it
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    // Move the slider handle
    void MoveSlider()
    {
        float progress = attackSlider.value + sliderSpeed * Time.deltaTime;
        progress = Mathf.Repeat(progress, attackSlider.maxValue);

        attackSlider.value = progress;
    }

    // Stop the slider and determine the hit outcome
    void StopAttack()
    {
        isAttacking = false;
        attackBox.SetActive(false);      // Hide the attack box
        fightButton.interactable = true; // Re-enable the fight button

        hitPosition = attackSlider.value;

        // Determine the hit zone and assign damage accordingly
        
        if (hitPosition >= 0.45f && hitPosition <= 0.66f)
        {
            Debug.Log("[AttackSystem] Weekhit! (Yellow Zone)");
            player_Damage = 5; // High damage for a perfect hit
        }
        else if (hitPosition >= 0.66f && hitPosition <= 0.80f)
        {
            player_Damage = 10; // Medium damage for a good hit

            Debug.Log("[AttackSystem] Good Hit! (Green Zone)");
        }
        else if (hitPosition >= 0.80f && hitPosition <= 0.90f)
        {
            player_Damage = 20; // Medium damage for a good hit

            Debug.Log("[AttackSystem] Amazing Hit! (Blue Zone)");
        }
        else if (hitPosition >= 0.90f && hitPosition <= 0.95f)
        {
            player_Damage = 30; // Medium damage for a good hit

            Debug.Log("[AttackSystem] Perfict Hit! (Perpule Zone)");
        }

        else
        {
            Debug.Log("[AttackSystem] Missed! (Red Zone)");
        }

        Debug.Log($"[AttackSystem] Damage dealt: {player_Damage}");

        feedEnemyInfo = true;

    }
}
