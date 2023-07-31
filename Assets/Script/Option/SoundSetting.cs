using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    //****************�@�I�v�V�����̖��O�@ ****************//

    private enum OptionName 
    { 
        TITLE,
        TUTORIAL,
        STAGESELECT,
        GAME
    }
    [SerializeField] private OptionName SceneName;



    //****************�@���͏��@ ****************//

    PlayerOperation playerOption;  // �A�N�V�����}�b�v



    //****************�@��A�N�e�B�u�@ ****************//

    [Header("��A�N�e�B�uSOUND"), SerializeField] private GameObject sound;



    //****************�@�t�F�[�h�@ ****************//

    [Header("Fade"), SerializeField] private Fade fade;


    //****************�@�{�����[���������鉹�̎�ށ@ ****************//

    public enum VolumeType { NULL, BGM, SE }
    [SerializeField] private VolumeType volumeType = 0;



    //****************�@���ʐݒ�p�@ ****************//

    public Slider bgmSlider;
    public Slider seSlider;
    public SoundManager soundManager;
    public static AudioSource audioSource;



    //****************�@�J�[�\���̏��@ ****************//

    [Header("�J�[�\���ԍ�"), SerializeField] private int cursol_num;                   // ���݂̑I�����ԍ�
    [Header("�J�[�\�����X�g"), SerializeField] private List<string> CursolList;        // �I�����̃��X�g
    [Header("�J�[�\�����I�u�W�F�N�g"), SerializeField] private RectTransform Arrow;  // ���̍��W
    [Header("���̍��W(��)"), SerializeField] private RectTransform upLimit;          // ������ʒu�ɐݒ肷��Obj 
    [Header("���̍��W(��)"), SerializeField] private RectTransform downLimit;        // ��󉺌��ʒu�ɐݒ肷��Obj
    private Vector3 upLimitPos;     // �J�[�\������ʒuY
    private int currentNum;         // ���莞�̔ԍ�
    public static bool CurrentMoveFlag; // �J�[�\���ړ��\�t���O




    private void Awake()
    {
        // �C���X�^���X����
        playerOption = new PlayerOperation();

        // �A�N�V���������s���ꂽ�Ƃ��Ăяo����郁�\�b�h��o�^
        playerOption.Option.Enter.started += context => OnEnter(context);
        playerOption.Option.MoveCursol.started += context => OnMoveCursol(context);
        playerOption.Option.SliderMove.started += context => OnSliderMove(context);
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
    private void Start()
    {
        cursol_num = 0;                             // �I�����̔ԍ�
        upLimitPos = upLimit.localPosition;         // ������W
        currentNum = -1;                            // ����ԍ�
        CurrentMoveFlag = true;                     // �ړ��\
        
        // GameObject��T����SoundManager�^�ŕԂ�
        soundManager = FindObjectOfType<SoundManager>();

        // �T�E���h�̉��ʐݒ�
        bgmSlider.value = soundManager.BgmVolume;
        seSlider.value = soundManager.SeVolume;
    }



    //*****************************************************
    // ����
    //*****************************************************
    private void OnEnter(InputAction.CallbackContext context)
    {
        // ���͂��ꂽ�u�ԈȊO�Ȃ珈�����Ȃ�
        if (!context.started) return;

        // �ŏ��͏��������Ȃ�
        if (!CurrentMoveFlag)
        {
            CurrentMoveFlag = true;  // �J�[�\���ړ��t���O��ON�ɐݒ�
            return;
        }
        else CurrentMoveFlag = false;   // �ړ��t���O��OFF


        currentNum = cursol_num;    // ���莞�̔ԍ����擾

        // ����ԍ��ɉ����ăT�E���h�̎�ނ�ύX
        if (currentNum == 0) volumeType = VolumeType.BGM;
        else if (currentNum == 1) volumeType = VolumeType.SE;

        // �߂��I�����Ă�����
        if (currentNum == GetSelectMax())
        {
            Reset();    // �e�v�f������
            StartCoroutine(fade.FadeIn());  // �t�F�[�h�A�E�g
            return;
        }
    }




    //*****************************************************
    //  �J�[�\���ړ�
    //*****************************************************
    private void OnMoveCursol(InputAction.CallbackContext context)
    {
        // ���͂��ꂽ�u�ԈȊO�Ȃ珈�����Ȃ�
        if ( !context.started) return;

        // �J�[�\���ړ��t���O��ON�Ȃ�
        if (CurrentMoveFlag)
        {
            float value = context.ReadValue<float>();   // ���͒l�擾

            // 0.�`�̒l��1,-1�ɕϊ�����
            if (value > 0.0f) value = -1.0f;
            else if (value < 0.0f) value = 1.0f;

            cursol_num += (int)value;   // �J�[�\���J�E���g��i�߂�

            // �J�[�\���̔ԍ��␳
            if (cursol_num >= CursolList.Count) cursol_num = 0;
            else if (cursol_num < 0) cursol_num = GetSelectMax();

            //  �g�ړ�, Y �̂� ����Y + (�J�[�\����񕪂̈ړ��� * �J�[�\���ԍ�)
            var prev_pos = Arrow.localPosition;
            Arrow.localPosition = new Vector3(prev_pos.x, upLimitPos.y - (20 * cursol_num), prev_pos.z);
        }
    }




    //*****************************************************
    // ���X�g�̍ő吔�擾
    //*****************************************************
    private int GetSelectMax()
    {
        return CursolList.Count - 1;
    }



    //*****************************************************
    // �L�����Z������
    //*****************************************************
    private void OnCansel(InputAction.CallbackContext context)
    {
        // ���͂��ꂽ�u�ԈȊO�Ȃ珈�����Ȃ�
        if (!context.started) return;

        // �J�[�\���ړ��t���O��true�Ȃ�
        if (CurrentMoveFlag)
        {
            Reset();    // �e�v�f������
            return;
        }

        CurrentMoveFlag = true;   // �J�[�\���ړ��t���O��ON�ɂ���
    }




    //*****************************************************
    // �X���C�_�[�̒l��ύX����֐�
    //*****************************************************
    private void OnSliderMove(InputAction.CallbackContext context)
    {
        // ���͂��ꂽ�u�ԈȊO�Ȃ珈�����Ȃ�
        if (!context.started) return;

        // �J�[�\���ړ��t���O��OFF�̂Ƃ��̂ݏ���
        if (!CurrentMoveFlag)
        {
            float value = context.ReadValue<float>();   // ���͎��

            // �ړ��ʂ�ݒ�
            if (value > 0.0f) value = 0.1f;
            else if (value < 0.0f) value = -0.1f;

            // ���̎�ނ���ǂ���̃X���C�_�[��ω������邩���߂�
            if (volumeType == VolumeType.BGM)
            {
                bgmSlider.value += value; // �X���C�_�[�̒l�ύX

                // �͈͐ݒ�
                if (bgmSlider.value > bgmSlider.maxValue) bgmSlider.value = bgmSlider.maxValue;
                else if (bgmSlider.value < bgmSlider.minValue) bgmSlider.value = bgmSlider.minValue;
            }
            else if (volumeType == VolumeType.SE)
            {
                seSlider.value += value; // �X���C�_�[�̒l�ύX

                // �͈͐ݒ�
                if (seSlider.value > seSlider.maxValue) seSlider.value = seSlider.maxValue;
                else if (seSlider.value < seSlider.minValue) seSlider.value = seSlider.minValue;
            }
        }
    }





    //*****************************************************
    // ���Z�b�g
    //*****************************************************
    private void Reset()
    {
        cursol_num = 0;                 // �J�[�\���ԍ�������
        currentNum = -1;                // ����ԍ���������
        CurrentMoveFlag = false;        // �ړ��t���O��OFF�ɂ���
        volumeType = VolumeType.NULL;   // �T�E���h��ޏ�����


        switch(SceneName)
        {
            case OptionName.TITLE:
                GetComponent<TitleDirector>().enabled = true; // �^�C�g���X�N���v�g�L����
                TitleDirector.isNext = false;                 // �V�[���ړ��t���O�𖳌���
                break;
            case OptionName.TUTORIAL:
                GetComponent<TutorialOption>().enabled = true;  // �`���[�g���A���I�v�V�����X�N���v�g�L����
                Search();                                       // �I�u�W�F�N�g����
                TutorialOption.CurrentMoveFlag = true;          // �`���[�g���A���I�v�V�����̃J�[�\���t���O��ON
                break;
            case OptionName.STAGESELECT or OptionName.GAME:
                GetComponent<StageSelectOption>().enabled = true;   // �X�e�[�W�Z���N�g�I�v�V�����̃X�N���v�g�L����
                Search();                                           // �I�u�W�F�N�g����
                StageSelectOption.CurrentMoveFlag = true;           // �X�e�[�W�Z���N�g�I�v�V�����̃J�[�\���t���O��L����
                break;
        }


        sound.SetActive (false);    // �T�E���h�I�v�V�������A�N�e�B�u��
        
        // ���̍��W�������ʒu�ɖ߂�
        Vector3 ArrowPos = new Vector3(-40.0f, 20.0f, 0.0f);
        Arrow.localPosition = ArrowPos;
        
        GetComponent<SoundSetting>().enabled = false;     // �I�v�V�����X�N���v�g�̖�����
    }



    //*****************************************************
    // ��������
    //*****************************************************
    private void Search()
    {
        // �I�u�W�F�N�g����
        GameObject option = GameObject.Find("Canvas").transform.Find("Option").gameObject;

        // ���������I�u�W�F�N�g���A�N�e�B�u��
        option.SetActive(true);
    }
}
