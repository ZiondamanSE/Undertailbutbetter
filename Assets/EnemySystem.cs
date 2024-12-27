using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] DialogManager dialogManager;

    public GameObject spawnpoint;
    private int randomIndex;


    void Start()
    {
        randomIndex = Random.Range(0, Enemies.Count);
        EnemyData randomEnemy = Enemies[randomIndex];

        // Instantiate the enemy at the spawn point
        Instantiate(randomEnemy.EnemyPrefab, spawnpoint.transform.position, Quaternion.identity);
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log($"enemy index = {randomIndex}");
        Namecalling();
    }

    void Namecalling()
    {
            if (dialogManager != null)
            {
                dialogManager.DisplayDialog(Enemies[randomIndex].EnemyName);
            }
            else
            {
                Debug.LogError("DialogManager is not assigned.");
            }
    }
}
