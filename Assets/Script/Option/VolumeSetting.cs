using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    // サウンドの種類
    public enum VolumeType { NULL,BGM,SE };
    [SerializeField] private VolumeType volumeType = 0;

    private Slider slider;  // スライダー
    private SoundManager soundManager;  // サウンドマネージャー

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();    // スライダー情報取得
        soundManager = FindObjectOfType<SoundManager>();    // サウンドマネージャー情報取得
        SetSliderValue();   // 現在の音量を設定
    }




    //*****************************************************
    // スライダーを変えたら音量変更
    //*****************************************************

    public void OnValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.BGM:
                soundManager.BgmVolume = slider.value;
                break;
            case VolumeType.SE:
                soundManager.SeVolume = slider.value;
                break;
        }
    }



    //*****************************************************
    // スライダーに現在の音量を設定
    //*****************************************************

    public void SetSliderValue()
    {
        switch (volumeType)
        {
            case VolumeType.BGM:
                slider.value = soundManager.BgmVolume;
                break;
            case VolumeType.SE:
                slider.value = soundManager.SeVolume;
                break;
        }
    }
}
