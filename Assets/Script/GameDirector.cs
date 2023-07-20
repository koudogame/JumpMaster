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
    [SerializeField] bool startFlag = false;

    [Header("Score")]
    [SerializeField] int score = 0;

    [Header("UI")]
    [SerializeField] private Sprite startSprite;
    [SerializeField] private Sprite endSprite;
    [SerializeField] private Sprite defSprite;
    [SerializeField] private Sprite[] countSprite = new Sprite[10];


    private GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Start&End");
        canvas.GetComponent<Image>().sprite = defSprite;
        endTime = 5f;
        startFlag = true;

        for( int i = 0; i < countSprite.Length; ++i )
        {
            if (countSprite[ i ] == null)
            {
                Debug.Log("countSprite[ " + i  + " ]‚ªNULL‚Å‚·");
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ErrorCheck();
        nowTime += Time.deltaTime;

        if( startFlag )
        {
            //switch( nowTime )
            //{
            //    case 1f: canvas.GetComponent<Image>().sprite = countSprite[ 3 ]; break;
            //    case 2f: canvas.GetComponent<Image>().sprite = countSprite[ 2 ]; break;
            //    case 3f: canvas.GetComponent<Image>().sprite = countSprite[ 1 ]; break;
            //    case 4f: canvas.GetComponent<Image>().sprite = startSprite;      break;
            //}
            if(nowTime >= 4f) canvas.GetComponent<Image>().sprite = startSprite;
            else if(nowTime >= 3f) canvas.GetComponent<Image>().sprite = countSprite[1];
            else if(nowTime >= 2f) canvas.GetComponent<Image>().sprite = countSprite[2];
            else if(nowTime >= 1f) canvas.GetComponent<Image>().sprite = countSprite[3];
        }

        if( startFlag && nowTime >= endTime )
        {
            canvas.GetComponent<Image>().sprite = defSprite;
            endTime = 60f;
            nowTime = 0f;
            startFlag = false;
        }
        else if( nowTime >= endTime )
        {
            canvas.GetComponent<Image>().sprite = endSprite;
            endTime = 5f;
            nowTime = 0f;
            startFlag = true;
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

    public void ScoreCount() { ++score; }

    bool GetStartFlag() { return startFlag; }
}