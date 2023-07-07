using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの移動を制御するクラス
/// </summary>
public class PlayerMove : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Transform playerTrn;
    [SerializeField] Transform orientationTrn;
    [SerializeField] Transform groundCheckTrn;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float rotationSpeed = 180;
    [SerializeField] float groundDrag = 10;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 10;
    [SerializeField] float jumpCooldown = 1;
    [SerializeField] float airMultiplier = 0.2f;
    bool readyToJump;

    [Header("Ground Check")]
    [SerializeField] LayerMask whatIsGround;
    float groundCheckRayLength = 0.3f;
    bool isGrounded = true;
    bool wasGrounded = true;

    [Header("Slope Handling")]
    [SerializeField] float maxSlopeAngle = 45;
    float slopeCheckRayLength = 0.5f;
    private RaycastHit slopeHit;
    private bool exitingSlope;


    Rigidbody rigidBody;
    Transform cameraTrn;
    Vector3 moveDirection;
    Quaternion targetRotation;

    Vector2 moveInput; // 移動入力
    bool jumpInput;

    readonly float GROUND_DRAG = 5;
    readonly float GRAVITY = 9.81f;
    readonly Vector2 VECTOR2_ZERO = new Vector2(0, 0);


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

    void Start()
    {
        cameraTrn = Camera.main.transform;

        rigidBody = playerTrn.GetComponent<Rigidbody>();
        rigidBody.drag = groundDrag;

        readyToJump = true;


        // Animatorコンポーネントを取得する
        anim = GetComponent<Animator>();
        // CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
        col = GetComponent<CapsuleCollider>();
        // CapsuleColliderコンポーネントのHeight、Centerの初期値を保存する
        orgColHight = col.height;
        orgVectColCenter = col.center;
    }

    void Update()
    {
        CheckGround();
        Rotate();
        SpeedControl();

        if (jumpInput)
        {
            Jump();
            jumpInput = false;
        }
    }

    void FixedUpdate()
    {
        float h = moveInput.x;  // 入力デバイスの水平軸をhで定義
        float v = Mathf.Abs(moveInput.y);  // 入力デバイスの垂直軸をvで定義
        anim.SetFloat("Speed", v);     // Animator側で設定している"Speed"パラメタにvを渡す
        anim.SetFloat("Direction", h); // Animator側で設定している"Direction"パラメタにhを渡す
        anim.speed = animSpeed;        // Animatorのモーション再生速度にanimSpeedを設定する
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // 参照用のステート変数にBaseLayer(0)の現在のステートを設定する

        Move();

        // 以下、Animatorの各ステート中での処理
        // Locomotion中
        // 現在のベースレイヤーがlocoStateの時
        if (currentBaseState.fullPathHash == locoState)
        {
            // カーブでコライダー調整をしているときは、念のためにリセットする
            if (useCurves)
            {
                ResetCollider();
            }
        }
        // JUMP中の処理
        // 現在のベースレイヤーがjumpStateの時
        else if (currentBaseState.fullPathHash == jumpState)
        {
            // ステートがトランジション中でない場合
            if (!anim.IsInTransition(0))
            {
                // 以下、カーブ調整をする場合の処理
                if (useCurves)
                {
                    // 以下JUMP00アニメーションについているカーブJumpHeightとGravityControl
                    // JumpHeight:JUMP00でのジャンプの高さ(0～1)
                    float jumpHeight = anim.GetFloat("JumpHeight");

                    // レイキャストをキャラクターのセンターから落とす
                    Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
                    RaycastHit hitInfo = new RaycastHit();
                    // 高さがuseCurvesHeight以上あるときのみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        if (hitInfo.distance > useCurvesHeight)
                        {
                            col.height = orgColHight - jumpHeight;      // 調整されたコライダーの高さ
                            float adjCenterY = orgVectColCenter.y + jumpHeight;
                            col.center = new Vector3(0, adjCenterY, 0); // 調整されたコライダーのセンター
                        }
                        else
                        {
                            // 閾値よりも低い時には初期値に戻す（念のため）					
                            ResetCollider();
                        }
                    }
                }
                // Jumpbool値をリセットする（ループしないようにする）			
                anim.SetBool("Jump", false);
            }
        }
        // IDLE中の処理
        // 現在のベースレイヤーがidleStateの時
        else if (currentBaseState.fullPathHash == idleState)
        {
            // カーブでコライダー調整をしているときは、念のためリセットする
            if (useCurves)
            {
                ResetCollider();
            }
        }
        // REST中の処理
        // 現在のベースレイヤーがrestStateの時
        else if (currentBaseState.fullPathHash == restState)
        {
            // ステートが遷移中でない場合、Restbool値をリセットする（ループしないようにする）
            if (!anim.IsInTransition(0))
            {
                anim.SetBool("Rest", false);
            }
        }
    }


    // キャラクターのコライダーサイズのリセット
    void ResetCollider()
    {
        // コンポーネントのHeight、Centerの初期値を戻す
        col.height = orgColHight;
        col.center = orgVectColCenter;
    }


    /// <summary>
    /// プレイヤーが接地しているか判定
    /// </summary>
    void CheckGround()
    {
        isGrounded = Physics.Raycast(groundCheckTrn.position, Vector3.down, groundCheckRayLength, whatIsGround);
        if (wasGrounded != isGrounded)
        {
            if (isGrounded)
            {
                rigidBody.drag = groundDrag;
            }
            else
            {
                rigidBody.drag = 0;
            }
            wasGrounded = isGrounded;
        }
    }


    // 移動入力を受け取る関数
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // ジャンプ入力を受け取る関数
    public void OnJump(InputValue value)
    {
        jumpInput = value.isPressed;
    }




    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        moveDirection = orientationTrn.forward * moveInput.y + orientationTrn.right * moveInput.x;

        // スロープ上
        if (OnSlope() && !exitingSlope)
        {
            rigidBody.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rigidBody.velocity.y > 0)
                rigidBody.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        // 地上
        else if (isGrounded)
        {
            rigidBody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // 空中
        else if (!isGrounded)
        {
            rigidBody.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        // スロープにいる時重力を無効化
        rigidBody.useGravity = !OnSlope();
    }


    /// <summary>
    /// 最高速度制限
    /// </summary>
    void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rigidBody.velocity.magnitude > moveSpeed)
                rigidBody.velocity = rigidBody.velocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rigidBody.velocity = new Vector3(limitedVel.x, rigidBody.velocity.y, limitedVel.z);
            }
        }

    }


    /// <summary>
    /// 回転処理
    /// </summary>
    void Rotate()
    {
        var dir = playerTrn.position - new Vector3(cameraTrn.position.x, playerTrn.position.y, cameraTrn.position.z);
        orientationTrn.forward = dir.normalized;

        moveDirection = orientationTrn.forward * moveInput.y + orientationTrn.right * moveInput.x;

        if (moveDirection != Vector3.zero)
        {
            playerTrn.forward = Vector3.Slerp(playerTrn.forward, moveDirection.normalized, rotationSpeed * Time.deltaTime);
        }
    }


    /// <summary>
    /// ジャンプ処理
    /// </summary>
    private void Jump()
    {
        if (readyToJump && isGrounded)
        {
            readyToJump = false;

            exitingSlope = true;
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
            rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump), jumpCooldown);

            //アニメーションのステートがLocomotionの最中のみジャンプできる
            if (currentBaseState.fullPathHash == locoState)
            {
                //ステート遷移中でなかったらジャンプできる
                if (!anim.IsInTransition(0))
                {
                    anim.SetBool("Jump", true);     // Animatorにジャンプに切り替えるフラグを送る
                }
            }
            // Rest状態になる
            if (currentBaseState.fullPathHash == idleState)
            {
                anim.SetBool("Rest", true);
            }
        }
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    /// <summary>
    /// スロープ上にいるか
    /// </summary>
    /// <returns></returns>
    private bool OnSlope()
    {
        if (Physics.Raycast(groundCheckTrn.position, Vector3.down, out slopeHit, slopeCheckRayLength))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }


}
