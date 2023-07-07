using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[�̈ړ��𐧌䂷��N���X
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

    Vector2 moveInput; // �ړ�����
    bool jumpInput;

    readonly float GROUND_DRAG = 5;
    readonly float GRAVITY = 9.81f;
    readonly Vector2 VECTOR2_ZERO = new Vector2(0, 0);


    public float animSpeed = 1.5f;              // �A�j���[�V�����Đ����x�ݒ�
    public float lookSmoother = 3.0f;           // a smoothing setting for camera motion
    public bool useCurves = true;               // Mecanim�ŃJ�[�u�������g�����ݒ肷��
                                                // ���̃X�C�b�`�������Ă��Ȃ��ƃJ�[�u�͎g���Ȃ�
    public float useCurvesHeight = 0.5f;        // �J�[�u�␳�̗L������(�n�ʂ����蔲���₷�����ɂ͑傫������)

    // �L�����N�^�[�R���g���[���i�J�v�Z���R���C�_�j�̎Q��
    private CapsuleCollider col;
    // CapsuleCollider�Őݒ肳��Ă���R���C�_��Heiht�ACenter�̏����l�����߂�ϐ�
    private float orgColHight;
    private Vector3 orgVectColCenter;
    private Animator anim;                          // �L�����ɃA�^�b�`�����A�j���[�^�[�ւ̎Q��
    private AnimatorStateInfo currentBaseState;     // baselayer�Ŏg����A�A�j���[�^�[�̌��݂̏�Ԃ̎Q��

    // �A�j���[�^�[�e�X�e�[�g�ւ̎Q��
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


        // Animator�R���|�[�l���g���擾����
        anim = GetComponent<Animator>();
        // CapsuleCollider�R���|�[�l���g���擾����i�J�v�Z���^�R���W�����j
        col = GetComponent<CapsuleCollider>();
        // CapsuleCollider�R���|�[�l���g��Height�ACenter�̏����l��ۑ�����
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
        float h = moveInput.x;  // ���̓f�o�C�X�̐�������h�Œ�`
        float v = Mathf.Abs(moveInput.y);  // ���̓f�o�C�X�̐�������v�Œ�`
        anim.SetFloat("Speed", v);     // Animator���Őݒ肵�Ă���"Speed"�p�����^��v��n��
        anim.SetFloat("Direction", h); // Animator���Őݒ肵�Ă���"Direction"�p�����^��h��n��
        anim.speed = animSpeed;        // Animator�̃��[�V�����Đ����x��animSpeed��ݒ肷��
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // �Q�Ɨp�̃X�e�[�g�ϐ���BaseLayer(0)�̌��݂̃X�e�[�g��ݒ肷��

        Move();

        // �ȉ��AAnimator�̊e�X�e�[�g���ł̏���
        // Locomotion��
        // ���݂̃x�[�X���C���[��locoState�̎�
        if (currentBaseState.fullPathHash == locoState)
        {
            // �J�[�u�ŃR���C�_�[���������Ă���Ƃ��́A�O�̂��߂Ƀ��Z�b�g����
            if (useCurves)
            {
                ResetCollider();
            }
        }
        // JUMP���̏���
        // ���݂̃x�[�X���C���[��jumpState�̎�
        else if (currentBaseState.fullPathHash == jumpState)
        {
            // �X�e�[�g���g�����W�V�������łȂ��ꍇ
            if (!anim.IsInTransition(0))
            {
                // �ȉ��A�J�[�u����������ꍇ�̏���
                if (useCurves)
                {
                    // �ȉ�JUMP00�A�j���[�V�����ɂ��Ă���J�[�uJumpHeight��GravityControl
                    // JumpHeight:JUMP00�ł̃W�����v�̍���(0�`1)
                    float jumpHeight = anim.GetFloat("JumpHeight");

                    // ���C�L���X�g���L�����N�^�[�̃Z���^�[���痎�Ƃ�
                    Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
                    RaycastHit hitInfo = new RaycastHit();
                    // ������useCurvesHeight�ȏ゠��Ƃ��̂݁A�R���C�_�[�̍����ƒ��S��JUMP00�A�j���[�V�����ɂ��Ă���J�[�u�Œ�������
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        if (hitInfo.distance > useCurvesHeight)
                        {
                            col.height = orgColHight - jumpHeight;      // �������ꂽ�R���C�_�[�̍���
                            float adjCenterY = orgVectColCenter.y + jumpHeight;
                            col.center = new Vector3(0, adjCenterY, 0); // �������ꂽ�R���C�_�[�̃Z���^�[
                        }
                        else
                        {
                            // 臒l�����Ⴂ���ɂ͏����l�ɖ߂��i�O�̂��߁j					
                            ResetCollider();
                        }
                    }
                }
                // Jumpbool�l�����Z�b�g����i���[�v���Ȃ��悤�ɂ���j			
                anim.SetBool("Jump", false);
            }
        }
        // IDLE���̏���
        // ���݂̃x�[�X���C���[��idleState�̎�
        else if (currentBaseState.fullPathHash == idleState)
        {
            // �J�[�u�ŃR���C�_�[���������Ă���Ƃ��́A�O�̂��߃��Z�b�g����
            if (useCurves)
            {
                ResetCollider();
            }
        }
        // REST���̏���
        // ���݂̃x�[�X���C���[��restState�̎�
        else if (currentBaseState.fullPathHash == restState)
        {
            // �X�e�[�g���J�ڒ��łȂ��ꍇ�ARestbool�l�����Z�b�g����i���[�v���Ȃ��悤�ɂ���j
            if (!anim.IsInTransition(0))
            {
                anim.SetBool("Rest", false);
            }
        }
    }


    // �L�����N�^�[�̃R���C�_�[�T�C�Y�̃��Z�b�g
    void ResetCollider()
    {
        // �R���|�[�l���g��Height�ACenter�̏����l��߂�
        col.height = orgColHight;
        col.center = orgVectColCenter;
    }


    /// <summary>
    /// �v���C���[���ڒn���Ă��邩����
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


    // �ړ����͂��󂯎��֐�
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // �W�����v���͂��󂯎��֐�
    public void OnJump(InputValue value)
    {
        jumpInput = value.isPressed;
    }




    /// <summary>
    /// �ړ�����
    /// </summary>
    void Move()
    {
        moveDirection = orientationTrn.forward * moveInput.y + orientationTrn.right * moveInput.x;

        // �X���[�v��
        if (OnSlope() && !exitingSlope)
        {
            rigidBody.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rigidBody.velocity.y > 0)
                rigidBody.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        // �n��
        else if (isGrounded)
        {
            rigidBody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // ��
        else if (!isGrounded)
        {
            rigidBody.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        // �X���[�v�ɂ��鎞�d�͂𖳌���
        rigidBody.useGravity = !OnSlope();
    }


    /// <summary>
    /// �ō����x����
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
    /// ��]����
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
    /// �W�����v����
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

            //�A�j���[�V�����̃X�e�[�g��Locomotion�̍Œ��̂݃W�����v�ł���
            if (currentBaseState.fullPathHash == locoState)
            {
                //�X�e�[�g�J�ڒ��łȂ�������W�����v�ł���
                if (!anim.IsInTransition(0))
                {
                    anim.SetBool("Jump", true);     // Animator�ɃW�����v�ɐ؂�ւ���t���O�𑗂�
                }
            }
            // Rest��ԂɂȂ�
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
    /// �X���[�v��ɂ��邩
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
