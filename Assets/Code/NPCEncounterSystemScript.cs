using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/*
Dialogue Class:
    Holds NPC text, custom strings, and art prefab references.

NPCEncounterSystemScript:

    Initialization:
        Awake: Sets up NPC chatting UI and initializes PlayerMovementScript reference.

    Input Handling:
        Update: Checks for NPC interaction and user input to proceed dialogue.

    Dialogue Control:
        StartDialogue: Initiates dialogue sequence.
        DisplayDialogue: Displays NPC text and art.
        NextDialogue: Advances to the next dialogue or ends the conversation.

    Typing Effect:
        TypeSentence: Gradually types out text.
        FinishCurrentSentence: Skips to complete text immediately.

    Art Management:
        SpawnNpcArt: Handles NPC art display.

    Ending Dialogue:
        EndDialogue: Closes the dialogue window and resets states.

*/


[System.Serializable] // Ensures Dialogue class is serializable in Inspector
public class Dialogue
{
    [SerializeField] public TMP_Text textDisplay; // Reference to the Text component
    [SerializeField] public string customString = "Your custom text here!";
    [SerializeField] public GameObject npcArt; // Prefab for NPC art
    [SerializeField] public float letterDelay = 0.05f; // Custom delay for typing letters
}

public class NPCEncounterSystemScript : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public List<Dialogue> textTriggers = new List<Dialogue>(); // List of dialogue triggers

    [Header("References")]
    [SerializeField] private PlayerMovementScript pm;
    [SerializeField] private GameObject npcChattingWindow;
    [SerializeField] private NPCChatbox npcChatbox;
    [SerializeField] private Transform npcArtSpawnParent; // Parent Transform within Canvas (UI)

    private bool isChatting = false;
    private int currentDialogueIndex = 0;

    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private GameObject currentNpcArtInstance; // Tracks the currently spawned NPC art

    void Awake()
    {
        npcChattingWindow.SetActive(false);
        if (pm == null)
            pm = GetComponent<PlayerMovementScript>();
    }

    void Update()
    {
        if (pm.int_NPC && !isChatting)
            StartDialogue();

        if (isChatting && Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
                FinishCurrentSentence();
            else
                NextDialogue();
        }
    }

    void StartDialogue()
    {
        npcChattingWindow.SetActive(true);

        pm.not_In_Screen = true;
        
        npcChatbox.Up();
        
        isChatting = true;
        currentDialogueIndex = 0;
        
        // add counter
        DisplayDialogue();
    }

    void DisplayDialogue()
    {
        if (currentDialogueIndex < textTriggers.Count)
        {
            Dialogue dialogue = textTriggers[currentDialogueIndex];
            SpawnNpcArt(dialogue.npcArt);

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeSentence(dialogue.customString, dialogue.textDisplay, dialogue.letterDelay));
        }
        else
            EndDialogue();
    }

    void SpawnNpcArt(GameObject npcArtPrefab)
    {
        if (currentNpcArtInstance != null) // clear assets
            Destroy(currentNpcArtInstance);


        if (npcArtPrefab != null && npcArtSpawnParent != null)
        {
            currentNpcArtInstance = Instantiate(npcArtPrefab, npcArtSpawnParent);
            RectTransform rt = currentNpcArtInstance.GetComponent<RectTransform>();

            if (rt != null) // rt to null
            {
                rt.localPosition = Vector3.zero;
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
            }
        }
    }

    IEnumerator TypeSentence(string sentence, TMP_Text textDisplay, float letterDelay)
    {
        isTyping = true;
        textDisplay.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }

        isTyping = false;
    }

    void FinishCurrentSentence()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        Dialogue dialogue = textTriggers[currentDialogueIndex];
        dialogue.textDisplay.text = dialogue.customString;
        isTyping = false;
    }

    void NextDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex < textTriggers.Count)
            DisplayDialogue();
        else
            EndDialogue();
    }

    void EndDialogue()
    {
        npcChatbox.Down();
        StartCoroutine(CloseWindowAfterDelay());
    }

    IEnumerator CloseWindowAfterDelay()
    {
        yield return new WaitForSeconds(0.34f);

        npcChattingWindow.SetActive(false);
        
        pm.not_In_Screen = false;
        pm.int_NPC = false;

        if (currentNpcArtInstance != null)
        {
            Destroy(currentNpcArtInstance);
            currentNpcArtInstance = null;
        }

        isChatting = false;
        currentDialogueIndex = 0;
    }
}
