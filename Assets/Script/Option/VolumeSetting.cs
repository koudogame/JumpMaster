using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    // �T�E���h�̎��
    public enum VolumeType { NULL,BGM,SE };
    [SerializeField] private VolumeType volumeType = 0;

    private Slider slider;  // �X���C�_�[
    private SoundManager soundManager;  // �T�E���h�}�l�[�W���[

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();    // �X���C�_�[���擾
        soundManager = FindObjectOfType<SoundManager>();    // �T�E���h�}�l�[�W���[���擾
        SetSliderValue();   // ���݂̉��ʂ�ݒ�
    }




    //*****************************************************
    // �X���C�_�[��ς����特�ʕύX
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
    // �X���C�_�[�Ɍ��݂̉��ʂ�ݒ�
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
