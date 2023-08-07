using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  �����̐ݒ�́AAudio Source �̃C���X�y�N�^�[�� AudioClip ����ݒ肷��
//  ���ʂ⃋�[�v�@�\�Ȃǂׂ̍����ݒ���A�C���X�y�N�^�[����ݒ肷��

[RequireComponent(typeof(AudioSource))]
public class SoundPlayParts : MonoBehaviour
{
    //  ����
    private AudioClip clip;
    //  ����
    private float volume;
    //  �����Ǘ�
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if(CanPlay())
        {
            clip = audioSource.clip;
            volume = audioSource.volume;
        }
    }

    //  �Đ�����(BGM����)
    public void PlayBGM()
    {
        if (!CanPlay()) return;

        //  �Đ�
        audioSource.Play();
    }

    //  �Đ�����(�d���Đ��Ή�,SE����)
    public void PlaySE()
    {
        if (!CanPlay()) return;

        //  �Đ�
        audioSource.PlayOneShot(clip);
    }

    //  �ꎞ��~(��������ƁA��������Đ������)
    public void Pause()
    {
        if (!CanPlay() || !audioSource.isPlaying) return;

        //  �ꎞ��~
        audioSource.Pause();
    }

    //  ��~(��������ƁA�ŏ�����Đ������)
    public void Stop()
    {
        if (!CanPlay() || !audioSource.isPlaying) return;

        //  ��~
        audioSource.Stop();
    }

    //  �ꎞ��~������
    public void UnPause()
    {
        if (!CanPlay() || audioSource.isPlaying) return;

        //  �ꎞ��~����
        audioSource.UnPause();
    }

    // ���ݍĐ������ǂ���
    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }


    //  �Đ��ł��邩
    private bool CanPlay()
    {
        //  audioSource �� null �͍Đ����Ȃ�
        if (audioSource == null)
        {
            Debug.Log(audioSource.clip.name + " �͍Đ��ł��܂���ł���");
            return false;
        }

        //  �������ݒ肳��Ă��Ȃ�
        if (audioSource.clip == null)
        {
            Debug.Log("�������ݒ肳��Ă��܂���ł���");
            return false;
        }

        return true;
    }
}
