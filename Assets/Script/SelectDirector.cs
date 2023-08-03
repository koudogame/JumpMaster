//
//  セレクトの進行管理
//
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    [SerializeField] private GameObject modeUIParent;
    [SerializeField] private GameObject levelUIParent;
    [SerializeField] private RectTransform[] modeUIRectTrans = new RectTransform[2];
    [SerializeField] private RectTransform[] levelUIRectTrans = new RectTransform[3];


    //************************** 入力情報 **************************//

    PlayerOperation playerOption;    // アクションマップ


    //************************** カーソル移動 **************************//

    [Header("枠オブジェクト"), SerializeField] private RectTransform frame;           // 枠の座標(rect)
    [Header("枠の座標(上)"), SerializeField] private RectTransform up_limit_obj;     //  カーソル初期位置(上)に設定するobj
    [Header("枠の座標(下)"), SerializeField] private RectTransform down_limit_obj;   //  カーソル初期位置(下)に設定するobj
    private Vector3 up_limit_pos;   //  カーソル初期Y(上)
    private Vector3 down_limit_pos; //  カーソル初期Y(下)
    private float move_value;       //  カーソル移動一回分の増減値


    //************************** 選択肢関連 **************************//

    //[Header("現在ページの選択肢"), SerializeField] private List<GameObject> choices; // 選択肢のリスト
    [Header("現在ページの選択肢"), SerializeField] private GameObject[] choices; // 選択肢のリスト
    [Header("セレクトシーンのページ総数"), SerializeField] private int pageTotal;
    [Header("セレクトシーンの現在ページ"), SerializeField] private int pageNum;
    public static int select_num;       //  操作中カーソル番号
    public static int next_num;         //  決定したカーソル番号
    public static bool isNext;          //  ページ移行したか


    //************************** 非アクティブ **************************//

    [Header("非アクティブOPTION"), SerializeField] private GameObject option;


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
        selectUI.GetComponent<Image>().sprite = selectSprite;

        for (int i = 0; i < levelUI.Length; ++i)
        {
            levelUI[i].GetComponent<Image>().sprite = levelSprite[i];
        }

        for (int i = 0; i < modeUI.Length; ++i)
        {
            modeUI[i].GetComponent <Image>().sprite = modeSprite[i];
        }

        modeUIParent.SetActive(true);
        levelUIParent.SetActive(false);

        choices = new GameObject[modeUI.Length];
        choices[0] = modeUI[0];
        choices[1] = modeUI[1];
        //choices = new List<GameObject> { modeUI[0], modeUI[1] };

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


    //*****************************************************
    //  決定
    //*****************************************************
    private void OnEnter(InputAction.CallbackContext context)
    {
        //  一度も移行していないか or 入力された瞬間以外なら処理しない
        if (isNext || !context.started) return;

        // 選択番号を決定する
        next_num = select_num;

        //  次のページ、またはシーンへ移行
        if(pageNum == 0)
        {
            SelectModeSingleton.Instance.SetMode(choices[next_num].name);

            if(next_num == 0)
            {
                choices = null;
                choices = new GameObject[levelUI.Length];
                for (int i = 0; i < levelUI.Length; ++i)
                {
                    choices[i] = levelUI[i];
                }
            }
            else if(next_num == 1)
            {
                choices = null;
                choices = new GameObject[levelUI.Length];
                for (int i = 0; i < levelUI.Length; ++i)
                {
                    choices[i] = levelUI[i];
                }
            }

            //  枠の初期座標を設定
            up_limit_obj = levelUIRectTrans[0];
            down_limit_obj = levelUIRectTrans[1];
            up_limit_pos = up_limit_obj.localPosition;
            down_limit_pos = down_limit_obj.localPosition;
            var prev_pos = frame.localPosition;
            frame.localPosition = new Vector3(
                prev_pos.x, up_limit_pos.y, prev_pos.z);

            //  カーソルの増減値を設定
            move_value = up_limit_pos.y - down_limit_pos.y;

            modeUIParent.SetActive(false);
            levelUIParent.SetActive(true);
            pageNum = 1;
        }
        else if(pageNum == 1)
        {
            SelectModeSingleton.Instance.SetLevel(choices[next_num].name);
            Next();
        }
        //if (next_num == 0) Next();                // シーン移行処理
        //else if (next_num == 1)
        //{
        //    StartCoroutine(fade.HalfFadeOut()); // フェードアウト
        //    Invoke("OptionProcess", 1); // fadeOutしてから表示
        //}
        //else if (next_num == 2)
        //{
        //    StartCoroutine(fade.HalfFadeOut()); // フェードアウト
        //    Invoke("CreditProcess", 1); // fadeOutしてから表示
        //}
    }

    //*****************************************************
    //  カーソル移動
    //*****************************************************
    private void OnMoveCursol(InputAction.CallbackContext context)
    {
        //  フェード中は処理しない or 入力された瞬間以外なら処理しない
        if (isNext || !context.started) return;

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
        SceneManager.LoadScene("Game");
    }

    void ErrorCheck()
    {
        if (modeUIParent == null)
        {
            Debug.Log("modeUIParentがNULLです");
            return;
        }

        if (levelUIParent == null)
        {
            Debug.Log("levelUIParentがNULLです");
            return;
        }

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

        for (int i = 0; i < levelUIRectTrans.Length; ++i)
        {
            if (levelUIRectTrans[i] == null)
            {
                Debug.Log("levelUIRectTrans[ " + i + " ]がNULLです");
                return;
            }
        }

        for (int i = 0; i < modeUIRectTrans.Length; ++i)
        {
            if (modeUIRectTrans[i] == null)
            {
                Debug.Log("modeUIRectTrans[ " + i + " ]がNULLです");
                return;
            }
        }
    }
}
