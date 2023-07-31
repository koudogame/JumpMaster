using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // �e���ʒ����p
    [SerializeField] private AudioSource BGMAudioSource;
    [SerializeField] private AudioSource SEAudioSource;


    // Start is called before the first frame update
    void Start()
    {
        // SoundManager������
        GameObject soundManager = CheckOtherSoundManager();

        // SoundManager��NULL ���� SoundManager���������true
        bool checkResult = soundManager != null && soundManager != gameObject;

        // ������Δj��
        if (checkResult) Destroy(gameObject);

        // �V�[�����܂����ł��j������Ȃ�obj�쐬
        DontDestroyOnLoad(gameObject);
    }





    //*****************************************************
    // BGM
    //*****************************************************
    public float BgmVolume
    {
        get
        {
            return BGMAudioSource.volume;   // BGM�̃{�����[����Ԃ�
        }
        set
        {
            // �w��͈͂̒l�ɐ���
            BGMAudioSource.volume = Mathf.Clamp01(value);
        }
    }




    //*****************************************************
    // SE�p
    //*****************************************************
    public float SeVolume
    {
        get
        {
            return SEAudioSource.volume;    // SE�̃{�����[����Ԃ�
        }
        set
        {
            // �w��͈͂̒l�ɐ���
            SEAudioSource.volume = Mathf.Clamp01(value);
        }
    }




    //*****************************************************
    // �I�u�W�F�N�g����������֐�
    //*****************************************************
    GameObject CheckOtherSoundManager()
    {
        // SoundManager�I�u�W�F�N�g������
        return GameObject.FindGameObjectWithTag("SoundManager");
    }
}
