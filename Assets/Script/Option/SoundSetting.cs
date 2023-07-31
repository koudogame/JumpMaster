using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    //****************　オプションの名前　 ****************//

    private enum OptionName 
    { 
        TITLE,
        TUTORIAL,
        STAGESELECT,
        GAME
    }
    [SerializeField] private OptionName SceneName;



    //****************　入力情報　 ****************//

    PlayerOperation playerOption;  // アクションマップ



    //****************　非アクティブ　 ****************//

    [Header("非アクティブSOUND"), SerializeField] private GameObject sound;



    //****************　フェード　 ****************//

    [Header("Fade"), SerializeField] private Fade fade;


    //****************　ボリューム調整する音の種類　 ****************//

    public enum VolumeType { NULL, BGM, SE }
    [SerializeField] private VolumeType volumeType = 0;



    //****************　音量設定用　 ****************//

    public Slider bgmSlider;
    public Slider seSlider;
    public SoundManager soundManager;
    public static AudioSource audioSource;



    //****************　カーソルの情報　 ****************//

    [Header("カーソル番号"), SerializeField] private int cursol_num;                   // 現在の選択肢番号
    [Header("カーソルリスト"), SerializeField] private List<string> CursolList;        // 選択肢のリスト
    [Header("カーソル矢印オブジェクト"), SerializeField] private RectTransform Arrow;  // 矢印の座標
    [Header("矢印の座標(上)"), SerializeField] private RectTransform upLimit;          // 矢印上限位置に設定するObj 
    [Header("矢印の座標(下)"), SerializeField] private RectTransform downLimit;        // 矢印下限位置に設定するObj
    private Vector3 upLimitPos;     // カーソル上限位置Y
    private int currentNum;         // 決定時の番号
    public static bool CurrentMoveFlag; // カーソル移動可能フラグ




    private void Awake()
    {
        // インスタンス生成
        playerOption = new PlayerOperation();

        // アクションが実行されたとき呼び出されるメソッドを登録
        playerOption.Option.Enter.started += context => OnEnter(context);
        playerOption.Option.MoveCursol.started += context => OnMoveCursol(context);
        playerOption.Option.SliderMove.started += context => OnSliderMove(context);
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
    private void Start()
    {
        cursol_num = 0;                             // 選択肢の番号
        upLimitPos = upLimit.localPosition;         // 上限座標
        currentNum = -1;                            // 決定番号
        CurrentMoveFlag = true;                     // 移動可能
        
        // GameObjectを探してSoundManager型で返す
        soundManager = FindObjectOfType<SoundManager>();

        // サウンドの音量設定
        bgmSlider.value = soundManager.BgmVolume;
        seSlider.value = soundManager.SeVolume;
    }



    //*****************************************************
    // 決定
    //*****************************************************
    private void OnEnter(InputAction.CallbackContext context)
    {
        // 入力された瞬間以外なら処理しない
        if (!context.started) return;

        // 最初は処理をしない
        if (!CurrentMoveFlag)
        {
            CurrentMoveFlag = true;  // カーソル移動フラグをONに設定
            return;
        }
        else CurrentMoveFlag = false;   // 移動フラグをOFF


        currentNum = cursol_num;    // 決定時の番号を取得

        // 決定番号に応じてサウンドの種類を変更
        if (currentNum == 0) volumeType = VolumeType.BGM;
        else if (currentNum == 1) volumeType = VolumeType.SE;

        // 戻るを選択していたら
        if (currentNum == GetSelectMax())
        {
            Reset();    // 各要素初期化
            StartCoroutine(fade.FadeIn());  // フェードアウト
            return;
        }
    }




    //*****************************************************
    //  カーソル移動
    //*****************************************************
    private void OnMoveCursol(InputAction.CallbackContext context)
    {
        // 入力された瞬間以外なら処理しない
        if ( !context.started) return;

        // カーソル移動フラグがONなら
        if (CurrentMoveFlag)
        {
            float value = context.ReadValue<float>();   // 入力値取得

            // 0.～の値を1,-1に変換する
            if (value > 0.0f) value = -1.0f;
            else if (value < 0.0f) value = 1.0f;

            cursol_num += (int)value;   // カーソルカウントを進める

            // カーソルの番号補正
            if (cursol_num >= CursolList.Count) cursol_num = 0;
            else if (cursol_num < 0) cursol_num = GetSelectMax();

            //  枠移動, Y のみ 初期Y + (カーソル一回分の移動量 * カーソル番号)
            var prev_pos = Arrow.localPosition;
            Arrow.localPosition = new Vector3(prev_pos.x, upLimitPos.y - (20 * cursol_num), prev_pos.z);
        }
    }




    //*****************************************************
    // リストの最大数取得
    //*****************************************************
    private int GetSelectMax()
    {
        return CursolList.Count - 1;
    }



    //*****************************************************
    // キャンセル処理
    //*****************************************************
    private void OnCansel(InputAction.CallbackContext context)
    {
        // 入力された瞬間以外なら処理しない
        if (!context.started) return;

        // カーソル移動フラグがtrueなら
        if (CurrentMoveFlag)
        {
            Reset();    // 各要素初期化
            return;
        }

        CurrentMoveFlag = true;   // カーソル移動フラグをONにする
    }




    //*****************************************************
    // スライダーの値を変更する関数
    //*****************************************************
    private void OnSliderMove(InputAction.CallbackContext context)
    {
        // 入力された瞬間以外なら処理しない
        if (!context.started) return;

        // カーソル移動フラグがOFFのときのみ処理
        if (!CurrentMoveFlag)
        {
            float value = context.ReadValue<float>();   // 入力受取

            // 移動量を設定
            if (value > 0.0f) value = 0.1f;
            else if (value < 0.0f) value = -0.1f;

            // 音の種類からどちらのスライダーを変化させるか決める
            if (volumeType == VolumeType.BGM)
            {
                bgmSlider.value += value; // スライダーの値変更

                // 範囲設定
                if (bgmSlider.value > bgmSlider.maxValue) bgmSlider.value = bgmSlider.maxValue;
                else if (bgmSlider.value < bgmSlider.minValue) bgmSlider.value = bgmSlider.minValue;
            }
            else if (volumeType == VolumeType.SE)
            {
                seSlider.value += value; // スライダーの値変更

                // 範囲設定
                if (seSlider.value > seSlider.maxValue) seSlider.value = seSlider.maxValue;
                else if (seSlider.value < seSlider.minValue) seSlider.value = seSlider.minValue;
            }
        }
    }





    //*****************************************************
    // リセット
    //*****************************************************
    private void Reset()
    {
        cursol_num = 0;                 // カーソル番号初期化
        currentNum = -1;                // 決定番号を初期化
        CurrentMoveFlag = false;        // 移動フラグをOFFにする
        volumeType = VolumeType.NULL;   // サウンド種類初期化


        switch(SceneName)
        {
            case OptionName.TITLE:
                GetComponent<TitleDirector>().enabled = true; // タイトルスクリプト有効化
                TitleDirector.isNext = false;                 // シーン移動フラグを無効化
                break;
            case OptionName.TUTORIAL:
                GetComponent<TutorialOption>().enabled = true;  // チュートリアルオプションスクリプト有効化
                Search();                                       // オブジェクト検索
                TutorialOption.CurrentMoveFlag = true;          // チュートリアルオプションのカーソルフラグをON
                break;
            case OptionName.STAGESELECT or OptionName.GAME:
                GetComponent<StageSelectOption>().enabled = true;   // ステージセレクトオプションのスクリプト有効化
                Search();                                           // オブジェクト検索
                StageSelectOption.CurrentMoveFlag = true;           // ステージセレクトオプションのカーソルフラグを有効化
                break;
        }


        sound.SetActive (false);    // サウンドオプションを非アクティブ化
        
        // 矢印の座標を初期位置に戻す
        Vector3 ArrowPos = new Vector3(-40.0f, 20.0f, 0.0f);
        Arrow.localPosition = ArrowPos;
        
        GetComponent<SoundSetting>().enabled = false;     // オプションスクリプトの無効化
    }



    //*****************************************************
    // 検索処理
    //*****************************************************
    private void Search()
    {
        // オブジェクト検索
        GameObject option = GameObject.Find("Canvas").transform.Find("Option").gameObject;

        // 検索したオブジェクトをアクティブ化
        option.SetActive(true);
    }
}
