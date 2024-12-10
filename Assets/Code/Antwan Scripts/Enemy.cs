using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public string enemyName = "Default Enemy";
    public int health = 10;

    void Start()
    {
        Debug.Log($"{enemyName} has appeared with {health} HP!");
    }
}

