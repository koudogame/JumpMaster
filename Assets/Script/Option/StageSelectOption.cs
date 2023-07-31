using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StageSelectOption : MonoBehaviour
{
    private enum SceneNameOption
    {
        STAGESELECT,
        GAME
    }
    [SerializeField] private SceneNameOption SceneName;

    //************************** 入力情報 **************************//

    PlayerOperation playerOption;

    //************************** Fade **************************//

    [Header("フェード処理script"), SerializeField] private Fade fade;


    //************************** カーソル移動 **************************//

    [Header("カーソル番号"), SerializeField] private int cursol_num;                    // 現在の選択肢番号
    [Header("カーソルリスト"), SerializeField] private List<string> OptionList;        // 選択肢のリスト
    [Header("カーソル矢印オブジェクト"), SerializeField] private RectTransform Arrow;  // 矢印の座標
    [Header("矢印の座標(上)"), SerializeField] private RectTransform upLimit;          // 矢印上限位置に設定するObj 
    [Header("矢印の座標(下)"), SerializeField] private RectTransform downLimit;         // 矢印下限位置に設定するObj
    private Vector3 upLimitPos;     // カーソル上限位置Y
    private int currentNum;         // 決定時の番号
    public static bool CurrentMoveFlag; // カーソル移動可能フラグ


    //************************** 非アクティブオブジェクト **************************//

    [Header("非アクティブOPTION"), SerializeField] private GameObject option;
    [Header("非アクティブSOUND"), SerializeField] private GameObject sound;
    [Header("非アクティブTUTORIALSELECT"),SerializeField] private GameObject tutorialselect;



    private void Awake()
    {
        // アクションマップのインスタンス生成
        playerOption = new PlayerOperation();

        // アクション実行時呼び出されるメソッドを登録
        playerOption.Option.Enter.started += context => OnEnter(context);
        playerOption.Option.MoveCursol.started += context => OnMoveCursol(context);
        playerOption.Option.Menu.started += context => OnMenu(context);
        playerOption.Option.Cansel.started += context => OnCansel(context);
    }

    private void OnEnable()
    {
        // オブジェクト有効時動作
        playerOption.Enable();
    }

    private void OnDisable()
    {
        // オブジェクト無効時停止
        playerOption.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        cursol_num = 0;                             // 選択肢の番号
        upLimitPos = upLimit.localPosition;         // 上限座標
        currentNum = -1;            // 決定番号初期化
        CurrentMoveFlag = false;    // 移動不可


        StartCoroutine(fade.HalfFadeIn());  // フェードイン
    }




    //*****************************************************
    //  決定
    //*****************************************************
    private void OnEnter(InputAction.CallbackContext context)
    {
        // カーソル移動フラグが無効なら処理しない
        if (!CurrentMoveFlag || !context.started) return;

        currentNum = cursol_num;    // 決定時の番号を取得

        if (currentNum == 1)
        {
            CurrentMoveFlag = false;                            // カーソル移動制限
            sound.SetActive(true);                              // サウンド画面をアクティブ化
            GetComponent<SoundSetting>().enabled = true;        // オプションのチェックボックスを有効化する
            SoundSetting.CurrentMoveFlag = true;                // サウンドのカーソル移動フラグをON
            option.SetActive(false);                            // オプション画面を非アクティブ化
            GetComponent<StageSelectOption>().enabled = false;  // チュートリアルのオプションのチェックボックスを無効化する
        }
        else if (currentNum == 1)
        {
            CurrentMoveFlag = false;                            // カーソル移動フラグOFF
            tutorialselect.SetActive(true);                     // チュートリアルセレクトオブジェクトをアクティブ化
            GetComponent<TutorialSelect>().enabled = true;      // チュートリアルセレクトのスクリプトを有効化
            TutorialSelect.CurrentMoveFlag = true;              // チュートリアルセレクトのカーソル移動フラグをON
            option.SetActive(false);                            // オプションを非アクティブ化
            GetComponent <StageSelectOption>().enabled = false; // ステージセレクトオプションスクリプトを無効化
        }
        else if(  currentNum == 2 && SceneName == SceneNameOption.GAME)
        {
            Reset();                                    // 各項目初期化
            StartCoroutine(fade.FadeOut());             // フェードアウト
            SceneManager.LoadScene("StageSelectScene"); // シーンをロード
        }

        // 戻るを選択していたら
        if (currentNum == GetSelectMax())
        {
            Reset();    // 各要素初期化
            return;
        }
    }




    //*****************************************************
    //  カーソル移動
    //*****************************************************
    private void OnMoveCursol(InputAction.CallbackContext context)
    {
        // 押された瞬間以外なら処理しない
        if (!context.started) return;

        // カーソル移動フラグがONなら
        if (CurrentMoveFlag)
        {
            // 入力値取得
            float value = context.ReadValue<float>();   

            // 0.～の値を1,-1に変換する
            if (value > 0.0f) value = -1.0f;
            else if (value < 0.0f) value = 1.0f;

            // カーソルカウントを進める
            cursol_num += (int)value;   

            // カーソルの番号補正
            if (cursol_num >= OptionList.Count) cursol_num = 0;
            else if (cursol_num < 0) cursol_num = GetSelectMax();

            //  枠移動, Y のみ 初期Y + (カーソル一回分の移動量 * カーソル番号)
            var prev_pos = Arrow.localPosition;
            Arrow.localPosition = new Vector3(prev_pos.x, upLimitPos.y - (20 * cursol_num), prev_pos.z);
        }
    }




    //*****************************************************
    // キャンセル処理
    //*****************************************************
    private void OnCansel(InputAction.CallbackContext context)
    {
        // 押された瞬間以外なら処理しない
        if (!context.started) return;

        // キャンセルを押されたときサウンド画面がアクティブ化状態のとき
        // サウンド画面を非アクティブ化させる
        if (sound.activeSelf)
        {
            option.SetActive(true); // オプション画面をアクティブ化
            GetComponent<SoundSetting>().enabled = false; // オプションのチェックボックスを無効化する
        }

        // カーソル移動フラグがtrueなら
        if (CurrentMoveFlag)
        {
            Reset();    // 各要素初期化
            return;
        }

        // オプションがアクティブのときのみフラグをON
        if( option.activeSelf ) CurrentMoveFlag = true;   // カーソル移動フラグをONにする
    }




    //*****************************************************
    // メニュー処理
    //*****************************************************
    private void OnMenu(InputAction.CallbackContext context)
    {
        // 押された瞬間以外なら処理しない
        if (!context.started) return;

        // アクティブ化状態でもう一度メニューが押されたら
        // 非アクティブ化にする
        if (option.activeSelf)
        {
            ReverseFlag(SceneName);     // ステージセレクトスクリプトの入力を受け取るかの設定
            option.SetActive(false);    // オプションを非アクティブ化
            return;
        }
        else option.SetActive(true);    // オプションをアクティブ化

        StageSelectScript.InputReceive = false; // 入力受取フラグをOFF
        CurrentMoveFlag = true;                 // カーソル移動フラグをON
    }




    //*****************************************************
    // 選択肢最大数取得 
    //*****************************************************
    private int GetSelectMax()
    {
        return OptionList.Count - 1;
    }




    //*****************************************************
    // リセット
    //*****************************************************
    private void Reset()
    {
        cursol_num = 0;                 // カーソル番号を初期化
        currentNum = -1;                // 決定番号を初期化
        CurrentMoveFlag = false;        // 移動フラグをOFFにする
        option.SetActive(false);        // 非アクティブ化
        ReverseFlag(SceneName);         // ステージセレクトスクリプトの入力受取するか設定


        // 矢印の位置を初期位置に指定
        if (SceneName == SceneNameOption.STAGESELECT)
        {
            // 矢印の座標を初期位置に戻す
            Vector3 ArrowPos = new Vector3(-40.0f, 25.0f, 0.0f);
            Arrow.localPosition = ArrowPos;
        }
        else
        {
            Vector3 NewPos = new Vector3(-40.0f, 35.0f, 0.0f);
            Arrow.localPosition = NewPos;
        }
    }




    //*****************************************************
    // 入力受取フラグの設定
    //*****************************************************
    private void ReverseFlag(SceneNameOption SceneName)
    {
        // シーンの名前がステージセレクト以外なら処理しない
        if (SceneName != SceneNameOption.STAGESELECT) return;

        StageSelectScript.InputReceive = true;  // 入力受取フラグをON
    }
}

