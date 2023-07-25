using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public sealed class Fade : MonoBehaviour
{
    [Header("�t�F�[�h�̃C���[�W"), SerializeField] Image fadeImage;
    [Header("�t�F�[�h����"),SerializeField] float duration;
    [Header("alpha�l"), SerializeField] float alpha;



    //*****************************************************
    // �t�F�[�h�C������
    //*****************************************************
    public IEnumerator FadeIn()
    {
        float time = 0.0f; // ���Ԍv���p

        // �w�肵�����Ԍo�߂���܂ŏ���
        while (time < duration)
        {
            // Fade�����Ăяo��
            time = FadeProcess(time, 1.0f, 0.0f);   

            // ���\�b�h�̒��f
            yield return null;
        }

        // �t�F�[�h�C���[�W�𖳌���
        fadeImage.enabled = false;

        //  �I��
        yield return true;
    }




    //*****************************************************+
    // �t�F�[�h�A�E�g����
    //*****************************************************
    public IEnumerator FadeOut()
    {
        float time = 0.0f; // ���Ԍv���p

        // �w�肵�����Ԍo�߂���܂ŏ���
        while (time < duration)
        {
            // Fade�����Ăяo��
            time = FadeProcess(time, 0.0f, 1.0f);   

            // ���\�b�h�̒��f
            yield return null;
        }

        // �t�F�[�h�C���[�W��L����
        fadeImage.enabled = true;

        //  �I��
        yield return true;
    }




    //*****************************************************
    // ���������猳�ɖ߂�Fade
    //*****************************************************
    public IEnumerator HalfFadeOut()
    {
        float time = 0.0f;  // ���Ԍv���p

        // �w�肵�����Ԍo�߂���܂ŏ���
        while (time < duration)
        {
            // Fade�����Ăяo��
            // �w�i�𔼓����ɂ���
            time = FadeProcess(time, 0.0f, 0.5f);

            // ���\�b�h�̒��f
            yield return null;
        }

        // �t�F�[�h�C���[�W��L����
        fadeImage.enabled = true;
    }




    //*****************************************************
    // �����甼�����ɂ���Fade
    //*****************************************************
    public IEnumerator HalfFadeIn()
    {
        float time = 0.0f;  // ���Ԍv���p

        // �w�肵�����Ԍo�߂���܂ŏ���
        while (time < duration)
        {
            // Fade�����Ăяo��
            time = FadeProcess(time, 1.0f, 0.5f);

            // ���\�b�h�̒��f
            yield return null;
        }
        // �t�F�[�h�C���[�W��L����
        fadeImage.enabled = false;
    }




    //*****************************************************
    // Fade���̏���
    //*****************************************************
    float FadeProcess(float currentTime, float Start,float End)
    {
        // �w�肵���͈͂ŃA���t�@�l��⊮
        currentTime += Time.deltaTime;
        alpha = Mathf.Lerp(Start, End, currentTime / duration);

        // �A���t�@�l�ݒ�
        Color imgColor = fadeImage.color;
        fadeImage.color = new Color(imgColor.r, imgColor.g, imgColor.b, alpha);

        // �o�ߎ��Ԃ�Ԃ�
        return currentTime;
    }
}