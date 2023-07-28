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
            //Debug.Log("FadeOut:ON");

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

    // �t�F�[�h����
    public IEnumerator StartFade(float Start, float End)
    {
        GameObject startAndEndUI = GameObject.Find("Start&End");
        float time = 0.0f; // ���Ԍv���p
        Image img = startAndEndUI.GetComponent<Image>(); // �R���|�[�l���g���擾

        while (true)
        {
            yield return null;

            // Lerp���\�b�h���g���ăA���t�@�l����
            float a = Mathf.Lerp(Start, End, time);
            //img.color = new Color(1.0f, 1.0f, 1.0f, a);
            Color imgColor = fadeImage.color;
            fadeImage.color = /*new Color(imgColor.r, imgColor.g, imgColor.b, a)*/new Color(1.0f, 1.0f, 1.0f, a);
            alpha = a;
            // ��ԂɎg�����Ԃ�i�߂�
            time += Time.deltaTime;

            // 1�b�o�߂�����I��
            if (time > 1f)
            {
                // �덷���o��Ǝv���̂ŔO�̂��ߏ㏑��
                //img.color = new Color(1.0f, 1.0f, 1.0f, End);
                fadeImage.color = /*new Color(imgColor.r, imgColor.g, imgColor.b, End)*/new Color(1.0f, 1.0f, 1.0f, a);
                break;
            }
        }
    }

    public float GetFadeAlpha()
    {
        return alpha;
    }
}