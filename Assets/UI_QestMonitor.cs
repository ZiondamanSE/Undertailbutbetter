using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_QestMonitor : MonoBehaviour
{
    [SerializeField] public NPCEncounterSystemScript ness;
    [SerializeField] private PlayerMovementScript pm;
    [SerializeField] public TMP_Text textDisplay; // Reference to the Text component

    public float fadeTime;

    public Transform orbitingCenter; // The point around which the marker orbits.
    public Transform targetPoint;    // The point the marker should point toward.
    public GameObject pointer;
    public float radius = 5f;        // Distance from the orbiting center.
    public float rotationSpeed = 90f; // Degrees per second.
    public float moveSpeed = 2f;     // Speed at which the marker moves toward the target.


    // Start is called before the first frame update
    void Start()
    {
        pointer.SetActive(false);

        if (ness == null) 
            ness = GetComponent<NPCEncounterSystemScript>();

        if (pm == null)
            pm = GetComponent<PlayerMovementScript>();

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(!pm.player_Next_to_NPC)
            QestManiger(true, "Speak to the boss");
        else if (pm.player_Next_to_NPC)
            QestManiger(false, "");*/

        QestManiger(ness.activeQest, ness.qestJob);
    }

    void QestManiger(bool thereIsQest, string theQest) // UI ELEMENET
    {
        Color newColor = textDisplay.color;


        if (!thereIsQest)
        {
            newColor.a = 0;
        }
        else if (thereIsQest)
        {
            newColor.a += 0.1f * Time.deltaTime * fadeTime;
            textDisplay.color = newColor;
            pointer.SetActive(true);
            QestMarker();
        }

        textDisplay.color = newColor;
        textDisplay.text = "Quest : " + theQest;
    }

    void QestMarker() // POINTER ELEMENT
    {
        if (orbitingCenter == null || targetPoint == null || pointer == null)
        {
            Debug.LogWarning("Ensure orbitingCenter, targetPoint, and pointer are assigned in the inspector.");
            return;
        }

        // Access the Transform component of the pointer GameObject
        Transform pointerTransform = pointer.transform;

        // Calculate the direction from the orbiting center to the target point
        Vector2 directionToTarget = (targetPoint.position - orbitingCenter.position).normalized;

        // Calculate the desired position constrained within the radius
        Vector2 desiredPosition = (Vector2)orbitingCenter.position + directionToTarget * radius;

        // Smoothly move the pointer to the desired position
        pointerTransform.position = Vector2.Lerp(pointerTransform.position, desiredPosition, Time.deltaTime * moveSpeed);

        // Rotate the pointer to face the target point
        Vector2 direction = (Vector2)targetPoint.position - (Vector2)pointerTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        pointerTransform.rotation = Quaternion.RotateTowards(pointerTransform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);
    }



}
