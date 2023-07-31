using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialOption : MonoBehaviour
{
    //****************�@���͏��@ ****************//

    PlayerOperation playerOption;



    //************************** Fade **************************//

    [Header("�t�F�[�h����script"), SerializeField] private Fade fade;



    //****************�@��A�N�e�B�u�@ ****************//

    [Header("��A�N�e�B�uOPTION"), SerializeField] private GameObject option;
    [Header("��A�N�e�B�uSOUND"), SerializeField] private GameObject sound;



    //****************�@�J�[�\���̏��@ ****************//

    [Header("�J�[�\���ԍ�"), SerializeField] private int cursol_num;                    // ���݂̑I�����ԍ�
    [Header("�J�[�\�����X�g"), SerializeField] private List<string> OptionList;        // �I�����̃��X�g
    [Header("�J�[�\�����I�u�W�F�N�g"), SerializeField] private RectTransform Arrow;  // ���̍��W
    [Header("���̍��W(��)"), SerializeField] private RectTransform upLimit;          // ������ʒu�ɐݒ肷��Obj 
    [Header("���̍��W(��)"), SerializeField] private RectTransform downLimit;         // ��󉺌��ʒu�ɐݒ肷��Obj
    private Vector3 upLimitPos;     // �J�[�\������ʒuY
    private int currentNum;         // ���莞�̔ԍ�
    public static bool CurrentMoveFlag; // �J�[�\���ړ��\�t���O



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
        // �I�u�W�F�N�g���L��������
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
        currentNum = -1;
        CurrentMoveFlag = false;   // �ړ��\
    }




    //*****************************************************
    //  ����
    //*****************************************************
    private void OnEnter(InputAction.CallbackContext context)
    {
        // �J�[�\���ړ��t���O��OFF�Ȃ珈�����Ȃ�
        if (!CurrentMoveFlag) return;

        currentNum = cursol_num;    // ���莞�̔ԍ����擾

        if( currentNum == 0)
        {
            CurrentMoveFlag = false;                        // �J�[�\���ړ�����
            sound.SetActive(true);                          // �T�E���h��ʂ��A�N�e�B�u��
            GetComponent<SoundSetting>().enabled = true;    // �I�v�V�����̃`�F�b�N�{�b�N�X��L��������
            SoundSetting.CurrentMoveFlag = true;            // �T�E���h�̃J�[�\���ړ��t���O��ON
            option.SetActive(false);                        // �I�v�V������ʂ��A�N�e�B�u��
            GetComponent<TutorialOption>().enabled = false; // �`���[�g���A���̃I�v�V�����̃`�F�b�N�{�b�N�X�𖳌�������
        }
        else if(  currentNum == 1) 
        {
            StartCoroutine(fade.FadeOut());             // �t�F�[�h�A�E�g
            SceneManager.LoadScene("StageSelectScene"); // �V�[�������[�h
        }
        // �߂��I�����Ă�����
        if (currentNum == GetSelectMax())
        {
            Reset();    // �e�v�f������
            option.SetActive(false);        // ��A�N�e�B�u
            return;
        }
    }




    //*****************************************************
    //  �J�[�\���ړ�
    //*****************************************************
    private void OnMoveCursol(InputAction.CallbackContext context)
    {
        // �J�[�\���ړ��t���O��ON�Ȃ�
        if (CurrentMoveFlag)
        {
            float value = context.ReadValue<float>();   // ���͒l�擾

            // 0.�`�̒l��1,-1�ɕϊ�����
            if (value > 0.0f) value = -1.0f;
            else if (value < 0.0f) value = 1.0f;

            cursol_num += (int)value;   // �J�[�\���J�E���g��i�߂�

            // �J�[�\���̔ԍ��␳
            if (cursol_num >= OptionList.Count) cursol_num = 0;
            else if (cursol_num < 0) cursol_num = GetSelectMax();

            //  �g�ړ�, Y �̂� ����Y + (�J�[�\����񕪂̈ړ��� * �J�[�\���ԍ�)
            var prev_pos = Arrow.localPosition;
            Arrow.localPosition = new Vector3(prev_pos.x, upLimitPos.y - (20 * cursol_num), prev_pos.z);
        }
    }





    //*****************************************************
    // ���j���[����
    //*****************************************************
    private void OnMenu(InputAction.CallbackContext context)
    {
        // �A�N�e�B�u����Ԃł�����x���j���[�������ꂽ��
        // ��A�N�e�B�u���ɂ���
        if (option.activeSelf)
        {
            option.SetActive(false);    // ��A�N�e�B�u��
            return;
        }

        option.SetActive(true); // ���j���[�{�^���������ꂽ��A�N�e�B�u��������
        CurrentMoveFlag = true; // �J�[�\���ړ��t���O��ON
    }




    //*****************************************************
    // �I�����ő吔�擾  
    //*****************************************************
    private int GetSelectMax()
    {
        return OptionList.Count - 1;
    }




    //*****************************************************
    // �L�����Z������
    //*****************************************************
    private void OnCansel(InputAction.CallbackContext context)
    {
        // �L�����Z���������ꂽ�Ƃ��T�E���h��ʂ��A�N�e�B�u����Ԃ̂Ƃ�
        // �T�E���h��ʂ��A�N�e�B�u��������
        if (sound.activeSelf)
        {
            sound.SetActive(false);                         // �T�E���h��ʂ��A�N�e�B�u��
            option.SetActive(true);                         // �I�v�V������ʂ��A�N�e�B�u��
            GetComponent<SoundSetting>().enabled = false;   // �I�v�V�����̃`�F�b�N�{�b�N�X�𖳌�������
            GetComponent<TutorialOption>().enabled = true;  // �I�v�V�����̃`�F�b�N�{�b�N�X�𖳌�������
        }

        // �J�[�\���ړ��t���O��true�Ȃ�
        if (CurrentMoveFlag)
        {
            Reset();                    // �e�v�f������
            option.SetActive(false);    // �I�v�V������ʂ��A�N�e�B�u��
            return;
        }

        // �I�v�V�������A�N�e�B�u�̂Ƃ��̂݃t���O��ON�ɂ���
        if( option.activeSelf)CurrentMoveFlag = true;   // �J�[�\���ړ��t���O��ON�ɂ���
    }




    //*****************************************************
    // ���Z�b�g
    //*****************************************************

    private void Reset()
    {
        cursol_num = 0;         // �J�[�\���ԍ���������
        currentNum = -1;        // ����ԍ���������
        CurrentMoveFlag = false;// �ړ��t���O��OFF�ɂ���


        // ���̍��W�������ʒu�ɖ߂�
        Vector3 ArrowPos = new Vector3(-40.0f, 25.0f, 0.0f);
        Arrow.localPosition = ArrowPos;
    }
}
