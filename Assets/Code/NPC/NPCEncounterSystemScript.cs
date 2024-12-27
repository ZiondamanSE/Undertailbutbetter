using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;


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

    [Space]

    [SerializeField] private GameObject npcChattingWindow;
    [SerializeField] private NPCChatbox npcChatbox;
    [SerializeField] private Transform npcArtSpawnParent; // Parent Transform within Canvas (UI)

    [Space]

    private bool isChatting = false;
    private int currentDialogueIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private GameObject currentNpcArtInstance; // Tracks the currently spawned NPC art

    public SpriteRenderer spriteRendererIntIcon;
    public UnityEngine.Color colorIntIcon;
    public GameObject interactive_prompt;

    [Header("Qest")]
    [HideInInspector] public bool activeQest;
    public string qestJob;

    void Awake()
    {
        spriteRendererIntIcon = interactive_prompt.GetComponent<SpriteRenderer>();

        colorIntIcon.a = 0f; // setting transparanciy to 0;
        colorIntIcon.r = 1f; // setting to defult color
        colorIntIcon.g = 1f;
        colorIntIcon.b = 1f;
        spriteRendererIntIcon.color = colorIntIcon;

        npcChattingWindow.SetActive(false);
        if (pm == null)
            pm = GetComponent<PlayerMovementScript>();
    }


    void Update()
    {
        if (pm.player_Next_to_NPC) // checking if the player is near 1 is true 2 is false
            interactiveAni(1);
        else if (!pm.player_Next_to_NPC)
            interactiveAni(2);

        if (pm.int_NPC && !isChatting)
        {
            interactive_prompt.SetActive(true);
            interactiveAni(1);
            StartDialogue();
        }
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

    void interactiveAni(int fashing)
    {
        //Debug.Log("fading");
        if (colorIntIcon.a != 1f && fashing == 1) // fading in
        {
            //Debug.Log("fading in");
            colorIntIcon.a += 0.01f;
            spriteRendererIntIcon.color = colorIntIcon;
        }

        if (colorIntIcon.a != 0f && fashing == 2) // fading out
        {
            //Debug.Log("fading out");
            colorIntIcon.a -= 0.01f;
            spriteRendererIntIcon.color = colorIntIcon;
        }
        
        if (colorIntIcon.a < 0) // stoping overdrive of value
            colorIntIcon.a = 0;
        if (colorIntIcon.a > 1)
            colorIntIcon.a = 1;
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
        activeQest = true;
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
