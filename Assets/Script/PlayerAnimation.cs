using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Transform playerTrn;
    [SerializeField] Transform orientationTrn;
    [SerializeField] Transform groundCheckTrn;

    [Header("Animation")]
    [SerializeField] private bool result = false;


    Rigidbody rigidBody;

    public float animSpeed = 1.5f;              // アニメーション再生速度設定
    public float lookSmoother = 3.0f;           // a smoothing setting for camera motion
    public bool useCurves = true;               // Mecanimでカーブ調整を使うか設定する
                                                // このスイッチが入っていないとカーブは使われない
    public float useCurvesHeight = 0.5f;        // カーブ補正の有効高さ(地面をすり抜けやすい時には大きくする)

    // キャラクターコントローラ（カプセルコライダ）の参照
    private CapsuleCollider col;
    // CapsuleColliderで設定されているコライダのHeiht、Centerの初期値を収める変数
    private float orgColHight;
    private Vector3 orgVectColCenter;
    private Animator anim;                          // キャラにアタッチされるアニメーターへの参照
    private AnimatorStateInfo currentBaseState;     // baselayerで使われる、アニメーターの現在の状態の参照

    // アニメーター各ステートへの参照
    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int locoState = Animator.StringToHash("Base Layer.Locomotion");
    static int jumpState = Animator.StringToHash("Base Layer.Jump");
    static int restState = Animator.StringToHash("Base Layer.Rest");

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = playerTrn.GetComponent<Rigidbody>();

        result = false;

        // Animatorコンポーネントを取得する
        anim = GetComponent<Animator>();
        // CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
        col = GetComponent<CapsuleCollider>();
        // CapsuleColliderコンポーネントのHeight、Centerの初期値を保存する
        orgColHight = col.height;
        orgVectColCenter = col.center;
    }

    // Update is called once per frame
    void Update()
    {
        if (result)
        {
            if (!anim.GetBool("Result")) anim.SetBool("Result", true);
        }
        else if (anim.GetBool("Result")) anim.SetBool("Result", false);
    }
}
