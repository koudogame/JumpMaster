using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour
{
    //************************** Fade **************************//

    [Header("Fade"), SerializeField] private Fade fade;



    //****************************** テキスト関連 ******************************//

    [Header("テキスト座標(上)"), SerializeField] private RectTransform text_up_pos;            // テキスト座標(上)
    [Header("テキスト座標(真ん中)"), SerializeField] private RectTransform text_middle_pos;    // テキスト座標(真ん中)
    [Header("テキスト座標(下)"), SerializeField] private RectTransform text_down_pos;          // テキスト座標(下)
    [Header("ThankYouForPlaying"), SerializeField] private RectTransform Thank_Text;           // テキスト座標(ラスト)
    [Header("非アクティブ"), SerializeField] private GameObject credit;

    // 各テキストの座標用
    private Vector3 kanda_text_pos;
    private Vector3 gondaira_text_pos;
    private Vector3 saito_text_pos;
    private Vector3 thank_text_pos;



    private bool moveFlag;  // テキストがいつまで移動できるか
    float time;             // 経過時間計測用



    // Start is called before the first frame update
    void Start()
    {
        // 各テキストの座標を取得
        kanda_text_pos = text_up_pos.localPosition;
        gondaira_text_pos = text_middle_pos.localPosition;
        saito_text_pos = text_down_pos.localPosition;
        thank_text_pos = Thank_Text.localPosition;

        moveFlag = false;   // フラグOFF
        time = 0.0f;        // 経過時間を初期化
    }




    // Update is called once per frame
    void Update()
    {
        if (TitleDirector.next_num == 2)moveFlag = true;

        // moveFlagがtrueなら移動可
        if( moveFlag ) MoveAnimetion();
    }




    //*****************************************************
    // テキストアニメーション
    //*****************************************************
    private void MoveAnimetion()
    {
        // 各テキストの座標を上にスクロール
        // 一番最後のテキスト以外は画面外までスクロール
        if (saito_text_pos.y <= 400.0f)
        {
            kanda_text_pos.y += 0.5f;
            gondaira_text_pos.y += 0.5f;
            saito_text_pos.y += 0.5f;
        }

        thank_text_pos.y += 0.5f;   // thankYouForPlayingテキストをスクロール

        // 指定場所に達したら
        if (thank_text_pos.y >= 0.0f && moveFlag)
        {
            thank_text_pos.y = 0.0f;    // 座標を固定
            time += Time.deltaTime; // 経過時間を取得
        }



        // 一番最後のテキストが指定場所に移動後
        // 特定時間がたったら
        if( time > 2.0f )
        {
            moveFlag = false;           // 移動制御フラグをOFF
            TitleDirector.isNext = false; // タイトルスクリプトのシーン移行フラグをOFF

            // 座標を初期位置に設定
            kanda_text_pos.y = -350.0f;
            gondaira_text_pos.y = -450.0f;
            saito_text_pos.y = -550.0f;
            thank_text_pos.y = -750.0f;
            
        }



        // 座標を再設定
        text_up_pos.localPosition = kanda_text_pos;
        text_middle_pos.localPosition = gondaira_text_pos;
        text_down_pos.localPosition = saito_text_pos;
        Thank_Text.localPosition = thank_text_pos;



        // 移動制御フラグがOFFなら処理
        if (!moveFlag)
        {
            StartCoroutine(fade.FadeOut());     // FadeOut
            credit.SetActive(false);            // 非アクティブ化
        }
    }
}
