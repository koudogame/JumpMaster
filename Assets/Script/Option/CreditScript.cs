using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour
{
    //************************** Fade **************************//

    [Header("Fade"), SerializeField] private Fade fade;



    //****************************** �e�L�X�g�֘A ******************************//

    [Header("�e�L�X�g���W(��)"), SerializeField] private RectTransform text_up_pos;            // �e�L�X�g���W(��)
    [Header("�e�L�X�g���W(�^��)"), SerializeField] private RectTransform text_middle_pos;    // �e�L�X�g���W(�^��)
    [Header("�e�L�X�g���W(��)"), SerializeField] private RectTransform text_down_pos;          // �e�L�X�g���W(��)
    [Header("ThankYouForPlaying"), SerializeField] private RectTransform Thank_Text;           // �e�L�X�g���W(���X�g)
    [Header("��A�N�e�B�u"), SerializeField] private GameObject credit;

    // �e�e�L�X�g�̍��W�p
    private Vector3 kanda_text_pos;
    private Vector3 gondaira_text_pos;
    private Vector3 saito_text_pos;
    private Vector3 thank_text_pos;



    private bool moveFlag;  // �e�L�X�g�����܂ňړ��ł��邩
    float time;             // �o�ߎ��Ԍv���p



    // Start is called before the first frame update
    void Start()
    {
        // �e�e�L�X�g�̍��W���擾
        kanda_text_pos = text_up_pos.localPosition;
        gondaira_text_pos = text_middle_pos.localPosition;
        saito_text_pos = text_down_pos.localPosition;
        thank_text_pos = Thank_Text.localPosition;

        moveFlag = false;   // �t���OOFF
        time = 0.0f;        // �o�ߎ��Ԃ�������
    }




    // Update is called once per frame
    void Update()
    {
        if (TitleDirector.next_num == 2)moveFlag = true;

        // moveFlag��true�Ȃ�ړ���
        if( moveFlag ) MoveAnimetion();
    }




    //*****************************************************
    // �e�L�X�g�A�j���[�V����
    //*****************************************************
    private void MoveAnimetion()
    {
        // �e�e�L�X�g�̍��W����ɃX�N���[��
        // ��ԍŌ�̃e�L�X�g�ȊO�͉�ʊO�܂ŃX�N���[��
        if (saito_text_pos.y <= 400.0f)
        {
            kanda_text_pos.y += 0.5f;
            gondaira_text_pos.y += 0.5f;
            saito_text_pos.y += 0.5f;
        }

        thank_text_pos.y += 0.5f;   // thankYouForPlaying�e�L�X�g���X�N���[��

        // �w��ꏊ�ɒB������
        if (thank_text_pos.y >= 0.0f && moveFlag)
        {
            thank_text_pos.y = 0.0f;    // ���W���Œ�
            time += Time.deltaTime; // �o�ߎ��Ԃ��擾
        }



        // ��ԍŌ�̃e�L�X�g���w��ꏊ�Ɉړ���
        // ���莞�Ԃ���������
        if( time > 2.0f )
        {
            moveFlag = false;           // �ړ�����t���O��OFF
            TitleDirector.isNext = false; // �^�C�g���X�N���v�g�̃V�[���ڍs�t���O��OFF

            // ���W�������ʒu�ɐݒ�
            kanda_text_pos.y = -350.0f;
            gondaira_text_pos.y = -450.0f;
            saito_text_pos.y = -550.0f;
            thank_text_pos.y = -750.0f;
            
        }



        // ���W���Đݒ�
        text_up_pos.localPosition = kanda_text_pos;
        text_middle_pos.localPosition = gondaira_text_pos;
        text_down_pos.localPosition = saito_text_pos;
        Thank_Text.localPosition = thank_text_pos;



        // �ړ�����t���O��OFF�Ȃ珈��
        if (!moveFlag)
        {
            StartCoroutine(fade.FadeOut());     // FadeOut
            credit.SetActive(false);            // ��A�N�e�B�u��
        }
    }
}
