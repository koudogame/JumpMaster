//
//  タイトルの進行管理
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleDirector : MonoBehaviour
{
    //************************** 入力情報 **************************//

    PlayerOperation playerOption;    // アクションマップ


    //************************** Fade **************************//

    [Header("フェード処理script"), SerializeField] private Fade fade;



    //************************** カーソル移動 **************************//

    [Header("枠オブジェクト"), SerializeField] private RectTransform frame;           // 枠の座標(rect)
    [Header("枠の座標(上)"), SerializeField] private RectTransform up_limit_obj;     //  カーソル初期位置(上)に設定するobj
    [Header("枠の座標(下)"), SerializeField] private RectTransform down_limit_obj;   //  カーソル初期位置(下)に設定するobj
    private Vector3 up_limit_pos;   //  カーソル初期Y(上)
    private Vector3 down_limit_pos; //  カーソル初期Y(下)
    private float move_value;       //  カーソル移動一回分の増減値


    //************************** 選択肢関連 **************************//

    [Header("シーンリスト"), SerializeField] private List<string> scenes; // 選択肢のリスト
    public static int select_num;       //  操作中カーソル番号
    public static int next_num;         //  決定したカーソル番号
    public static bool isNext;          //  シーン移行したか

    //************************** 非アクティブ **************************//

    [Header("非アクティブOPTION"), SerializeField] private GameObject option;
    [Header("非アクティブCREDIT"), SerializeField] private GameObject credit;



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
        select_num = 0; //  現在の番号
        next_num = -1;  //  決定した番号
        isNext = false; //  次のシーンへ移行するフラグ管理

        //  枠の初期座標を設定
        up_limit_pos = up_limit_obj.localPosition;
        down_limit_pos = down_limit_obj.localPosition;

        //  カーソルの増減値を設定
        move_value = Mathf.Abs(down_limit_pos.y) / (float)GetSelectMax();

        //  fadeinスタート
        StartCoroutine(fade.FadeIn());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameEndCheck(); // ゲームが終了するかのチェック
    }



    //*****************************************************
    //  決定
    //*****************************************************
    private void OnEnter(InputAction.CallbackContext context)
    {
        //  一度も移行していないか or 入力された瞬間以外なら処理しない
        if (isNext || !context.started) return;

        // シーン番号を決定する
        next_num = select_num;

        //  次のシーンへ移行
        if (next_num == 0) Next();                // シーン移行処理
        else if (next_num == 1)
        {
            StartCoroutine(fade.HalfFadeOut()); // フェードアウト
            Invoke("OptionProcess", 1); // fadeOutしてから表示
        }
        else if (next_num == 2)
        {
            StartCoroutine(fade.HalfFadeOut()); // フェードアウト
            Invoke("CreditProcess", 1); // fadeOutしてから表示
        }
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
        if (select_num >= scenes.Count) select_num = 0;
        else if (select_num < 0) select_num = GetSelectMax();

        //  枠移動, Y のみ 初期Y + (カーソル一回分の移動量 * カーソル番号)
        var prev_pos = frame.localPosition;
        frame.localPosition = new Vector3(
            prev_pos.x, up_limit_pos.y - (move_value * select_num), prev_pos.z
            );
    }



    //*****************************************************
    //  ゲーム終了か
    //*****************************************************
    private void GameEndCheck()
    {
        //  フェード中は処理しない
        if (isNext) return;

        // 終了が選択されていたら
        if (next_num == GetSelectMax())
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;    // ゲームの処理を中止
#else
            Application.Quit();     // ゲームの終了
#endif
        }
    }




    //*****************************************************
    //  次のシーンへ移行(移行できるか判定を含む)
    //*****************************************************
    private void Next()
    {
        //  一度も移行していないか or ロードしない番号の範囲か
        if (isNext || next_num >= 1) return;

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
    private int GetSelectMax() { return scenes.Count - 1; }



    //*****************************************************
    // オプション処理
    //*****************************************************
    private void OptionProcess()
    {
        isNext = true;                                  // シーン移行フラグをON
        option.SetActive(true);                         // アクティブ化
        GetComponent<SoundSetting>().enabled = true;    // オプションスクリプトの有効化
        SoundSetting.CurrentMoveFlag = true;            // サウンドスクリプトのカーソルフラグを有効化
        GetComponent<TitleDirector>().enabled = false;    // タイトルスクリプト無効化
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
        option.SetActive(false);                        // オプションを非アクティブ化
        GetComponent<TitleDirector>().enabled = true;     // タイトルスクリプトの有効化
        GetComponent<SoundSetting>().enabled = false;   // オプションスクリプトの無効化
    }



    //*****************************************************
    // クレジット処理
    //*****************************************************
    private void CreditProcess()
    {
        isNext = true;                      // シーン移行フラグをON
        StartCoroutine(fade.HalfFadeOut()); // フェードアウト
        credit.SetActive(true);             // アクティブ化
    }




    //*****************************************************
    // シーンのロード
    //*****************************************************
    private void LoadScene()
    {
        //  フェードが終わったらロード
        SceneManager.LoadScene(scenes[next_num]);
    }
}
