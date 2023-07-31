using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;  // DoTeen���g���̂ɕK�v
using UnityEngine.SceneManagement;

public sealed class StageSelectScript : MonoBehaviour
{
    //************************** ���͏�� **************************//

    PlayerOperation playerOption;
    public static bool InputReceive;    // ���͎��t���O


    //************************** Fade **************************//

    [Header("�t�F�[�h����script"), SerializeField] private Fade fade;


    private Camera MainCamera;           //  ���C���J�����̃I�u�W�F�N�g


    //************************** �I�����֘A **************************//

    [Header("�X�e�[�W�ԍ�"),SerializeField] private int stage_num;
    [Header("�X�e�[�W��"),SerializeField]   private int stage_max;
    [Header("�J�����𓮂����b��"), SerializeField] private float rotate_second;
    public static string SceneName; // �V�[�����ݒ�



    //************************** ��A�N�e�B�u�I�u�W�F�N�g **************************//

    [Header("��A�N�e�B�u�I�u�W�F�N�g"), SerializeField] private GameObject option;




    private void Awake()
    {
        MainCamera = Camera.main;   // �J�����̉�]���W���擾
        stage_num = 1;              //  �X�e�[�W��I��ł���ԍ�(1�Œ�)
        stage_max = 5;              //  �ő�X�e�[�W��
        rotate_second = 2f;         //  ��]�b��

        setNextSceneName();         // ���̃V�[���̖��O���i�[
        
        //  �J�����̏�����]�l��ݒ�
        MainCamera.transform.rotation = Quaternion.Euler(new Vector3(0, (stage_num-1) * GetRotDeg(), 0));


        // �A�N�V�����}�b�v�̃C���X�^���X����
        playerOption = new PlayerOperation();


        // �A�N�V���������s���ꂽ�Ƃ��ɌĂяo����郁�\�b�h��o�^
        playerOption.Option.Rotation.started += context => OnRotation(context);
        playerOption.Option.Enter.started += context => OnEnter(context);
        
        // �t�F�[�h�C��
        StartCoroutine(fade.FadeIn());
    }


    private void OnEnable()
    {
        // �I�u�W�F�N�g���L���ɂȂ���������
        playerOption.Enable();
    }

    private void OnDisable()
    {
        // �I�u�W�F�N�g�������ɂȂ�������~
        playerOption.Disable();
    }

    private void Start()
    {
        InputReceive = true;    // ���͎��t���O��ON
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //*****************************************************
    // ���[�h����V�[���̖��O��ݒ�
    //*****************************************************
    void setNextSceneName()
    {
        // Load�V�[���̃N���X�ɃV�[���ԍ����i�[
        LoadSceneDirector.LoadStage(stage_num);
    }



    //*****************************************************
    //  �J��������
    //*****************************************************+
    public void OnRotation(InputAction.CallbackContext context)
    {
        //  �������u�ԈȊO�Ȃ珈�����Ȃ� or ���͎��t���OOFF�Ȃ珈�����Ȃ�
        if (!context.started || !InputReceive) return;

        //  ���͒l���擾
        float value = context.ReadValue<float>();

        // �I�����̔ԍ��𑝂₷
        stage_num += (int)value;
        int stage_max = Mathf.FloorToInt(360.0f / GetRotDeg());

        // ����ݒ�
        if (stage_num < 1) stage_num += stage_max;
        else if (stage_num > stage_max) stage_num -= stage_max;

        // ��]
        MainCamera.transform.DORotate(new Vector3(0, (stage_num - 1) * GetRotDeg(), 0), rotate_second).SetEase(Ease.OutExpo);
        setNextSceneName(); // ���̃V�[���̖��O��ݒ�
    }



    //*****************************************************
    //  ����
    //*****************************************************
    public void OnEnter(InputAction.CallbackContext context)
    {
        if (!context.started || !InputReceive) return;

        SceneName = LoadSceneDirector.LoadScene();  // ���݂̃V�[�������擾
        SceneManager.LoadScene(SceneName);      // �V�[�������[�h
    }



    //*****************************************************
    //  ��񕪂̃J�����𓮂����p�x
    //*****************************************************
    private float GetRotDeg()
    {
        return 360.0f / stage_max;
    }
}
