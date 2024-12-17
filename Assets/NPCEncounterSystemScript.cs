using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] // Ensures Dialogue class is serializable in Inspector
public class Dialogue
{
    [SerializeField] public TMP_Text textDisplay; // Reference to the Text component
    [SerializeField] public string customString = "Your custom text here!";
    [SerializeField] public GameObject npcArt; // Prefab for NPC art
}

public class NPCEncounterSystemScript : MonoBehaviour
{
    public List<Dialogue> textTriggers = new List<Dialogue>(); // List of dialogue triggers

    [SerializeField] private PlayerMovementScript pm;
    public GameObject npc_Chatting_Window;
    [SerializeField] private NPCChatbox npcChatbox;
    [SerializeField] private Transform npcArtSpawnParent; // Parent Transform within Canvas (UI)

    private bool isChatting = false;
    private int currentDialogueIndex = 0;

    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private bool hasFinishedTyping = false;

    [SerializeField] private float letterDelay = 1.0f; // Delay for letter typing (1 second per letter)

    private GameObject currentNpcArtInstance; // Tracks the currently spawned NPC art

    void Awake()
    {
        npc_Chatting_Window.SetActive(false);
        if (pm == null)
            pm = GetComponent<PlayerMovementScript>();
    }

    void Update()
    {
        if (pm.int_NPC && !isChatting)
        {
            StartDialogue();
        }

        // Handle input to skip or proceed to next dialogue
        if (isChatting && Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping) // If typing, finish the current sentence
            {
                FinishCurrentSentence();
            }
            else // Move to the next dialogue
            {
                NextDialogue();
            }
        }
    }

    void StartDialogue()
    {
        npc_Chatting_Window.SetActive(true);
        pm.not_In_Screen = true;
        npcChatbox.Up();
        isChatting = true;

        // Start the first dialogue
        currentDialogueIndex = 0;
        DisplayDialogue();
    }

    void DisplayDialogue()
    {
        if (currentDialogueIndex < textTriggers.Count)
        {
            Dialogue dialogue = textTriggers[currentDialogueIndex];

            // Spawn NPC Art within Canvas
            SpawnNpcArt(dialogue.npcArt);

            // Start typing the dialogue
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeSentence(dialogue.customString, dialogue.textDisplay));
        }
        else
        {
            EndDialogue();
        }
    }

    void SpawnNpcArt(GameObject npcArtPrefab)
    {
        // Destroy the previous NPC art if it exists
        if (currentNpcArtInstance != null)
        {
            Destroy(currentNpcArtInstance);
        }

        // Instantiate the new NPC art as a child of npcArtSpawnParent
        if (npcArtPrefab != null && npcArtSpawnParent != null)
        {
            currentNpcArtInstance = Instantiate(npcArtPrefab, npcArtSpawnParent);

            // Optionally reset RectTransform properties
            RectTransform rt = currentNpcArtInstance.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localPosition = Vector3.zero; // Centered in the parent
                rt.localScale = Vector3.one;     // Reset scale
                rt.anchoredPosition = Vector2.zero; // Ensure anchored correctly
            }
        }
    }

    IEnumerator TypeSentence(string sentence, TMP_Text textDisplay)
    {
        isTyping = true;
        hasFinishedTyping = false;

        textDisplay.text = ""; // Clear text display
        foreach (char letter in sentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(letterDelay); // Wait for specified delay per letter
        }

        isTyping = false;
        hasFinishedTyping = true;
    }

    void FinishCurrentSentence()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // Display the full sentence instantly
        Dialogue dialogue = textTriggers[currentDialogueIndex];
        dialogue.textDisplay.text = dialogue.customString;

        isTyping = false;
        hasFinishedTyping = true;
    }

    void NextDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex < textTriggers.Count)
        {
            DisplayDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        npcChatbox.Down();
        StartCoroutine(WaitToCloseWindow());
    }

    private IEnumerator WaitToCloseWindow()
    {
        yield return new WaitForSeconds(0.34f);
        npc_Chatting_Window.SetActive(false);
        pm.not_In_Screen = false;
        pm.int_NPC = false;

        // Destroy the current NPC art on close
        if (currentNpcArtInstance != null)
        {
            Destroy(currentNpcArtInstance);
            currentNpcArtInstance = null;
        }

        isChatting = false;
        currentDialogueIndex = 0; // Reset dialogue index
    }
}
