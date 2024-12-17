
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AttackSystem : MonoBehaviour
{
    public GameObject attackBox;         // The attack box UI panel
    public Slider attackSlider;           // The slider representing the timing bar
    public float sliderSpeed = 2f;        // Speed at which the slider moves
    public Button fightButton;            // The Fight button to initiate the attack
    public EnemyScript currentEnemy;      // Reference to the current enemy

    private bool isAttacking = false;     // Is the attack currently happening?

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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopAttack();
            }
        }
    }

    // Start the attack process
    void StartAttack()
    {
        if (currentEnemy == null)
        {
            Debug.LogError("No enemy assigned to attack!");
            return;
        }

        attackBox.SetActive(true);
        attackSlider.value = 0;          // Reset slider to start
        isAttacking = true;
    }

    // Move the slider handle
    void MoveSlider()
    {
        attackSlider.value += sliderSpeed * Time.deltaTime;

        // Loop the slider if it reaches the end
        if (attackSlider.value >= attackSlider.maxValue)
        {
            attackSlider.value = 0;
        }
    }

    // Stop the slider and determine the hit outcome
    void StopAttack()
    {
        isAttacking = false;
        attackBox.SetActive(false);

        float hitPosition = attackSlider.value;

        int damage = 0;

        // Determine the hit zone and assign damage accordingly
        if (hitPosition >= 0.45f && hitPosition <= 0.55f)
        {
            Debug.Log("Perfect Hit! (Green Zone)");
            damage = 10; // High damage for a perfect hit
        }
        else if (hitPosition >= 0.35f && hitPosition < 0.45f || hitPosition > 0.55f && hitPosition <= 0.65f)
        {
            Debug.Log("Good Hit! (Yellow Zone)");
            damage = 5; // Medium damage for a good hit
        }
        else
        {
            Debug.Log("Missed! (Red Zone)");
            damage = 0; // No damage for a miss
        }

        // Apply damage to the current enemy
        if (damage > 0)
        {
            currentEnemy.TakeDamage(damage);
        }
        else
        {
            Debug.Log($"{currentEnemy.enemyName} dodged the attack!");
        }
    }
}
