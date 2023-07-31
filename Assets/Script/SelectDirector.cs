//
//  セレクトの進行管理
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDirector : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] int score = 0;

    [Header("UI")]
    [SerializeField] private Fade fade;
    [SerializeField] private Sprite selectSprite;
    [SerializeField] private Sprite scoreSprite;
    [SerializeField] private Sprite timeSprite;
    [SerializeField] private Sprite[] countSprite = new Sprite[10];


    private GameObject selectUI;
    private GameObject counter1UI;
    private GameObject counter2UI;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        selectUI = GameObject.Find("ModeSelect");
        counter1UI = GameObject.Find("Counter1");
        counter2UI = GameObject.Find("Counter2");

        selectUI.GetComponent<Image>().sprite = selectSprite;

        // エラーチェック
        if (selectUI == null)
        {
            Debug.Log("selectUIがNULLです");
            return;
        }

        if (counter1UI == null)
        {
            Debug.Log("counter1UIがNULLです");
            return;
        }

        if (counter2UI == null)
        {
            Debug.Log("counter2UIがNULLです");
            return;
        }

        ErrorCheck();

        // フェードイン
        StartCoroutine(fade.FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        ErrorCheck();

        ///- スコアカウント処理
        // スコアカウンタUIの十の位の処理
        switch (score / 10)
        {
            case 0: counter1UI.GetComponent<Image>().sprite = countSprite[0]; break;
            case 1: counter1UI.GetComponent<Image>().sprite = countSprite[1]; break;
            case 2: counter1UI.GetComponent<Image>().sprite = countSprite[2]; break;
            case 3: counter1UI.GetComponent<Image>().sprite = countSprite[3]; break;
            case 4: counter1UI.GetComponent<Image>().sprite = countSprite[4]; break;
            case 5: counter1UI.GetComponent<Image>().sprite = countSprite[5]; break;
            case 6: counter1UI.GetComponent<Image>().sprite = countSprite[6]; break;
            case 7: counter1UI.GetComponent<Image>().sprite = countSprite[7]; break;
            case 8: counter1UI.GetComponent<Image>().sprite = countSprite[8]; break;
            case 9: counter1UI.GetComponent<Image>().sprite = countSprite[9]; break;
            default: break;
        }
        // スコアカウンタUIの一の位の処理
        switch (score % 10)
        {
            case 0: counter2UI.GetComponent<Image>().sprite = countSprite[0]; break;
            case 1: counter2UI.GetComponent<Image>().sprite = countSprite[1]; break;
            case 2: counter2UI.GetComponent<Image>().sprite = countSprite[2]; break;
            case 3: counter2UI.GetComponent<Image>().sprite = countSprite[3]; break;
            case 4: counter2UI.GetComponent<Image>().sprite = countSprite[4]; break;
            case 5: counter2UI.GetComponent<Image>().sprite = countSprite[5]; break;
            case 6: counter2UI.GetComponent<Image>().sprite = countSprite[6]; break;
            case 7: counter2UI.GetComponent<Image>().sprite = countSprite[7]; break;
            case 8: counter2UI.GetComponent<Image>().sprite = countSprite[8]; break;
            case 9: counter2UI.GetComponent<Image>().sprite = countSprite[9]; break;
            default: break;
        }
    }

    void ErrorCheck()
    {
        if (selectSprite == null)
        {
            Debug.Log("selectSpriteがNULLです");
            return;
        }
        if (scoreSprite == null)
        {
            Debug.Log("scoreSpriteがNULLです");
            return;
        }
        if (timeSprite == null)
        {
            Debug.Log("timeSpriteがNULLです");
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
}
