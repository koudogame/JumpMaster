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

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = playerTrn.GetComponent<Rigidbody>();

        result = false;

        // Animator�R���|�[�l���g���擾����
        anim = GetComponent<Animator>();
        // CapsuleCollider�R���|�[�l���g���擾����i�J�v�Z���^�R���W�����j
        col = GetComponent<CapsuleCollider>();
        // CapsuleCollider�R���|�[�l���g��Height�ACenter�̏����l��ۑ�����
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
