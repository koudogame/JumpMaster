//
//  リザルトの進行管理
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class ResultDirector : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] int score = 0;
    [SerializeField] int time = 0;

    [Header("UI")]
    [SerializeField] private Fade fade;
    [SerializeField] private Sprite resultSprite;
    [SerializeField] private Sprite scoreSprite;
    [SerializeField] private Sprite timeSprite;
    [SerializeField] private Sprite[] countSprite = new Sprite[10];

    [Header("Player")]
    [SerializeField] private PlayerAnimation playerAnim;

    [Header("Sound")]
    [SerializeField] private SoundPlayParts bgm;
    [SerializeField] private SoundPlayParts moveSE;
    [SerializeField] private SoundPlayParts pushSE;


    //************************** 入力情報 **************************//

    PlayerOperation playerOption;    // アクションマップ


    //************************** カーソル移動 **************************//

    [Header("枠オブジェクト"), SerializeField] private RectTransform frame;           // 枠の座標(rect)
    [Header("枠の座標(上)"), SerializeField] private RectTransform up_limit_obj;     //  カーソル初期位置(上)に設定するobj
    [Header("枠の座標(下)"), SerializeField] private RectTransform down_limit_obj;   //  カーソル初期位置(下)に設定するobj
    [Header("カーソル移動量"), SerializeField] private float move_value;
    private Vector3 up_limit_pos;   //  カーソル初期Y(上)
    private Vector3 down_limit_pos; //  カーソル初期Y(下)
    //private float move_value;       //  カーソル移動一回分の増減値


    //************************** 選択肢関連 **************************//

    //[Header("現在ページの選択肢"), SerializeField] private List<GameObject> choices; // 選択肢のリスト
    [Header("現在ページの選択肢"), SerializeField] private GameObject[] choices; // 選択肢のリスト
    [Header("選択したシーンの名前"), SerializeField] private string sceneName = "";
    public static int select_num;       //  操作中カーソル番号
    public static int next_num;         //  決定したカーソル番号
    public static bool isNext;          //  ページ移行したか


    //************************** 非アクティブ **************************//

    [Header("非アクティブOPTION"), SerializeField] private GameObject option;


    private GameObject resultUI;
    private GameObject counter1UI;
    private GameObject counter2UI;
    private GameObject countdown1UI;
    private GameObject countdown2UI;


    private void Awake()
    {
        // アクションマップのインスタンス生成
        playerOption = new PlayerOperation();

        // アクションが実行されたときに呼び出されるメソッドを登録
        playerOption.Option.Enter.started += context => OnEnter(context);
        playerOption.Option.MoveCursol.started += context => OnMoveCursol(context);
        playerOption.Option.Cansel.started += context => OnCansel(context);
    }

    private void OnEnable()
    {
        // オブジェクトが有効になった時動作
        playerOption.Enable();
    }

    private void OnDisable()
    {
        // オブジェクトが無効になった時停止
        playerOption.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        resultUI = GameObject.Find("ResultUI");
        counter1UI = GameObject.Find("Counter1");
        counter2UI = GameObject.Find("Counter2");
        countdown1UI = GameObject.Find("Countdown1");
        countdown2UI = GameObject.Find("Countdown2");
        //sceneSetter = GetComponent<ScoreGetter>();

        resultUI.GetComponent<Image>().sprite = resultSprite;
        score = GameScoreSingleton.Instance.GetScore();
        time = GameScoreSingleton.Instance.GetTime();

        // エラーチェック
        if (resultUI == null)
        {
            Debug.Log("resultUIがNULLです");
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

        if (playerAnim == null)
        {
            Debug.Log("playerAnimがNULLです");
            return;
        }

        ErrorCheck();


        ///- クリア時間処理
        // クリア時間UIの十の位の処理
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
        // クリア時間UIの一の位の処理
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

        select_num = 0; //  現在の番号
        next_num = -1;  //  決定した番号
        isNext = false; //  次のシーンへ移行するフラグ管理

        //  枠の初期座標を設定
        up_limit_pos = up_limit_obj.localPosition;
        down_limit_pos = down_limit_obj.localPosition;
        var prev_pos = frame.localPosition;
        frame.localPosition = new Vector3(
            prev_pos.x, up_limit_pos.y, prev_pos.z);

        //  カーソルの増減値を設定
        //move_value = Mathf.Abs(down_limit_pos.y) / (float)GetSelectMax();
        move_value = up_limit_pos.y - down_limit_pos.y;

        playerAnim.SetResultFlag(true);
        bgm.PlayBGM();

        // フェードイン
        StartCoroutine(fade.FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        ErrorCheck();
    }

    //*****************************************************
    //  決定
    //*****************************************************
    private void OnEnter(InputAction.CallbackContext context)
    {
        //  一度も移行していないか or 入力された瞬間以外なら処理しない
        if (isNext || !context.started) return;

        pushSE.PlaySE();

        // 選択番号を決定する
        next_num = select_num;

        //  次のページ、またはシーンへ移行
        if (next_num == 0)
        {
            StartCoroutine(fade.HalfFadeOut()); // フェードアウト
            sceneName = "Game";
            Next();                // シーン移行処理
        }
        else if (next_num == 1)
        {
            StartCoroutine(fade.HalfFadeOut()); // フェードアウト
            sceneName = "Select";
            Next();                // シーン移行処理
        }
        else if (next_num == 2)
        {
            StartCoroutine(fade.HalfFadeOut()); // フェードアウト
            sceneName = "Title";
            Next();                // シーン移行処理
        }
    }

    //*****************************************************
    //  カーソル移動
    //*****************************************************
    private void OnMoveCursol(InputAction.CallbackContext context)
    {
        //  フェード中は処理しない or 入力された瞬間以外なら処理しない
        if (isNext || !context.started) return;

        moveSE.PlaySE();

        // 入力値を取得
        float counter = context.ReadValue<float>();

        // 0.～の値を1,-1に変換する
        if (counter > 0.0f) counter = -1.0f;
        else if (counter < 0.0f) counter = 1.0f;

        select_num += (int)counter; // 現在選択している選択肢の番号

        //  カーソル番号補正
        //if (select_num >= choices.Count) select_num = 0;
        if (select_num >= choices.Length) select_num = 0;
        else if (select_num < 0) select_num = GetSelectMax();

        //  枠移動, Y のみ 初期Y + (カーソル一回分の移動量 * カーソル番号)
        var prev_pos = frame.localPosition;
        frame.localPosition = new Vector3(
            prev_pos.x, up_limit_pos.y - (move_value * select_num), prev_pos.z
            );
    }

    //*****************************************************
    //  次のシーンへ移行(移行できるか判定を含む)
    //*****************************************************
    private void Next()
    {
        //  一度も移行していないか or ロードしない番号の範囲か
        if (isNext /*|| next_num >= 1*/) return;

        //  条件を満たしていたら、シーン移行を実行する
        isNext = true;

        //  フェードアウト
        StartCoroutine(fade.FadeOut());

        // 1秒待ってからロード
        Invoke("LoadScene", 1);
    }

    //*****************************************************
    //  選択肢の最大数取得
    //*****************************************************
    private int GetSelectMax() { return /*choices.Count - 1*/choices.Length - 1; }

    //*****************************************************
    // オプション処理
    //*****************************************************
    private void OptionProcess()
    {
        isNext = true;                                  // シーン移行フラグをON
        option.SetActive(true);                         // アクティブ化
        GetComponent<SoundSetting>().enabled = true;    // オプションスクリプトの有効化
        SoundSetting.CurrentMoveFlag = true;            // サウンドスクリプトのカーソルフラグを有効化
        GetComponent<SelectDirector>().enabled = false;    // タイトルスクリプト無効化
    }

    //*****************************************************
    // キャンセル処理
    //*****************************************************
    private void OnCansel(InputAction.CallbackContext context)
    {
        // 入力された瞬間以外なら処理しない
        if (!context.started) return;

        isNext = false;                                 // シーン移行フラグをOFF
        StartCoroutine(fade.FadeIn());                  // フェードイン
        //option.SetActive(false);                        // オプションを非アクティブ化
        GetComponent<SelectDirector>().enabled = true;     // タイトルスクリプトの有効化
        GetComponent<SoundSetting>().enabled = false;   // オプションスクリプトの無効化
    }

    //*****************************************************
    // シーンのロード
    //*****************************************************
    private void LoadScene()
    {
        //  フェードが終わったらロード
        SceneManager.LoadScene(sceneName);
    }

    void ErrorCheck()
    {
        if (resultSprite == null)
        {
            Debug.Log("resultSpriteがNULLです");
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
