using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [Header("TimeLimit")]
    [SerializeField] float endTime = 60f;
    [SerializeField] float nowTime = 0f;
    [SerializeField] bool endFlag = false;

    [Header("UI")]
    [SerializeField] private Sprite startSprite;
    [SerializeField] private Sprite endSprite;
    [SerializeField] private Sprite defSprite;


    private GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Image");
        canvas.GetComponent<Image>().sprite = startSprite;
        endTime = 5f;
        endFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        ErrorCheck();
        nowTime += Time.deltaTime;

        if( endFlag && nowTime >= endTime )
        {
            canvas.GetComponent<Image>().sprite = defSprite;
            endTime = 60f;
            nowTime = 0f;
            endFlag = false;
        }
        else if( nowTime >= endTime )
        {
            canvas.GetComponent<Image>().sprite = endSprite;
            endTime = 5f;
            nowTime = 0f;
            endFlag = true;
        }
    }

    void ErrorCheck()
    {
        if( defSprite == null )
        {
            Debug.Log("defSprite‚ªNULL‚Å‚·");
            return;
        }
        if (startSprite == null)
        {
            Debug.Log("startSprite‚ªNULL‚Å‚·");
            return;
        }
        if (endSprite == null)
        {
            Debug.Log("endSprite‚ªNULL‚Å‚·");
            return;
        }
    }
}
