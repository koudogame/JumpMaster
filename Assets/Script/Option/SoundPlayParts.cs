using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  音源の設定は、Audio Source のインスペクターの AudioClip から設定する
//  音量やループ機能などの細かい設定も、インスペクターから設定する

[RequireComponent(typeof(AudioSource))]
public class SoundPlayParts : MonoBehaviour
{
    //  音源
    private AudioClip clip;
    //  音量
    private float volume;
    //  音源管理
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

    //  再生する(BGM向け)
    public void PlayBGM()
    {
        if (!CanPlay()) return;

        //  再生
        audioSource.Play();
    }

    //  再生する(重複再生対応,SE向け)
    public void PlaySE()
    {
        if (!CanPlay()) return;

        //  再生
        audioSource.PlayOneShot(clip);
    }

    //  一時停止(解除すると、続きから再生される)
    public void Pause()
    {
        if (!CanPlay() || !audioSource.isPlaying) return;

        //  一時停止
        audioSource.Pause();
    }

    //  停止(解除すると、最初から再生される)
    public void Stop()
    {
        if (!CanPlay() || !audioSource.isPlaying) return;

        //  停止
        audioSource.Stop();
    }

    //  一時停止を解除
    public void UnPause()
    {
        if (!CanPlay() || audioSource.isPlaying) return;

        //  一時停止解除
        audioSource.UnPause();
    }

    // 現在再生中かどうか
    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }


    //  再生できるか
    private bool CanPlay()
    {
        //  audioSource が null は再生しない
        if (audioSource == null)
        {
            Debug.Log(audioSource.clip.name + " は再生できませんでした");
            return false;
        }

        //  音源が設定されていない
        if (audioSource.clip == null)
        {
            Debug.Log("音源が設定されていませんでした");
            return false;
        }

        return true;
    }
}
