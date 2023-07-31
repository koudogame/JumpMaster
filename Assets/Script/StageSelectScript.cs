using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;  // DoTeenを使うのに必要
using UnityEngine.SceneManagement;

public sealed class StageSelectScript : MonoBehaviour
{
    //************************** 入力情報 **************************//

    PlayerOperation playerOption;
    public static bool InputReceive;    // 入力受取フラグ


    //************************** Fade **************************//

    [Header("フェード処理script"), SerializeField] private Fade fade;


    private Camera MainCamera;           //  メインカメラのオブジェクト


    //************************** 選択肢関連 **************************//

    [Header("ステージ番号"),SerializeField] private int stage_num;
    [Header("ステージ数"),SerializeField]   private int stage_max;
    [Header("カメラを動かす秒数"), SerializeField] private float rotate_second;
    public static string SceneName; // シーン名設定



    //************************** 非アクティブオブジェクト **************************//

    [Header("非アクティブオブジェクト"), SerializeField] private GameObject option;




    private void Awake()
    {
        MainCamera = Camera.main;   // カメラの回転座標を取得
        stage_num = 1;              //  ステージを選んでいる番号(1固定)
        stage_max = 5;              //  最大ステージ数
        rotate_second = 2f;         //  回転秒数

        setNextSceneName();         // 次のシーンの名前を格納
        
        //  カメラの初期回転値を設定
        MainCamera.transform.rotation = Quaternion.Euler(new Vector3(0, (stage_num-1) * GetRotDeg(), 0));


        // アクションマップのインスタンス生成
        playerOption = new PlayerOperation();


        // アクションが実行されたときに呼び出されるメソッドを登録
        playerOption.Option.Rotation.started += context => OnRotation(context);
        playerOption.Option.Enter.started += context => OnEnter(context);
        
        // フェードイン
        StartCoroutine(fade.FadeIn());
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

    private void Start()
    {
        InputReceive = true;    // 入力受取フラグをON
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //*****************************************************
    // ロードするシーンの名前を設定
    //*****************************************************
    void setNextSceneName()
    {
        // Loadシーンのクラスにシーン番号を格納
        LoadSceneDirector.LoadStage(stage_num);
    }



    //*****************************************************
    //  カメラを回す
    //*****************************************************+
    public void OnRotation(InputAction.CallbackContext context)
    {
        //  押した瞬間以外なら処理しない or 入力受取フラグOFFなら処理しない
        if (!context.started || !InputReceive) return;

        //  入力値を取得
        float value = context.ReadValue<float>();

        // 選択肢の番号を増やす
        stage_num += (int)value;
        int stage_max = Mathf.FloorToInt(360.0f / GetRotDeg());

        // 上限設定
        if (stage_num < 1) stage_num += stage_max;
        else if (stage_num > stage_max) stage_num -= stage_max;

        // 回転
        MainCamera.transform.DORotate(new Vector3(0, (stage_num - 1) * GetRotDeg(), 0), rotate_second).SetEase(Ease.OutExpo);
        setNextSceneName(); // 次のシーンの名前を設定
    }



    //*****************************************************
    //  決定
    //*****************************************************
    public void OnEnter(InputAction.CallbackContext context)
    {
        if (!context.started || !InputReceive) return;

        SceneName = LoadSceneDirector.LoadScene();  // 現在のシーン名を取得
        SceneManager.LoadScene(SceneName);      // シーンをロード
    }



    //*****************************************************
    //  一回分のカメラを動かす角度
    //*****************************************************
    private float GetRotDeg()
    {
        return 360.0f / stage_max;
    }
}
