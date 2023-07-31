//
//  �^�C�g���̐i�s�Ǘ�
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleDirector : MonoBehaviour
{
    //************************** ���͏�� **************************//

    PlayerOperation playerOption;    // �A�N�V�����}�b�v


    //************************** Fade **************************//

    [Header("�t�F�[�h����script"), SerializeField] private Fade fade;



    //************************** �J�[�\���ړ� **************************//

    [Header("�g�I�u�W�F�N�g"), SerializeField] private RectTransform frame;           // �g�̍��W(rect)
    [Header("�g�̍��W(��)"), SerializeField] private RectTransform up_limit_obj;     //  �J�[�\�������ʒu(��)�ɐݒ肷��obj
    [Header("�g�̍��W(��)"), SerializeField] private RectTransform down_limit_obj;   //  �J�[�\�������ʒu(��)�ɐݒ肷��obj
    private Vector3 up_limit_pos;   //  �J�[�\������Y(��)
    private Vector3 down_limit_pos; //  �J�[�\������Y(��)
    private float move_value;       //  �J�[�\���ړ���񕪂̑����l


    //************************** �I�����֘A **************************//

    [Header("�V�[�����X�g"), SerializeField] private List<string> scenes; // �I�����̃��X�g
    public static int select_num;       //  ���쒆�J�[�\���ԍ�
    public static int next_num;         //  ���肵���J�[�\���ԍ�
    public static bool isNext;          //  �V�[���ڍs������

    //************************** ��A�N�e�B�u **************************//

    [Header("��A�N�e�B�uOPTION"), SerializeField] private GameObject option;
    [Header("��A�N�e�B�uCREDIT"), SerializeField] private GameObject credit;



    private void Awake()
    {
        // �A�N�V�����}�b�v�̃C���X�^���X����
        playerOption = new PlayerOperation();

        // �A�N�V���������s���ꂽ�Ƃ��ɌĂяo����郁�\�b�h��o�^
        playerOption.Option.Enter.started += context => OnEnter(context);
        playerOption.Option.MoveCursol.started += context => OnMoveCursol(context);
        playerOption.Option.Cansel.started += context => OnCansel(context);

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


    // Start is called before the first frame update
    void Start()
    {
        select_num = 0; //  ���݂̔ԍ�
        next_num = -1;  //  ���肵���ԍ�
        isNext = false; //  ���̃V�[���ֈڍs����t���O�Ǘ�

        //  �g�̏������W��ݒ�
        up_limit_pos = up_limit_obj.localPosition;
        down_limit_pos = down_limit_obj.localPosition;

        //  �J�[�\���̑����l��ݒ�
        move_value = Mathf.Abs(down_limit_pos.y) / (float)GetSelectMax();

        //  fadein�X�^�[�g
        StartCoroutine(fade.FadeIn());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameEndCheck(); // �Q�[�����I�����邩�̃`�F�b�N
    }



    //*****************************************************
    //  ����
    //*****************************************************
    private void OnEnter(InputAction.CallbackContext context)
    {
        //  ��x���ڍs���Ă��Ȃ��� or ���͂��ꂽ�u�ԈȊO�Ȃ珈�����Ȃ�
        if (isNext || !context.started) return;

        // �V�[���ԍ������肷��
        next_num = select_num;

        //  ���̃V�[���ֈڍs
        if (next_num == 0) Next();                // �V�[���ڍs����
        else if (next_num == 1)
        {
            StartCoroutine(fade.HalfFadeOut()); // �t�F�[�h�A�E�g
            Invoke("OptionProcess", 1); // fadeOut���Ă���\��
        }
        else if (next_num == 2)
        {
            StartCoroutine(fade.HalfFadeOut()); // �t�F�[�h�A�E�g
            Invoke("CreditProcess", 1); // fadeOut���Ă���\��
        }
    }



    //*****************************************************
    //  �J�[�\���ړ�
    //*****************************************************
    private void OnMoveCursol(InputAction.CallbackContext context)
    {
        //  �t�F�[�h���͏������Ȃ� or ���͂��ꂽ�u�ԈȊO�Ȃ珈�����Ȃ�
        if (isNext || !context.started) return;

        // ���͒l���擾
        float counter = context.ReadValue<float>();

        // 0.�`�̒l��1,-1�ɕϊ�����
        if (counter > 0.0f) counter = -1.0f;
        else if (counter < 0.0f) counter = 1.0f;

        select_num += (int)counter; // ���ݑI�����Ă���I�����̔ԍ�

        //  �J�[�\���ԍ��␳
        if (select_num >= scenes.Count) select_num = 0;
        else if (select_num < 0) select_num = GetSelectMax();

        //  �g�ړ�, Y �̂� ����Y + (�J�[�\����񕪂̈ړ��� * �J�[�\���ԍ�)
        var prev_pos = frame.localPosition;
        frame.localPosition = new Vector3(
            prev_pos.x, up_limit_pos.y - (move_value * select_num), prev_pos.z
            );
    }



    //*****************************************************
    //  �Q�[���I����
    //*****************************************************
    private void GameEndCheck()
    {
        //  �t�F�[�h���͏������Ȃ�
        if (isNext) return;

        // �I�����I������Ă�����
        if (next_num == GetSelectMax())
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;    // �Q�[���̏����𒆎~
#else
            Application.Quit();     // �Q�[���̏I��
#endif
        }
    }




    //*****************************************************
    //  ���̃V�[���ֈڍs(�ڍs�ł��邩������܂�)
    //*****************************************************
    private void Next()
    {
        //  ��x���ڍs���Ă��Ȃ��� or ���[�h���Ȃ��ԍ��͈̔͂�
        if (isNext || next_num >= 1) return;

        //  �����𖞂����Ă�����A�V�[���ڍs�����s����
        isNext = true;

        //  �t�F�[�h�A�E�g
        StartCoroutine(fade.FadeOut());

        // 1�b�҂��Ă��烍�[�h
        Invoke("LoadScene", 1);
    }



    //*****************************************************
    //  �I�����̍ő吔�擾
    //*****************************************************
    private int GetSelectMax() { return scenes.Count - 1; }



    //*****************************************************
    // �I�v�V��������
    //*****************************************************
    private void OptionProcess()
    {
        isNext = true;                                  // �V�[���ڍs�t���O��ON
        option.SetActive(true);                         // �A�N�e�B�u��
        GetComponent<SoundSetting>().enabled = true;    // �I�v�V�����X�N���v�g�̗L����
        SoundSetting.CurrentMoveFlag = true;            // �T�E���h�X�N���v�g�̃J�[�\���t���O��L����
        GetComponent<TitleDirector>().enabled = false;    // �^�C�g���X�N���v�g������
    }



    //*****************************************************
    // �L�����Z������
    //*****************************************************
    private void OnCansel(InputAction.CallbackContext context)
    {
        // ���͂��ꂽ�u�ԈȊO�Ȃ珈�����Ȃ�
        if (!context.started) return;

        isNext = false;                                 // �V�[���ڍs�t���O��OFF
        StartCoroutine(fade.FadeIn());                  // �t�F�[�h�C��
        option.SetActive(false);                        // �I�v�V�������A�N�e�B�u��
        GetComponent<TitleDirector>().enabled = true;     // �^�C�g���X�N���v�g�̗L����
        GetComponent<SoundSetting>().enabled = false;   // �I�v�V�����X�N���v�g�̖�����
    }



    //*****************************************************
    // �N���W�b�g����
    //*****************************************************
    private void CreditProcess()
    {
        isNext = true;                      // �V�[���ڍs�t���O��ON
        StartCoroutine(fade.HalfFadeOut()); // �t�F�[�h�A�E�g
        credit.SetActive(true);             // �A�N�e�B�u��
    }




    //*****************************************************
    // �V�[���̃��[�h
    //*****************************************************
    private void LoadScene()
    {
        //  �t�F�[�h���I������烍�[�h
        SceneManager.LoadScene(scenes[next_num]);
    }
}
