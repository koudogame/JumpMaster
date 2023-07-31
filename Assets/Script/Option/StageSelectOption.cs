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

    //************************** ���͏�� **************************//

    PlayerOperation playerOption;

    //************************** Fade **************************//

    [Header("�t�F�[�h����script"), SerializeField] private Fade fade;


    //************************** �J�[�\���ړ� **************************//

    [Header("�J�[�\���ԍ�"), SerializeField] private int cursol_num;                    // ���݂̑I�����ԍ�
    [Header("�J�[�\�����X�g"), SerializeField] private List<string> OptionList;        // �I�����̃��X�g
    [Header("�J�[�\�����I�u�W�F�N�g"), SerializeField] private RectTransform Arrow;  // ���̍��W
    [Header("���̍��W(��)"), SerializeField] private RectTransform upLimit;          // ������ʒu�ɐݒ肷��Obj 
    [Header("���̍��W(��)"), SerializeField] private RectTransform downLimit;         // ��󉺌��ʒu�ɐݒ肷��Obj
    private Vector3 upLimitPos;     // �J�[�\������ʒuY
    private int currentNum;         // ���莞�̔ԍ�
    public static bool CurrentMoveFlag; // �J�[�\���ړ��\�t���O


    //************************** ��A�N�e�B�u�I�u�W�F�N�g **************************//

    [Header("��A�N�e�B�uOPTION"), SerializeField] private GameObject option;
    [Header("��A�N�e�B�uSOUND"), SerializeField] private GameObject sound;
    [Header("��A�N�e�B�uTUTORIALSELECT"),SerializeField] private GameObject tutorialselect;



    private void Awake()
    {
        // �A�N�V�����}�b�v�̃C���X�^���X����
        playerOption = new PlayerOperation();

        // �A�N�V�������s���Ăяo����郁�\�b�h��o�^
        playerOption.Option.Enter.started += context => OnEnter(context);
        playerOption.Option.MoveCursol.started += context => OnMoveCursol(context);
        playerOption.Option.Menu.started += context => OnMenu(context);
        playerOption.Option.Cansel.started += context => OnCansel(context);
    }

    private void OnEnable()
    {
        // �I�u�W�F�N�g�L��������
        playerOption.Enable();
    }

    private void OnDisable()
    {
        // �I�u�W�F�N�g��������~
        playerOption.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        cursol_num = 0;                             // �I�����̔ԍ�
        upLimitPos = upLimit.localPosition;         // ������W
        currentNum = -1;            // ����ԍ�������
        CurrentMoveFlag = false;    // �ړ��s��


        StartCoroutine(fade.HalfFadeIn());  // �t�F�[�h�C��
    }




    //*****************************************************
    //  ����
    //*****************************************************
    private void OnEnter(InputAction.CallbackContext context)
    {
        // �J�[�\���ړ��t���O�������Ȃ珈�����Ȃ�
        if (!CurrentMoveFlag || !context.started) return;

        currentNum = cursol_num;    // ���莞�̔ԍ����擾

        if (currentNum == 1)
        {
            CurrentMoveFlag = false;                            // �J�[�\���ړ�����
            sound.SetActive(true);                              // �T�E���h��ʂ��A�N�e�B�u��
            GetComponent<SoundSetting>().enabled = true;        // �I�v�V�����̃`�F�b�N�{�b�N�X��L��������
            SoundSetting.CurrentMoveFlag = true;                // �T�E���h�̃J�[�\���ړ��t���O��ON
            option.SetActive(false);                            // �I�v�V������ʂ��A�N�e�B�u��
            GetComponent<StageSelectOption>().enabled = false;  // �`���[�g���A���̃I�v�V�����̃`�F�b�N�{�b�N�X�𖳌�������
        }
        else if (currentNum == 1)
        {
            CurrentMoveFlag = false;                            // �J�[�\���ړ��t���OOFF
            tutorialselect.SetActive(true);                     // �`���[�g���A���Z���N�g�I�u�W�F�N�g���A�N�e�B�u��
            GetComponent<TutorialSelect>().enabled = true;      // �`���[�g���A���Z���N�g�̃X�N���v�g��L����
            TutorialSelect.CurrentMoveFlag = true;              // �`���[�g���A���Z���N�g�̃J�[�\���ړ��t���O��ON
            option.SetActive(false);                            // �I�v�V�������A�N�e�B�u��
            GetComponent <StageSelectOption>().enabled = false; // �X�e�[�W�Z���N�g�I�v�V�����X�N���v�g�𖳌���
        }
        else if(  currentNum == 2 && SceneName == SceneNameOption.GAME)
        {
            Reset();                                    // �e���ڏ�����
            StartCoroutine(fade.FadeOut());             // �t�F�[�h�A�E�g
            SceneManager.LoadScene("StageSelectScene"); // �V�[�������[�h
        }

        // �߂��I�����Ă�����
        if (currentNum == GetSelectMax())
        {
            Reset();    // �e�v�f������
            return;
        }
    }




    //*****************************************************
    //  �J�[�\���ړ�
    //*****************************************************
    private void OnMoveCursol(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�ԈȊO�Ȃ珈�����Ȃ�
        if (!context.started) return;

        // �J�[�\���ړ��t���O��ON�Ȃ�
        if (CurrentMoveFlag)
        {
            // ���͒l�擾
            float value = context.ReadValue<float>();   

            // 0.�`�̒l��1,-1�ɕϊ�����
            if (value > 0.0f) value = -1.0f;
            else if (value < 0.0f) value = 1.0f;

            // �J�[�\���J�E���g��i�߂�
            cursol_num += (int)value;   

            // �J�[�\���̔ԍ��␳
            if (cursol_num >= OptionList.Count) cursol_num = 0;
            else if (cursol_num < 0) cursol_num = GetSelectMax();

            //  �g�ړ�, Y �̂� ����Y + (�J�[�\����񕪂̈ړ��� * �J�[�\���ԍ�)
            var prev_pos = Arrow.localPosition;
            Arrow.localPosition = new Vector3(prev_pos.x, upLimitPos.y - (20 * cursol_num), prev_pos.z);
        }
    }




    //*****************************************************
    // �L�����Z������
    //*****************************************************
    private void OnCansel(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�ԈȊO�Ȃ珈�����Ȃ�
        if (!context.started) return;

        // �L�����Z���������ꂽ�Ƃ��T�E���h��ʂ��A�N�e�B�u����Ԃ̂Ƃ�
        // �T�E���h��ʂ��A�N�e�B�u��������
        if (sound.activeSelf)
        {
            option.SetActive(true); // �I�v�V������ʂ��A�N�e�B�u��
            GetComponent<SoundSetting>().enabled = false; // �I�v�V�����̃`�F�b�N�{�b�N�X�𖳌�������
        }

        // �J�[�\���ړ��t���O��true�Ȃ�
        if (CurrentMoveFlag)
        {
            Reset();    // �e�v�f������
            return;
        }

        // �I�v�V�������A�N�e�B�u�̂Ƃ��̂݃t���O��ON
        if( option.activeSelf ) CurrentMoveFlag = true;   // �J�[�\���ړ��t���O��ON�ɂ���
    }




    //*****************************************************
    // ���j���[����
    //*****************************************************
    private void OnMenu(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�ԈȊO�Ȃ珈�����Ȃ�
        if (!context.started) return;

        // �A�N�e�B�u����Ԃł�����x���j���[�������ꂽ��
        // ��A�N�e�B�u���ɂ���
        if (option.activeSelf)
        {
            ReverseFlag(SceneName);     // �X�e�[�W�Z���N�g�X�N���v�g�̓��͂��󂯎�邩�̐ݒ�
            option.SetActive(false);    // �I�v�V�������A�N�e�B�u��
            return;
        }
        else option.SetActive(true);    // �I�v�V�������A�N�e�B�u��

        StageSelectScript.InputReceive = false; // ���͎��t���O��OFF
        CurrentMoveFlag = true;                 // �J�[�\���ړ��t���O��ON
    }




    //*****************************************************
    // �I�����ő吔�擾 
    //*****************************************************
    private int GetSelectMax()
    {
        return OptionList.Count - 1;
    }




    //*****************************************************
    // ���Z�b�g
    //*****************************************************
    private void Reset()
    {
        cursol_num = 0;                 // �J�[�\���ԍ���������
        currentNum = -1;                // ����ԍ���������
        CurrentMoveFlag = false;        // �ړ��t���O��OFF�ɂ���
        option.SetActive(false);        // ��A�N�e�B�u��
        ReverseFlag(SceneName);         // �X�e�[�W�Z���N�g�X�N���v�g�̓��͎�悷�邩�ݒ�


        // ���̈ʒu�������ʒu�Ɏw��
        if (SceneName == SceneNameOption.STAGESELECT)
        {
            // ���̍��W�������ʒu�ɖ߂�
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
    // ���͎��t���O�̐ݒ�
    //*****************************************************
    private void ReverseFlag(SceneNameOption SceneName)
    {
        // �V�[���̖��O���X�e�[�W�Z���N�g�ȊO�Ȃ珈�����Ȃ�
        if (SceneName != SceneNameOption.STAGESELECT) return;

        StageSelectScript.InputReceive = true;  // ���͎��t���O��ON
    }
}

