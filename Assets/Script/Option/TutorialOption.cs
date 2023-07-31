using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialOption : MonoBehaviour
{
    //****************　入力情報　 ****************//

    PlayerOperation playerOption;



    //************************** Fade **************************//

    [Header("フェード処理script"), SerializeField] private Fade fade;



    //****************　非アクティブ　 ****************//

    [Header("非アクティブOPTION"), SerializeField] private GameObject option;
    [Header("非アクティブSOUND"), SerializeField] private GameObject sound;



    //****************　カーソルの情報　 ****************//

    [Header("カーソル番号"), SerializeField] private int cursol_num;                    // 現在の選択肢番号
    [Header("カーソルリスト"), SerializeField] private List<string> OptionList;        // 選択肢のリスト
    [Header("カーソル矢印オブジェクト"), SerializeField] private RectTransform Arrow;  // 矢印の座標
    [Header("矢印の座標(上)"), SerializeField] private RectTransform upLimit;          // 矢印上限位置に設定するObj 
    [Header("矢印の座標(下)"), SerializeField] private RectTransform downLimit;         // 矢印下限位置に設定するObj
    private Vector3 upLimitPos;     // カーソル上限位置Y
    private int currentNum;         // 決定時の番号
    public static bool CurrentMoveFlag; // カーソル移動可能フラグ



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
        // オブジェクトが有効時動作
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
        currentNum = -1;
        CurrentMoveFlag = false;   // 移動可能
    }




    //*****************************************************
    //  決定
    //*****************************************************
    private void OnEnter(InputAction.CallbackContext context)
    {
        // カーソル移動フラグがOFFなら処理しない
        if (!CurrentMoveFlag) return;

        currentNum = cursol_num;    // 決定時の番号を取得

        if( currentNum == 0)
        {
            CurrentMoveFlag = false;                        // カーソル移動制限
            sound.SetActive(true);                          // サウンド画面をアクティブ化
            GetComponent<SoundSetting>().enabled = true;    // オプションのチェックボックスを有効化する
            SoundSetting.CurrentMoveFlag = true;            // サウンドのカーソル移動フラグをON
            option.SetActive(false);                        // オプション画面を非アクティブ化
            GetComponent<TutorialOption>().enabled = false; // チュートリアルのオプションのチェックボックスを無効化する
        }
        else if(  currentNum == 1) 
        {
            StartCoroutine(fade.FadeOut());             // フェードアウト
            SceneManager.LoadScene("StageSelectScene"); // シーンをロード
        }
        // 戻るを選択していたら
        if (currentNum == GetSelectMax())
        {
            Reset();    // 各要素初期化
            option.SetActive(false);        // 非アクティブ
            return;
        }
    }




    //*****************************************************
    //  カーソル移動
    //*****************************************************
    private void OnMoveCursol(InputAction.CallbackContext context)
    {
        // カーソル移動フラグがONなら
        if (CurrentMoveFlag)
        {
            float value = context.ReadValue<float>();   // 入力値取得

            // 0.～の値を1,-1に変換する
            if (value > 0.0f) value = -1.0f;
            else if (value < 0.0f) value = 1.0f;

            cursol_num += (int)value;   // カーソルカウントを進める

            // カーソルの番号補正
            if (cursol_num >= OptionList.Count) cursol_num = 0;
            else if (cursol_num < 0) cursol_num = GetSelectMax();

            //  枠移動, Y のみ 初期Y + (カーソル一回分の移動量 * カーソル番号)
            var prev_pos = Arrow.localPosition;
            Arrow.localPosition = new Vector3(prev_pos.x, upLimitPos.y - (20 * cursol_num), prev_pos.z);
        }
    }





    //*****************************************************
    // メニュー処理
    //*****************************************************
    private void OnMenu(InputAction.CallbackContext context)
    {
        // アクティブ化状態でもう一度メニューが押されたら
        // 非アクティブ化にする
        if (option.activeSelf)
        {
            option.SetActive(false);    // 非アクティブ化
            return;
        }

        option.SetActive(true); // メニューボタンを押されたらアクティブ化させる
        CurrentMoveFlag = true; // カーソル移動フラグをON
    }




    //*****************************************************
    // 選択肢最大数取得  
    //*****************************************************
    private int GetSelectMax()
    {
        return OptionList.Count - 1;
    }




    //*****************************************************
    // キャンセル処理
    //*****************************************************
    private void OnCansel(InputAction.CallbackContext context)
    {
        // キャンセルを押されたときサウンド画面がアクティブ化状態のとき
        // サウンド画面を非アクティブ化させる
        if (sound.activeSelf)
        {
            sound.SetActive(false);                         // サウンド画面を非アクティブ化
            option.SetActive(true);                         // オプション画面をアクティブ化
            GetComponent<SoundSetting>().enabled = false;   // オプションのチェックボックスを無効化する
            GetComponent<TutorialOption>().enabled = true;  // オプションのチェックボックスを無効化する
        }

        // カーソル移動フラグがtrueなら
        if (CurrentMoveFlag)
        {
            Reset();                    // 各要素初期化
            option.SetActive(false);    // オプション画面をアクティブ化
            return;
        }

        // オプションがアクティブのときのみフラグをONにする
        if( option.activeSelf)CurrentMoveFlag = true;   // カーソル移動フラグをONにする
    }




    //*****************************************************
    // リセット
    //*****************************************************

    private void Reset()
    {
        cursol_num = 0;         // カーソル番号を初期化
        currentNum = -1;        // 決定番号を初期化
        CurrentMoveFlag = false;// 移動フラグをOFFにする


        // 矢印の座標を初期位置に戻す
        Vector3 ArrowPos = new Vector3(-40.0f, 25.0f, 0.0f);
        Arrow.localPosition = ArrowPos;
    }
}
