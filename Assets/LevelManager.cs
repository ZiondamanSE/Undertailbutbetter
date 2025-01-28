using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    [SerializeField] public RawImage levelTrans;

    public Color newColor;
    public int levle;
    public float fadeTime;
    bool cover;


    // Start is called before the first frame update
    void Start()
    {
        newColor.a = 1;
        newColor = levelTrans.color;
        cover = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cover)
            UnCover();

        if (cover)
            Cover();
    }

    void Cover()
    {
        if (levelTrans.color.a <= 1)
        {
            newColor.a += 0.1f * Time.deltaTime * fadeTime;
            levelTrans.color = newColor;
        }
        else
            SceneManager.LoadScene(levle);
    }
    void UnCover()
    {
        if (levelTrans.color.a >= 0)
        {
            newColor.a -= 0.1f * Time.deltaTime * fadeTime;
            levelTrans.color = newColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("SOMTHING COLIDED WITH ME!");
        cover = true;
    }
}
