using UnityEngine;
using UnityEngine.UI; // Required for working with UI elements

public class BattleManager : MonoBehaviour
{
    public GameObject battleBox;           // The Battle UI Panel
    public Image enemyImage;               // The Image component to display the enemy sprite
    public Sprite[] enemySprites;          // Array of enemy sprites for random selection

    private bool isInBattle = false;

    void Start()
    {
        // Ensure the battle UI is hidden at the start
        battleBox.SetActive(false);
    }

    public void TriggerBattle()
    {
        if (!isInBattle)
        {
            isInBattle = true;

            // Activate the Battle UI
            battleBox.SetActive(true);

            // Randomly select an enemy sprite (if you have multiple enemies)
            if (enemySprites.Length > 0)
            {
                int randomIndex = Random.Range(0, enemySprites.Length);
                enemyImage.sprite = enemySprites[randomIndex];
            }
        }
    }

    public void EndBattle()
    {
        // Hide the Battle UI and reset the battle state
        battleBox.SetActive(false);
        isInBattle = false;
    }
}
