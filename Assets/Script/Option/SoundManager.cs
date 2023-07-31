using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 各音量調整用
    [SerializeField] private AudioSource BGMAudioSource;
    [SerializeField] private AudioSource SEAudioSource;


    // Start is called before the first frame update
    void Start()
    {
        // SoundManagerを検索
        GameObject soundManager = CheckOtherSoundManager();

        // SoundManagerがNULL かつ SoundManagerが無ければtrue
        bool checkResult = soundManager != null && soundManager != gameObject;

        // 無ければ破棄
        if (checkResult) Destroy(gameObject);

        // シーンをまたいでも破棄されないobj作成
        DontDestroyOnLoad(gameObject);
    }





    //*****************************************************
    // BGM
    //*****************************************************
    public float BgmVolume
    {
        get
        {
            return BGMAudioSource.volume;   // BGMのボリュームを返す
        }
        set
        {
            // 指定範囲の値に制限
            BGMAudioSource.volume = Mathf.Clamp01(value);
        }
    }




    //*****************************************************
    // SE用
    //*****************************************************
    public float SeVolume
    {
        get
        {
            return SEAudioSource.volume;    // SEのボリュームを返す
        }
        set
        {
            // 指定範囲の値に制限
            SEAudioSource.volume = Mathf.Clamp01(value);
        }
    }




    //*****************************************************
    // オブジェクトを検索する関数
    //*****************************************************
    GameObject CheckOtherSoundManager()
    {
        // SoundManagerオブジェクトを検索
        return GameObject.FindGameObjectWithTag("SoundManager");
    }
}
