using UnityEngine;

public class NPCChatbox : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on " + gameObject.name);
        }
    }

    // Update is called once per frame
    public void Up()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator not initialized in Up()");
            return;
        }
        animator.SetTrigger("Up");
    }

    public void Down()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator not initialized in Down()");
            return;
        }
        animator.SetTrigger("Down");
    }
}
