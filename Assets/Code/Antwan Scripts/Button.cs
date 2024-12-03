using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Optional for scene loading

public class ButtonManager : MonoBehaviour
{
    public void FightAction()
    {
        Debug.Log("Fight button clicked!");
        // Add functionality for Fight
    }

    public void ActAction()
    {
        Debug.Log("Act button clicked!");
        // Add functionality for Act
    }

    public void ItemAction()
    {
        Debug.Log("Item button clicked!");
        // Add functionality for Item
    }

    public void MercyAction()
    {
        Debug.Log("Mercy button clicked!");
        // Add functionality for Mercy
    }
}
