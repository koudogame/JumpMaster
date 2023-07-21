using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [Header("TimeLimit")]
    [SerializeField] float endTime = 0f;
    [SerializeField] float nowTime = 60f;
    [SerializeField] bool startFlag = false;

    [Header("Score")]
    [SerializeField] int score = 0;

    [Header("UI")]
    [SerializeField] private Sprite startSprite;
    [SerializeField] private Sprite endSprite;
    [SerializeField] private Sprite defSprite;
    [SerializeField] private Sprite[] countSprite = new Sprite[10];


    private GameObject startAndEndUI;
    private GameObject counter1UI;
    private GameObject counter2UI;
    private GameObject countdown1UI;
    private GameObject countdown2UI;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        startAndEndUI = GameObject.Find("Start&End");
        counter1UI = GameObject.Find("Counter1");
        counter2UI = GameObject.Find("Counter2");
        countdown1UI = GameObject.Find("Countdown1");
        countdown2UI = GameObject.Find("Countdown2");

        startAndEndUI.GetComponent<Image>().sprite = defSprite;
        nowTime = 5f;
        endTime = 0f;
        startFlag = true;

        // エラーチェック
        if(startAndEndUI == null)
        {
            Debug.Log("startAndEndUIがNULLです");
            return;
        }

        if(counter1UI == null)
        {
            Debug.Log("counter1UIがNULLです");
            return;
        }

        if(counter2UI == null) 
        {
            Debug.Log("counter2UIがNULLです");
            return;
        }

        if (countdown1UI == null)
        {
            Debug.Log("countdown1UIがNULLです");
            return;
        }

        if (countdown2UI == null)
        {
            Debug.Log("countdown2UIがNULLです");
            return;
        }

        ErrorCheck();
    }

    // Update is called once per frame
    void Update()
    {
        ErrorCheck();

        ///- 制限時間処理
        // 制限時間カウント処理と、開始および終了合図のUI処理
        nowTime -= Time.deltaTime;
        if( nowTime < 0f ) nowTime = 0f;

        if( startFlag )
        {
            //switch( nowTime )
            //{
            //    case 1f: startAndEndUI.GetComponent<Image>().sprite = countSprite[ 3 ]; break;
            //    case 2f: startAndEndUI.GetComponent<Image>().sprite = countSprite[ 2 ]; break;
            //    case 3f: startAndEndUI.GetComponent<Image>().sprite = countSprite[ 1 ]; break;
            //    case 4f: startAndEndUI.GetComponent<Image>().sprite = startSprite;      break;
            //}
            if(nowTime <= 1f) startAndEndUI.GetComponent<Image>().sprite = startSprite;
            else if(nowTime <= 2f) startAndEndUI.GetComponent<Image>().sprite = countSprite[1];
            else if(nowTime <= 3f) startAndEndUI.GetComponent<Image>().sprite = countSprite[2];
            else if(nowTime <= 4f) startAndEndUI.GetComponent<Image>().sprite = countSprite[3];
        }

        if( startFlag && nowTime <= endTime )
        {
            startAndEndUI.GetComponent<Image>().sprite = defSprite;
            endTime = 0f;
            nowTime = 60f;
            startFlag = false;
        }
        else if( nowTime <= endTime )
        {
            startAndEndUI.GetComponent<Image>().sprite = endSprite;
            //endTime = 0f;
            //nowTime = 5f;
            //startFlag = true;
        }
        // 制限時間UIの処理
        if( !startFlag )
        {
            int time = (int)nowTime;
            // 制限時間UIの十の位の処理
            switch (time / 10)
            {
                case 0: countdown1UI.GetComponent<Image>().sprite = countSprite[0]; break;
                case 1: countdown1UI.GetComponent<Image>().sprite = countSprite[1]; break;
                case 2: countdown1UI.GetComponent<Image>().sprite = countSprite[2]; break;
                case 3: countdown1UI.GetComponent<Image>().sprite = countSprite[3]; break;
                case 4: countdown1UI.GetComponent<Image>().sprite = countSprite[4]; break;
                case 5: countdown1UI.GetComponent<Image>().sprite = countSprite[5]; break;
                case 6: countdown1UI.GetComponent<Image>().sprite = countSprite[6]; break;
                case 7: countdown1UI.GetComponent<Image>().sprite = countSprite[7]; break;
                case 8: countdown1UI.GetComponent<Image>().sprite = countSprite[8]; break;
                case 9: countdown1UI.GetComponent<Image>().sprite = countSprite[9]; break;
                default: break;
            }
            // 制限時間UIの一の位の処理
            switch (time % 10)
            {
                case 0: countdown2UI.GetComponent<Image>().sprite = countSprite[0]; break;
                case 1: countdown2UI.GetComponent<Image>().sprite = countSprite[1]; break;
                case 2: countdown2UI.GetComponent<Image>().sprite = countSprite[2]; break;
                case 3: countdown2UI.GetComponent<Image>().sprite = countSprite[3]; break;
                case 4: countdown2UI.GetComponent<Image>().sprite = countSprite[4]; break;
                case 5: countdown2UI.GetComponent<Image>().sprite = countSprite[5]; break;
                case 6: countdown2UI.GetComponent<Image>().sprite = countSprite[6]; break;
                case 7: countdown2UI.GetComponent<Image>().sprite = countSprite[7]; break;
                case 8: countdown2UI.GetComponent<Image>().sprite = countSprite[8]; break;
                case 9: countdown2UI.GetComponent<Image>().sprite = countSprite[9]; break;
                default: break;
            }
        }


        ///- スコアカウント処理
        // スコアカウンタUIの十の位の処理
        switch ( score / 10 )
        {
            case 0: counter1UI.GetComponent<Image>().sprite = countSprite[ 0 ]; break;
            case 1: counter1UI.GetComponent<Image>().sprite = countSprite[ 1 ]; break;
            case 2: counter1UI.GetComponent<Image>().sprite = countSprite[ 2 ]; break;
            case 3: counter1UI.GetComponent<Image>().sprite = countSprite[ 3 ]; break;
            case 4: counter1UI.GetComponent<Image>().sprite = countSprite[ 4 ]; break;
            case 5: counter1UI.GetComponent<Image>().sprite = countSprite[ 5 ]; break;
            case 6: counter1UI.GetComponent<Image>().sprite = countSprite[ 6 ]; break;
            case 7: counter1UI.GetComponent<Image>().sprite = countSprite[ 7 ]; break;
            case 8: counter1UI.GetComponent<Image>().sprite = countSprite[ 8 ]; break;
            case 9: counter1UI.GetComponent<Image>().sprite = countSprite[ 9 ]; break;
            default: break;
        }
        // スコアカウンタUIの一の位の処理
        switch ( score % 10 )
        {
            case 0: counter2UI.GetComponent<Image>().sprite = countSprite[ 0 ]; break;
            case 1: counter2UI.GetComponent<Image>().sprite = countSprite[ 1 ]; break;
            case 2: counter2UI.GetComponent<Image>().sprite = countSprite[ 2 ]; break;
            case 3: counter2UI.GetComponent<Image>().sprite = countSprite[ 3 ]; break;
            case 4: counter2UI.GetComponent<Image>().sprite = countSprite[ 4 ]; break;
            case 5: counter2UI.GetComponent<Image>().sprite = countSprite[ 5 ]; break;
            case 6: counter2UI.GetComponent<Image>().sprite = countSprite[ 6 ]; break;
            case 7: counter2UI.GetComponent<Image>().sprite = countSprite[ 7 ]; break;
            case 8: counter2UI.GetComponent<Image>().sprite = countSprite[ 8 ]; break;
            case 9: counter2UI.GetComponent<Image>().sprite = countSprite[ 9 ]; break;
            default: break;
        }
    }

    void ErrorCheck()
    {
        if( defSprite == null )
        {
            Debug.Log("defSpriteがNULLです");
            return;
        }
        if (startSprite == null)
        {
            Debug.Log("startSpriteがNULLです");
            return;
        }
        if (endSprite == null)
        {
            Debug.Log("endSpriteがNULLです");
            return;
        }

        for (int i = 0; i < countSprite.Length; ++i)
        {
            if (countSprite[i] == null)
            {
                Debug.Log("countSprite[ " + i + " ]がNULLです");
                return;
            }
        }
    }

    public void ScoreCount() { ++score; }

    bool GetStartFlag() { return startFlag; }
}