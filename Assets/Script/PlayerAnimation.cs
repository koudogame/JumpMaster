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


    public float animSpeed = 1.5f;              // アニメーション再生速度設定
    public float lookSmoother = 3.0f;           // a smoothing setting for camera motion
    public bool useCurves = true;               // Mecanimでカーブ調整を使うか設定する
                                                // このスイッチが入っていないとカーブは使われない
    public float useCurvesHeight = 0.5f;        // カーブ補正の有効高さ(地面をすり抜けやすい時には大きくする)

    // キャラクターコントローラ（カプセルコライダ）の参照
    private CapsuleCollider col;
    private Animator anim;                          // キャラにアタッチされるアニメーターへの参照

    // Start is called before the first frame update
    void Start()
    {
        // Animatorコンポーネントを取得する
        anim = GetComponent<Animator>();
        // CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
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
