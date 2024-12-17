using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    float user_Vertical_Input;
    float user_Horizontal_Input;
    bool user_interact_Input;

    private bool user_can_Move = true;
    private bool wall_Infront_of_Player;
    [HideInInspector] public bool int_NPC; // interact with the npc 
    [HideInInspector] public bool not_In_Screen;

    public float stepping_Distens;
    public float movement_Speed;
    public float movement_Cooldown = 1f; // Adjustable time in seconds between movements

    [Space]

    [HideInInspector] public bool user_Found_Random_Enemy;
    public Vector2 encounter_Rainge;
    public int encounter_Enemy_value;

    private RaycastHit2D hit_up;
    private RaycastHit2D hit_down;
    private RaycastHit2D hit_left;
    private RaycastHit2D hit_right;

    private Vector3 newPosition;
    

    [Space]

    public bool there_is_Monstors;

    private void Awake()
    {
        if (encounter_Enemy_value < encounter_Rainge.x || encounter_Enemy_value > encounter_Rainge.y)
            Debug.LogError("Encounter_Enemy_value out of Range. there will be no Encouter!");
        if (encounter_Rainge.x == encounter_Rainge.y)
            Debug.LogError("encounter_Rainge has the same value and there is no Range!");
    }

    void Update()
    {
        PlayerInput();
        if (user_can_Move && !not_In_Screen)
        {
            PlayerMovement();
        }
        if (!user_can_Move && !not_In_Screen)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movement_Speed);
        }
    }

    void PlayerInput()
    {
        user_Horizontal_Input = Input.GetAxisRaw("Horizontal");
        user_Vertical_Input = Input.GetAxisRaw("Vertical");
        user_interact_Input = Input.GetKey(KeyCode.E);
    }

    void WallDetector()
    {
        hit_up = Physics2D.Raycast(transform.position, Vector2.up, stepping_Distens);
        hit_down = Physics2D.Raycast(transform.position, Vector2.down, stepping_Distens);
        hit_left = Physics2D.Raycast(transform.position, Vector2.left, stepping_Distens);
        hit_right = Physics2D.Raycast(transform.position, Vector2.right, stepping_Distens);

        Debug.DrawRay(transform.position, Vector2.up * stepping_Distens, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * stepping_Distens, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * stepping_Distens, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * stepping_Distens, Color.red);
    }

    void PlayerMovement()
    {
        WallDetector();

        newPosition = transform.position;

        // cheacks if there is somthing in the way and if so stops the input

        if (user_Horizontal_Input > 0 && (hit_right.collider == null || !hit_right.collider.CompareTag("wall")) && (hit_right.collider == null || !hit_right.collider.CompareTag("npc")))
            newPosition.x += stepping_Distens;
        else if (user_Horizontal_Input < 0 && (hit_left.collider == null || !hit_left.collider.CompareTag("wall")) && (hit_left.collider == null || !hit_left.collider.CompareTag("npc")))
            newPosition.x -= stepping_Distens;
        else if (user_Vertical_Input > 0 && (hit_up.collider == null || !hit_up.collider.CompareTag("wall")) && (hit_up.collider == null || !hit_up.collider.CompareTag("npc")))
            newPosition.y += stepping_Distens;
        else if (user_Vertical_Input < 0 && (hit_down.collider == null || !hit_down.collider.CompareTag("wall")) && (hit_down.collider == null || !hit_down.collider.CompareTag("npc")))
            newPosition.y -= stepping_Distens;

        if (hit_down.collider.CompareTag("npc") || hit_left.collider.CompareTag("npc") || hit_right.collider.CompareTag("npc") || hit_up.collider.CompareTag("npc")) // if there is an npc near
        {
            Debug.Log("player found npc");
            InteractiveNPC();
        }


        // Smoothly move the player towards the target position
        if (newPosition != transform.position)
        {
            StartCoroutine(MovementCooldown());
        }
    }

    void RandomEnemyEncounter()
    {
        // Specify UnityEngine.Random to resolve the ambiguity
        float randomEncounter = UnityEngine.Random.Range(encounter_Rainge.x, encounter_Rainge.y);
        randomEncounter = MathF.Round(randomEncounter);

        if (randomEncounter == encounter_Enemy_value)
            user_Found_Random_Enemy = true;

        if (user_Found_Random_Enemy)
            Debug.Log("user_Found_Random_Enemy");
        else
            Debug.Log("did not user_Found_Random_Enemy and the curent encouter nummber was on : " + randomEncounter);
    }

    void InteractiveNPC()
    {
        if (user_interact_Input)
        {
            Debug.Log("Chatting with npc...");
            int_NPC = true;
        }
    }

    IEnumerator MovementCooldown()
    {
        user_can_Move = false;
        yield return new WaitForSeconds(movement_Cooldown);
        if(there_is_Monstors)
            RandomEnemyEncounter();
        user_can_Move = true;
    }
}
