//
//  �Z���N�g�̐i�s�Ǘ�
//
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectDirector : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] int score = 0;

    [Header("UI")]
    [SerializeField] private Fade fade;
    [SerializeField] private Sprite selectSprite;
    [SerializeField] private Sprite[] modeSprite = new Sprite[2];
    [SerializeField] private Sprite[] levelSprite = new Sprite[3];
    [SerializeField] private GameObject selectUI;
    [SerializeField] private GameObject[] modeUI = new GameObject[2];
    [SerializeField] private GameObject[] levelUI = new GameObject[3];
    [SerializeField] private GameObject modeUIParent;
    [SerializeField] private GameObject levelUIParent;
    [SerializeField] private RectTransform[] modeUIRectTrans = new RectTransform[2];
    [SerializeField] private RectTransform[] levelUIRectTrans = new RectTransform[3];


    //************************** ���͏�� **************************//

    PlayerOperation playerOption;    // �A�N�V�����}�b�v


    //************************** �J�[�\���ړ� **************************//

    [Header("�g�I�u�W�F�N�g"), SerializeField] private RectTransform frame;           // �g�̍��W(rect)
    [Header("�g�̍��W(��)"), SerializeField] private RectTransform up_limit_obj;     //  �J�[�\�������ʒu(��)�ɐݒ肷��obj
    [Header("�g�̍��W(��)"), SerializeField] private RectTransform down_limit_obj;   //  �J�[�\�������ʒu(��)�ɐݒ肷��obj
    private Vector3 up_limit_pos;   //  �J�[�\������Y(��)
    private Vector3 down_limit_pos; //  �J�[�\������Y(��)
    private float move_value;       //  �J�[�\���ړ���񕪂̑����l


    //************************** �I�����֘A **************************//

    //[Header("���݃y�[�W�̑I����"), SerializeField] private List<GameObject> choices; // �I�����̃��X�g
    [Header("���݃y�[�W�̑I����"), SerializeField] private GameObject[] choices; // �I�����̃��X�g
    [Header("�Z���N�g�V�[���̃y�[�W����"), SerializeField] private int pageTotal;
    [Header("�Z���N�g�V�[���̌��݃y�[�W"), SerializeField] private int pageNum;
    public static int select_num;       //  ���쒆�J�[�\���ԍ�
    public static int next_num;         //  ���肵���J�[�\���ԍ�
    public static bool isNext;          //  �y�[�W�ڍs������


    //************************** ��A�N�e�B�u **************************//

    [Header("��A�N�e�B�uOPTION"), SerializeField] private GameObject option;


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
        // ������
        selectUI.GetComponent<Image>().sprite = selectSprite;

        for (int i = 0; i < levelUI.Length; ++i)
        {
            levelUI[i].GetComponent<Image>().sprite = levelSprite[i];
        }

        for (int i = 0; i < modeUI.Length; ++i)
        {
            modeUI[i].GetComponent <Image>().sprite = modeSprite[i];
        }

        modeUIParent.SetActive(true);
        levelUIParent.SetActive(false);

        choices = new GameObject[modeUI.Length];
        choices[0] = modeUI[0];
        choices[1] = modeUI[1];
        //choices = new List<GameObject> { modeUI[0], modeUI[1] };

        select_num = 0; //  ���݂̔ԍ�
        next_num = -1;  //  ���肵���ԍ�
        isNext = false; //  ���̃V�[���ֈڍs����t���O�Ǘ�

        //  �g�̏������W��ݒ�
        up_limit_pos = up_limit_obj.localPosition;
        down_limit_pos = down_limit_obj.localPosition;
        var prev_pos = frame.localPosition;
        frame.localPosition = new Vector3(
            prev_pos.x, up_limit_pos.y, prev_pos.z);

        //  �J�[�\���̑����l��ݒ�
        //move_value = Mathf.Abs(down_limit_pos.y) / (float)GetSelectMax();
        move_value = up_limit_pos.y - down_limit_pos.y;

        // �G���[�`�F�b�N
        ErrorCheck();

        // �t�F�[�h�C��
        StartCoroutine(fade.FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        ErrorCheck();
    }


    //*****************************************************
    //  ����
    //*****************************************************
    private void OnEnter(InputAction.CallbackContext context)
    {
        //  ��x���ڍs���Ă��Ȃ��� or ���͂��ꂽ�u�ԈȊO�Ȃ珈�����Ȃ�
        if (isNext || !context.started) return;

        // �I��ԍ������肷��
        next_num = select_num;

        //  ���̃y�[�W�A�܂��̓V�[���ֈڍs
        if(pageNum == 0)
        {
            SelectModeSingleton.Instance.SetMode(choices[next_num].name);

            if(next_num == 0)
            {
                choices = null;
                choices = new GameObject[levelUI.Length];
                for (int i = 0; i < levelUI.Length; ++i)
                {
                    choices[i] = levelUI[i];
                }
            }
            else if(next_num == 1)
            {
                choices = null;
                choices = new GameObject[levelUI.Length];
                for (int i = 0; i < levelUI.Length; ++i)
                {
                    choices[i] = levelUI[i];
                }
            }

            //  �g�̏������W��ݒ�
            up_limit_obj = levelUIRectTrans[0];
            down_limit_obj = levelUIRectTrans[1];
            up_limit_pos = up_limit_obj.localPosition;
            down_limit_pos = down_limit_obj.localPosition;
            var prev_pos = frame.localPosition;
            frame.localPosition = new Vector3(
                prev_pos.x, up_limit_pos.y, prev_pos.z);

            //  �J�[�\���̑����l��ݒ�
            move_value = up_limit_pos.y - down_limit_pos.y;

            modeUIParent.SetActive(false);
            levelUIParent.SetActive(true);
            pageNum = 1;
        }
        else if(pageNum == 1)
        {
            SelectModeSingleton.Instance.SetLevel(choices[next_num].name);
            Next();
        }
        //if (next_num == 0) Next();                // �V�[���ڍs����
        //else if (next_num == 1)
        //{
        //    StartCoroutine(fade.HalfFadeOut()); // �t�F�[�h�A�E�g
        //    Invoke("OptionProcess", 1); // fadeOut���Ă���\��
        //}
        //else if (next_num == 2)
        //{
        //    StartCoroutine(fade.HalfFadeOut()); // �t�F�[�h�A�E�g
        //    Invoke("CreditProcess", 1); // fadeOut���Ă���\��
        //}
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
        //if (select_num >= choices.Count) select_num = 0;
        if (select_num >= choices.Length) select_num = 0;
        else if (select_num < 0) select_num = GetSelectMax();

        //  �g�ړ�, Y �̂� ����Y + (�J�[�\����񕪂̈ړ��� * �J�[�\���ԍ�)
        var prev_pos = frame.localPosition;
        frame.localPosition = new Vector3(
            prev_pos.x, up_limit_pos.y - (move_value * select_num), prev_pos.z
            );
    }

    //*****************************************************
    //  ���̃V�[���ֈڍs(�ڍs�ł��邩������܂�)
    //*****************************************************
    private void Next()
    {
        //  ��x���ڍs���Ă��Ȃ��� or ���[�h���Ȃ��ԍ��͈̔͂�
        if (isNext /*|| next_num >= 1*/) return;

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
    private int GetSelectMax() { return /*choices.Count - 1*/choices.Length - 1; }

    //*****************************************************
    // �I�v�V��������
    //*****************************************************
    private void OptionProcess()
    {
        isNext = true;                                  // �V�[���ڍs�t���O��ON
        option.SetActive(true);                         // �A�N�e�B�u��
        GetComponent<SoundSetting>().enabled = true;    // �I�v�V�����X�N���v�g�̗L����
        SoundSetting.CurrentMoveFlag = true;            // �T�E���h�X�N���v�g�̃J�[�\���t���O��L����
        GetComponent<SelectDirector>().enabled = false;    // �^�C�g���X�N���v�g������
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
        //option.SetActive(false);                        // �I�v�V�������A�N�e�B�u��
        GetComponent<SelectDirector>().enabled = true;     // �^�C�g���X�N���v�g�̗L����
        GetComponent<SoundSetting>().enabled = false;   // �I�v�V�����X�N���v�g�̖�����
    }

    //*****************************************************
    // �V�[���̃��[�h
    //*****************************************************
    private void LoadScene()
    {
        //  �t�F�[�h���I������烍�[�h
        SceneManager.LoadScene("Game");
    }

    void ErrorCheck()
    {
        if (modeUIParent == null)
        {
            Debug.Log("modeUIParent��NULL�ł�");
            return;
        }

        if (levelUIParent == null)
        {
            Debug.Log("levelUIParent��NULL�ł�");
            return;
        }

        if (selectUI == null)
        {
            Debug.Log("selectUI��NULL�ł�");
            return;
        }

        if (selectSprite == null)
        {
            Debug.Log("selectSprite��NULL�ł�");
            return;
        }


        for (int i = 0; i < levelSprite.Length; ++i)
        {
            if (levelSprite[i] == null)
            {
                Debug.Log("levelSprite[ " + i + " ]��NULL�ł�");
                return;
            }
        }

        for (int i = 0; i < modeSprite.Length; ++i)
        {
            if (modeSprite[i] == null)
            {
                Debug.Log("modeSprite[ " + i + " ]��NULL�ł�");
                return;
            }
        }

        for (int i = 0; i < levelUI.Length; ++i)
        {
            if (levelUI[i] == null)
            {
                Debug.Log("levelUI[ " + i + " ]��NULL�ł�");
                return;
            }
        }

        for (int i = 0; i < modeUI.Length; ++i)
        {
            if (modeUI[i] == null)
            {
                Debug.Log("modeUI[ " + i + " ]��NULL�ł�");
                return;
            }
        }

        for (int i = 0; i < levelUIRectTrans.Length; ++i)
        {
            if (levelUIRectTrans[i] == null)
            {
                Debug.Log("levelUIRectTrans[ " + i + " ]��NULL�ł�");
                return;
            }
        }

        for (int i = 0; i < modeUIRectTrans.Length; ++i)
        {
            if (modeUIRectTrans[i] == null)
            {
                Debug.Log("modeUIRectTrans[ " + i + " ]��NULL�ł�");
                return;
            }
        }
    }
}
