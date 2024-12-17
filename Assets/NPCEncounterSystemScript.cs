using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCEncounterSystemScript : MonoBehaviour
{
    [SerializeField] private PlayerMovementScript pm;

    public GameObject npc_Chatting_Window;

    void Awake()
    {
        npc_Chatting_Window.SetActive(false);
        if (pm == null)
            pm = GetComponent<PlayerMovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.int_NPC)
        {
            npc_Chatting_Window.SetActive(true);
            pm.not_In_Screen = false;
        }

        if (npc_Chatting_Window && Input.GetKey(KeyCode.Escape))
        {
            npc_Chatting_Window.SetActive(false);
            pm.not_In_Screen = true;
        }
    }
}
