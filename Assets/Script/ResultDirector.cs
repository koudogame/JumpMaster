//
//  ���U���g�̐i�s�Ǘ�
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDirector : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] int score = 0;
    [SerializeField] int time = 0;
    [SerializeField] private ScoreGetter sceneSetter;

    [Header("UI")]
    [SerializeField] private Fade fade;
    [SerializeField] private Sprite resultSprite;
    [SerializeField] private Sprite scoreSprite;
    [SerializeField] private Sprite timeSprite;
    [SerializeField] private Sprite[] countSprite = new Sprite[10];


    private GameObject resultUI;
    private GameObject counter1UI;
    private GameObject counter2UI;
    private GameObject countdown1UI;
    private GameObject countdown2UI;

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
        //score = GetComponent<ScoreGetter>().GetScore();
        //time = GetComponent<ScoreGetter>().GetTime();
        if (GameScoreSingleton.Instance != null)
        {
            score = GameScoreSingleton.Instance.Score;
            time = GameScoreSingleton.Instance.Time;
        }

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


        // �t�F�[�h�C��
        StartCoroutine(fade.FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        ErrorCheck();
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
