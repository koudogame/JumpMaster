using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public sealed class Fade : MonoBehaviour
{
    [Header("フェードのイメージ"), SerializeField] Image fadeImage;
    [Header("フェード時間"),SerializeField] float duration;
    [Header("alpha値"), SerializeField] float alpha;


    //*****************************************************
    // フェードイン処理
    //*****************************************************
    public IEnumerator FadeIn()
    {
        float time = 0.0f; // 時間計測用

        // 指定した時間経過するまで処理
        while (time < duration)
        {
            // Fade処理呼び出し
            time = FadeProcess(time, 1.0f, 0.0f);

            // メソッドの中断
            yield return null;
        }

        // フェードイメージを無効化
        fadeImage.enabled = false;

        //  終了
        yield return true;
    }




    //*****************************************************+
    // フェードアウト処理
    //*****************************************************
    public IEnumerator FadeOut()
    {
        float time = 0.0f; // 時間計測用

        // 指定した時間経過するまで処理
        while (time < duration)
        {
            // Fade処理呼び出し
            time = FadeProcess(time, 0.0f, 1.0f);
            //Debug.Log("FadeOut:ON");

            // メソッドの中断
            yield return null;
        }

        // フェードイメージを有効化
        fadeImage.enabled = true;

        //  終了
        yield return true;
    }




    //*****************************************************
    // 半透明から元に戻すFade
    //*****************************************************
    public IEnumerator HalfFadeOut()
    {
        float time = 0.0f;  // 時間計測用

        // 指定した時間経過するまで処理
        while (time < duration)
        {
            // Fade処理呼び出し
            // 背景を半透明にする
            time = FadeProcess(time, 0.0f, 0.5f);

            // メソッドの中断
            yield return null;
        }

        // フェードイメージを有効化
        fadeImage.enabled = true;
    }




    //*****************************************************
    // 元から半透明にするFade
    //*****************************************************
    public IEnumerator HalfFadeIn()
    {
        float time = 0.0f;  // 時間計測用

        // 指定した時間経過するまで処理
        while (time < duration)
        {
            // Fade処理呼び出し
            time = FadeProcess(time, 1.0f, 0.5f);

            // メソッドの中断
            yield return null;
        }
        // フェードイメージを有効化
        fadeImage.enabled = false;
    }




    //*****************************************************
    // Fade内の処理
    //*****************************************************
    float FadeProcess(float currentTime, float Start,float End)
    {
        // 指定した範囲でアルファ値を補完
        currentTime += Time.deltaTime;
        alpha = Mathf.Lerp(Start, End, currentTime / duration);

        // アルファ値設定
        Color imgColor = fadeImage.color;
        fadeImage.color = new Color(imgColor.r, imgColor.g, imgColor.b, alpha);

        // 経過時間を返す
        return currentTime;
    }

    // フェード処理
    public IEnumerator StartFade(float Start, float End)
    {
        GameObject startAndEndUI = GameObject.Find("Start&End");
        float time = 0.0f; // 時間計測用
        Image img = startAndEndUI.GetComponent<Image>(); // コンポーネントを取得

        while (true)
        {
            yield return null;

            // Lerpメソッドを使ってアルファ値を補間
            float a = Mathf.Lerp(Start, End, time);
            //img.color = new Color(1.0f, 1.0f, 1.0f, a);
            Color imgColor = fadeImage.color;
            fadeImage.color = /*new Color(imgColor.r, imgColor.g, imgColor.b, a)*/new Color(1.0f, 1.0f, 1.0f, a);
            alpha = a;
            // 補間に使う時間を進める
            time += Time.deltaTime;

            // 1秒経過したら終了
            if (time > 1f)
            {
                // 誤差も出ると思うので念のため上書き
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