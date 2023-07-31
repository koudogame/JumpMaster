//
//  �Q�[���̐i�s�Ǘ�
//
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [Header("TimeLimit")]
    [SerializeField] float endTime = 0f;
    [SerializeField] float nowTime = 60f;
    [SerializeField] bool startFlag = false;

    [Header("Score")]
    [SerializeField] int score = 0;

    [Header("UI")]
    [SerializeField] private Fade fade;
    [SerializeField] private Sprite startSprite;
    [SerializeField] private Sprite endSprite;
    [SerializeField] private Sprite defSprite;
    [SerializeField] private Sprite[] countSprite = new Sprite[10];


    private GameObject startAndEndUI;
    private GameObject counter1UI;
    private GameObject counter2UI;
    private GameObject countdown1UI;
    private GameObject countdown2UI;
    private bool isClear;            //  �N���A������

    // Start is called before the first frame update
    void Start()
    {
        // ������
        startAndEndUI = GameObject.Find("Start&End");
        counter1UI = GameObject.Find("Counter1");
        counter2UI = GameObject.Find("Counter2");
        countdown1UI = GameObject.Find("Countdown1");
        countdown2UI = GameObject.Find("Countdown2");

        startAndEndUI.GetComponent<Image>().sprite = defSprite;
        nowTime = 5f;
        endTime = 0f;
        startFlag = true;
        isClear = false;

        // �G���[�`�F�b�N
        if (startAndEndUI == null)
        {
            Debug.Log("startAndEndUI��NULL�ł�");
            return;
        }

        if(counter1UI == null)
        {
            Debug.Log("counter1UI��NULL�ł�");
            return;
        }

        if(counter2UI == null) 
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

        ErrorCheck();

        // �t�F�[�h�C��
        StartCoroutine(fade.FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        ErrorCheck();

        ///- �������ԏ���
        // �������ԃJ�E���g�����ƁA�J�n����яI�����}��UI����
        nowTime -= Time.deltaTime;
        if( nowTime < 0f ) nowTime = 0f;

        if( startFlag )
        {
            // �X�^�[�g�O�̃J�E���g�_�E������
            if(nowTime <= 1f) startAndEndUI.GetComponent<Image>().sprite = startSprite;
            else if(nowTime <= 2f) startAndEndUI.GetComponent<Image>().sprite = countSprite[1];
            else if(nowTime <= 3f) startAndEndUI.GetComponent<Image>().sprite = countSprite[2];
            else if(nowTime <= 4f) startAndEndUI.GetComponent<Image>().sprite = countSprite[3];
        }

        if ( startFlag && nowTime <= endTime )
        {
            // �X�^�[�g���̏���
            startAndEndUI.GetComponent<Image>().sprite = defSprite;
            endTime = 0f;
            nowTime = 60f;
            startFlag = false;
        }
        else if( nowTime <= endTime && !isClear )
        {
            // �N���A��̏���
            startAndEndUI.GetComponent<Image>().sprite = endSprite;
            StartCoroutine(/*fade.StartFade(0.0f, 1.0f)*/fade.FadeOut());
            //new WaitForSeconds(12);
            //StartCoroutine(/*fade.StartFade(0.0f, 1.0f)*/fade.FadeOut());
            endTime = 0f;
            nowTime = 3f;
            isClear = true;
        }
        else if (nowTime <= endTime && isClear)
        {
            //StartCoroutine(/*fade.StartFade(0.0f, 1.0f)*/fade.FadeOut());
            isClear = false;
            endTime = 0f;
            nowTime = 0f;
            SceneManager.LoadScene("Result");
        }
        //if(fade.GetFadeAlpha() >= 1f) SceneManager.LoadScene("Result");

        // ��������UI�̏���
        if ( !startFlag && !isClear )
        {
            int time = (int)nowTime;
            // ��������UI�̏\�̈ʂ̏���
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
            // ��������UI�̈�̈ʂ̏���
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
        }


        ///- �X�R�A�J�E���g����
        // 
        if (!startFlag && !isClear)
        {
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
        }
    }

    private void FixedUpdate()
    {
        ////  �N���A
        //if (isClear)
        //{
        //    StartCoroutine(Clear());

        //    //  �N���A������Q�[���I�[�o�[�𖳌��ɂ���
        //    return;
        //}
        //if (nowTime <= endTime && isClear)
        //{
        //    //StartCoroutine(Clear());
        //    // �t�F�[�h�A�E�g
        //    //StartCoroutine(fade.FadeOut());
        //    StartCoroutine(fade.StartFade(0.0f, 1.0f));
        //    //  ���U���g�V�[���ֈڍs
        //    SceneManager.LoadScene("Result");
        //}
    }

    void ErrorCheck()
    {
        if( defSprite == null )
        {
            Debug.Log("defSprite��NULL�ł�");
            return;
        }
        if (startSprite == null)
        {
            Debug.Log("startSprite��NULL�ł�");
            return;
        }
        if (endSprite == null)
        {
            Debug.Log("endSprite��NULL�ł�");
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

    //  �N���A
    private IEnumerator Clear()
    {
        //_eventText.text = "�X�e�[�W�N���A!!";
        //_eventText.color = Color.yellow;

        //  2s�҂�
        //yield return new WaitForSeconds(2);

        // �t�F�[�h�A�E�g
        StartCoroutine(fade.FadeOut());

        ////  2s�҂�
        yield return new WaitForSeconds(2);

        //  ���U���g�V�[���ֈڍs
        SceneManager.LoadScene("Result");
    }

    ////  �N���A��m�点�Ă��炤
    //public void CallClear() { isClear = true; }

    public void ScoreCount() { ++score; }

    bool GetStartFlag() { return startFlag; }
}