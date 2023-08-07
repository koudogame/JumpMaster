//
//  ���U���g�̐i�s�Ǘ�
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class ResultDirector : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] int score = 0;
    [SerializeField] int time = 0;

    [Header("UI")]
    [SerializeField] private Fade fade;
    [SerializeField] private Sprite resultSprite;
    [SerializeField] private Sprite scoreSprite;
    [SerializeField] private Sprite timeSprite;
    [SerializeField] private Sprite[] countSprite = new Sprite[10];

    [Header("Player")]
    [SerializeField] private PlayerAnimation playerAnim;

    [Header("Sound")]
    [SerializeField] private SoundPlayParts bgm;
    [SerializeField] private SoundPlayParts moveSE;
    [SerializeField] private SoundPlayParts pushSE;


    //************************** ���͏�� **************************//

    PlayerOperation playerOption;    // �A�N�V�����}�b�v


    //************************** �J�[�\���ړ� **************************//

    [Header("�g�I�u�W�F�N�g"), SerializeField] private RectTransform frame;           // �g�̍��W(rect)
    [Header("�g�̍��W(��)"), SerializeField] private RectTransform up_limit_obj;     //  �J�[�\�������ʒu(��)�ɐݒ肷��obj
    [Header("�g�̍��W(��)"), SerializeField] private RectTransform down_limit_obj;   //  �J�[�\�������ʒu(��)�ɐݒ肷��obj
    [Header("�J�[�\���ړ���"), SerializeField] private float move_value;
    private Vector3 up_limit_pos;   //  �J�[�\������Y(��)
    private Vector3 down_limit_pos; //  �J�[�\������Y(��)
    //private float move_value;       //  �J�[�\���ړ���񕪂̑����l


    //************************** �I�����֘A **************************//

    //[Header("���݃y�[�W�̑I����"), SerializeField] private List<GameObject> choices; // �I�����̃��X�g
    [Header("���݃y�[�W�̑I����"), SerializeField] private GameObject[] choices; // �I�����̃��X�g
    [Header("�I�������V�[���̖��O"), SerializeField] private string sceneName = "";
    public static int select_num;       //  ���쒆�J�[�\���ԍ�
    public static int next_num;         //  ���肵���J�[�\���ԍ�
    public static bool isNext;          //  �y�[�W�ڍs������


    //************************** ��A�N�e�B�u **************************//

    [Header("��A�N�e�B�uOPTION"), SerializeField] private GameObject option;


    private GameObject resultUI;
    private GameObject counter1UI;
    private GameObject counter2UI;
    private GameObject countdown1UI;
    private GameObject countdown2UI;


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
        resultUI = GameObject.Find("ResultUI");
        counter1UI = GameObject.Find("Counter1");
        counter2UI = GameObject.Find("Counter2");
        countdown1UI = GameObject.Find("Countdown1");
        countdown2UI = GameObject.Find("Countdown2");
        //sceneSetter = GetComponent<ScoreGetter>();

        resultUI.GetComponent<Image>().sprite = resultSprite;
        score = GameScoreSingleton.Instance.GetScore();
        time = GameScoreSingleton.Instance.GetTime();

        // �G���[�`�F�b�N
        if (resultUI == null)
        {
            Debug.Log("resultUI��NULL�ł�");
            return;
        }

        if (counter1UI == null)
        {
            Debug.Log("counter1UI��NULL�ł�");
            return;
        }

        if (counter2UI == null)
        {
            Debug.Log("counter2UI��NULL�ł�");
            return;
        }

        if (countdown1UI == null)
        {
            Debug.Log("countdown1UI��NULL�ł�");
            return;
        }

        if (countdown2UI == null)
        {
            Debug.Log("countdown2UI��NULL�ł�");
            return;
        }

        if (playerAnim == null)
        {
            Debug.Log("playerAnim��NULL�ł�");
            return;
        }

        ErrorCheck();


        ///- �N���A���ԏ���
        // �N���A����UI�̏\�̈ʂ̏���
        switch (time / 10)
        {
            case 0: countdown1UI.GetComponent<Image>().sprite = countSprite[0]; break;
            case 1: countdown1UI.GetComponent<Image>().sprite = countSprite[1]; break;
            case 2: countdown1UI.GetComponent<Image>().sprite = countSprite[2]; break;
            case 3: countdown1UI.GetComponent<Image>().sprite = countSprite[3]; break;
            case 4: countdown1UI.GetComponent<Image>().sprite = countSprite[4]; break;
            case 5: countdown1UI.GetComponent<Image>().sprite = countSprite[5]; break;
            case 6: countdown1UI.GetComponent<Image>().sprite = countSprite[6]; break;
            case 7: countdown1UI.GetComponent<Image>().sprite = countSprite[7]; break;
            case 8: countdown1UI.GetComponent<Image>().sprite = countSprite[8]; break;
            case 9: countdown1UI.GetComponent<Image>().sprite = countSprite[9]; break;
            default: break;
        }
        // �N���A����UI�̈�̈ʂ̏���
        switch (time % 10)
        {
            case 0: countdown2UI.GetComponent<Image>().sprite = countSprite[0]; break;
            case 1: countdown2UI.GetComponent<Image>().sprite = countSprite[1]; break;
            case 2: countdown2UI.GetComponent<Image>().sprite = countSprite[2]; break;
            case 3: countdown2UI.GetComponent<Image>().sprite = countSprite[3]; break;
            case 4: countdown2UI.GetComponent<Image>().sprite = countSprite[4]; break;
            case 5: countdown2UI.GetComponent<Image>().sprite = countSprite[5]; break;
            case 6: countdown2UI.GetComponent<Image>().sprite = countSprite[6]; break;
            case 7: countdown2UI.GetComponent<Image>().sprite = countSprite[7]; break;
            case 8: countdown2UI.GetComponent<Image>().sprite = countSprite[8]; break;
            case 9: countdown2UI.GetComponent<Image>().sprite = countSprite[9]; break;
            default: break;
        }

        ///- �X�R�A�J�E���g����
        // �X�R�A�J�E���^UI�̏\�̈ʂ̏���
        switch (score / 10)
        {
            case 0: counter1UI.GetComponent<Image>().sprite = countSprite[0]; break;
            case 1: counter1UI.GetComponent<Image>().sprite = countSprite[1]; break;
            case 2: counter1UI.GetComponent<Image>().sprite = countSprite[2]; break;
            case 3: counter1UI.GetComponent<Image>().sprite = countSprite[3]; break;
            case 4: counter1UI.GetComponent<Image>().sprite = countSprite[4]; break;
            case 5: counter1UI.GetComponent<Image>().sprite = countSprite[5]; break;
            case 6: counter1UI.GetComponent<Image>().sprite = countSprite[6]; break;
            case 7: counter1UI.GetComponent<Image>().sprite = countSprite[7]; break;
            case 8: counter1UI.GetComponent<Image>().sprite = countSprite[8]; break;
            case 9: counter1UI.GetComponent<Image>().sprite = countSprite[9]; break;
            default: break;
        }
        // �X�R�A�J�E���^UI�̈�̈ʂ̏���
        switch (score % 10)
        {
            case 0: counter2UI.GetComponent<Image>().sprite = countSprite[0]; break;
            case 1: counter2UI.GetComponent<Image>().sprite = countSprite[1]; break;
            case 2: counter2UI.GetComponent<Image>().sprite = countSprite[2]; break;
            case 3: counter2UI.GetComponent<Image>().sprite = countSprite[3]; break;
            case 4: counter2UI.GetComponent<Image>().sprite = countSprite[4]; break;
            case 5: counter2UI.GetComponent<Image>().sprite = countSprite[5]; break;
            case 6: counter2UI.GetComponent<Image>().sprite = countSprite[6]; break;
            case 7: counter2UI.GetComponent<Image>().sprite = countSprite[7]; break;
            case 8: counter2UI.GetComponent<Image>().sprite = countSprite[8]; break;
            case 9: counter2UI.GetComponent<Image>().sprite = countSprite[9]; break;
            default: break;
        }

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

        playerAnim.SetResultFlag(true);
        bgm.PlayBGM();

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

        pushSE.PlaySE();

        // �I��ԍ������肷��
        next_num = select_num;

        //  ���̃y�[�W�A�܂��̓V�[���ֈڍs
        if (next_num == 0)
        {
            StartCoroutine(fade.HalfFadeOut()); // �t�F�[�h�A�E�g
            sceneName = "Game";
            Next();                // �V�[���ڍs����
        }
        else if (next_num == 1)
        {
            StartCoroutine(fade.HalfFadeOut()); // �t�F�[�h�A�E�g
            sceneName = "Select";
            Next();                // �V�[���ڍs����
        }
        else if (next_num == 2)
        {
            StartCoroutine(fade.HalfFadeOut()); // �t�F�[�h�A�E�g
            sceneName = "Title";
            Next();                // �V�[���ڍs����
        }
    }

    //*****************************************************
    //  �J�[�\���ړ�
    //*****************************************************
    private void OnMoveCursol(InputAction.CallbackContext context)
    {
        //  �t�F�[�h���͏������Ȃ� or ���͂��ꂽ�u�ԈȊO�Ȃ珈�����Ȃ�
        if (isNext || !context.started) return;

        moveSE.PlaySE();

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
        SceneManager.LoadScene(sceneName);
    }

    void ErrorCheck()
    {
        if (resultSprite == null)
        {
            Debug.Log("resultSprite��NULL�ł�");
            return;
        }
        if (scoreSprite == null)
        {
            Debug.Log("scoreSprite��NULL�ł�");
            return;
        }
        if (timeSprite == null)
        {
            Debug.Log("timeSprite��NULL�ł�");
            return;
        }

        for (int i = 0; i < countSprite.Length; ++i)
        {
            if (countSprite[i] == null)
            {
                Debug.Log("countSprite[ " + i + " ]��NULL�ł�");
                return;
            }
        }
    }
}
