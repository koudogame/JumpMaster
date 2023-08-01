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
    [SerializeField] private Sprite[] modeSprite = new Sprite[2];
    [SerializeField] private Sprite[] levelSprite = new Sprite[3];
    [SerializeField] private GameObject selectUI;
    [SerializeField] private GameObject[] modeUI = new GameObject[2];
    [SerializeField] private GameObject[] levelUI = new GameObject[3];


    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        selectUI.GetComponent<Image>().sprite = selectSprite;

        for (int i = 0; i < levelUI.Length; ++i)
        {
            levelUI[i].GetComponent<Image>().sprite = levelSprite[i];
        }

        for (int i = 0; i < modeUI.Length; ++i)
        {
            modeUI[i].GetComponent <Image>().sprite = modeSprite[i];
        }

        // エラーチェック
        ErrorCheck();

        // フェードイン
        StartCoroutine(fade.FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        ErrorCheck();
    }

    void ErrorCheck()
    {
        if (selectUI == null)
        {
            Debug.Log("selectUIがNULLです");
            return;
        }
        if (selectSprite == null)
        {
            Debug.Log("selectSpriteがNULLです");
            return;
        }


        for (int i = 0; i < levelSprite.Length; ++i)
        {
            if (levelSprite[i] == null)
            {
                Debug.Log("levelSprite[ " + i + " ]がNULLです");
                return;
            }
        }

        for (int i = 0; i < modeSprite.Length; ++i)
        {
            if (modeSprite[i] == null)
            {
                Debug.Log("modeSprite[ " + i + " ]がNULLです");
                return;
            }
        }

        for (int i = 0; i < levelUI.Length; ++i)
        {
            if (levelUI[i] == null)
            {
                Debug.Log("levelUI[ " + i + " ]がNULLです");
                return;
            }
        }

        for (int i = 0; i < modeUI.Length; ++i)
        {
            if (modeUI[i] == null)
            {
                Debug.Log("modeUI[ " + i + " ]がNULLです");
                return;
            }
        }
    }
}
