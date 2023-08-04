//
//  ゲームの進行管理
//
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [Header("TimeLimit")]
    [SerializeField] float endTime = 0f;
    [SerializeField] float nowTime = 60f;
    [SerializeField] bool startFlag = false;
    [SerializeField] bool unLimitedFlag = false;

    [Header("Score")]
    [SerializeField] int score = 0;

    [Header("UI")]
    [SerializeField] private Fade fade;
    [SerializeField] private Sprite startSprite;
    [SerializeField] private Sprite endSprite;
    [SerializeField] private Sprite defSprite;
    [SerializeField] private Sprite[] countSprite = new Sprite[10];

    [Header("Mode&Level")]
    [SerializeField] private string mode = "Standard";
    [SerializeField] private string level = "Easy";
    //[SerializeField] private string[] modeNames = new string[2];
    //[SerializeField] private string[] levelNames = new string[3];

    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private bool wasGrounded;
    [SerializeField] private float hitTrampolinePosY;


    private GameObject startAndEndUI;
    private GameObject counter1UI;
    private GameObject counter2UI;
    private GameObject countdown1UI;
    private GameObject countdown2UI;
    private bool isClear;            //  クリアしたか

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
        //mode = SelectModeSingleton.Instance.GetMode();
        //level = SelectModeSingleton.Instance.GetLevel();
        if (SelectModeSingleton.Instance != null)
        {
            mode = SelectModeSingleton.Instance.GetMode();
            level = SelectModeSingleton.Instance.GetLevel();
        }
        else
        {
            mode = "Standard";
            level = "Easy";
        }
        nowTime = 5f;
        endTime = 0f;
        hitTrampolinePosY = 0f;
        startFlag = true;
        isClear = false;
        wasGrounded = true;

        // エラーチェック
        if (startAndEndUI == null)
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

        // フェードイン
        StartCoroutine(fade.FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        ErrorCheck();

        bool EndFlag = false;
        if (unLimitedFlag)
        {
            if (player.GetComponent<PlayerMove>().GetIsGrounded()
                && !wasGrounded)
            {
                if (player.transform.position.y < hitTrampolinePosY) EndFlag = true;
            }
            if (EndFlag) Debug.Log(EndFlag);
        }

        ///- 制限時間処理
        // 制限時間カウント処理と、開始および終了合図のUI処理
        if( !unLimitedFlag )nowTime -= Time.deltaTime;
        if( nowTime < 0f ) nowTime = 0f;

        if( startFlag )
        {
            // スタート前のカウントダウン処理
            if(nowTime <= 1f) startAndEndUI.GetComponent<Image>().sprite = startSprite;
            else if(nowTime <= 2f) startAndEndUI.GetComponent<Image>().sprite = countSprite[1];
            else if(nowTime <= 3f) startAndEndUI.GetComponent<Image>().sprite = countSprite[2];
            else if(nowTime <= 4f) startAndEndUI.GetComponent<Image>().sprite = countSprite[3];
        }

        if ( startFlag && nowTime <= endTime )
        {
            // スタート時の処理
            ModeSet();
            LevelSet();
            startAndEndUI.GetComponent<Image>().sprite = defSprite;
            startFlag = false;
        }
        else if( ( ( nowTime <= endTime && !unLimitedFlag ) || ( unLimitedFlag && EndFlag ) ) && !isClear )
        {
            // クリア後の処理
            startAndEndUI.GetComponent<Image>().sprite = endSprite;
            //StartCoroutine(/*fade.StartFade(0.0f, 1.0f)*/fade.FadeOut());
            //new WaitForSeconds(12);
            //StartCoroutine(/*fade.StartFade(0.0f, 1.0f)*/fade.FadeOut());
            endTime = 0f;
            nowTime = 3f;
            unLimitedFlag = false;
            isClear = true;
        }
        else if( nowTime <= endTime && isClear )
        {
            StartCoroutine(/*fade.StartFade(0.0f, 1.0f)*/fade.FadeOut());
            isClear = false;
            endTime = 0f;
            nowTime = 0f;
            GameScoreSingleton.Instance.Score = score;
            GameScoreSingleton.Instance.Time = (int)nowTime;
            SceneManager.LoadScene("Result");
        }

        // 制限時間UIの処理
        if ( !startFlag && !isClear )
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
        // 
        if (!startFlag && !isClear)
        {
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


        if (unLimitedFlag)
        {
            // 接地判定の履歴を保存
            wasGrounded = player.GetComponent<PlayerMove>().GetWasGrounded();
        }
    }

    private void FixedUpdate()
    {
        ////  クリア
        //if (isClear)
        //{
        //    StartCoroutine(Clear());

        //    //  クリアしたらゲームオーバーを無効にする
        //    return;
        //}
        //if (nowTime <= endTime && isClear)
        //{
        //    //StartCoroutine(Clear());
        //    // フェードアウト
        //    //StartCoroutine(fade.FadeOut());
        //    StartCoroutine(fade.StartFade(0.0f, 1.0f));
        //    //  リザルトシーンへ移行
        //    SceneManager.LoadScene("Result");
        //}
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

    void ModeSet()
    {
        if (mode == null) return;
        else
        {
            if(mode == "Standard")
            {
                // スタート時の処理
                endTime = 0f;
                nowTime = 60f;
            }
            else if(mode == "Sudden Death")
            {
                // スタート時の処理
                endTime = 99f;
                nowTime = 99f;
                unLimitedFlag = true;
            }
        }
    }

    void LevelSet()
    {
        if (level == null) return;
        else if (mode != "Sudden Death")
        {
            if (level == "Easy")
            {
                // スタート時の処理
                endTime = 0f;
                nowTime = 60f;
            }
            else if (level == "Normal")
            {
                // スタート時の処理
                endTime = 0f;
                nowTime = 40f;
            }
            else if (level == "Hard")
            {
                // スタート時の処理
                endTime = 0f;
                nowTime = 20f;
            }
        }
    }

    //  クリア
    private IEnumerator Clear()
    {
        //_eventText.text = "ステージクリア!!";
        //_eventText.color = Color.yellow;

        //  2s待つ
        //yield return new WaitForSeconds(2);

        // フェードアウト
        StartCoroutine(fade.FadeOut());

        ////  2s待つ
        yield return new WaitForSeconds(2);

        //  リザルトシーンへ移行
        SceneManager.LoadScene("Result");
    }

    ////  クリアを知らせてもらう
    //public void CallClear() { isClear = true; }

    public void ScoreCount() { if( !isClear ) ++score; }

    public void SetHitTrampolinePosY( float HitPosY ) { hitTrampolinePosY = HitPosY; }

    bool GetStartFlag() { return startFlag; }
}