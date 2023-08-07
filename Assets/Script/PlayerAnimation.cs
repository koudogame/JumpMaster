using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] GameObject resultVoiceObj;
    SoundPlayParts resultVoice;
    bool voicePlay = false;

    [Header("Animation")]
    [SerializeField] private bool result = false;
    [SerializeField] private bool title = false;
    [SerializeField] private bool select = false;


    public float animSpeed = 1.5f;              // �A�j���[�V�����Đ����x�ݒ�
    public float lookSmoother = 3.0f;           // a smoothing setting for camera motion
    public bool useCurves = true;               // Mecanim�ŃJ�[�u�������g�����ݒ肷��
                                                // ���̃X�C�b�`�������Ă��Ȃ��ƃJ�[�u�͎g���Ȃ�
    public float useCurvesHeight = 0.5f;        // �J�[�u�␳�̗L������(�n�ʂ����蔲���₷�����ɂ͑傫������)

    // �L�����N�^�[�R���g���[���i�J�v�Z���R���C�_�j�̎Q��
    private CapsuleCollider col;
    private Animator anim;                          // �L�����ɃA�^�b�`�����A�j���[�^�[�ւ̎Q��

    // Start is called before the first frame update
    void Start()
    {
        // Animator�R���|�[�l���g���擾����
        anim = GetComponent<Animator>();
        // CapsuleCollider�R���|�[�l���g���擾����i�J�v�Z���^�R���W�����j
        col = GetComponent<CapsuleCollider>();

        if (resultVoiceObj == null) return;
        resultVoice = resultVoiceObj.GetComponent<SoundPlayParts>();
    }

    // Update is called once per frame
    void Update()
    {
        if (result)
        {
            if (!anim.GetBool("Result")) anim.SetBool("Result", true);
            if (!resultVoice.IsPlaying() && !voicePlay)
            {
                resultVoice.PlaySE();
                voicePlay = true;
            }
        }
        else if (anim.GetBool("Result")) anim.SetBool("Result", false);
    }

    public void SetResultFlag(bool flg) { result = flg; }
    public void SetTitleFlag(bool flg) {  title = flg; }
    public void SetSelectFlag(bool flg) {  select = flg; }
}
