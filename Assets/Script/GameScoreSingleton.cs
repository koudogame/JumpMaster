//
//  シーン間のデータ受け渡し等によるシーンのセッティング管理
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// "GameScene"でGameObjectにアタッチされてる想定
public sealed class GameScoreSingleton : MonoBehaviour
{
    private static GameScoreSingleton instance;
    public static GameScoreSingleton Instance => instance;

    public int Score = 0;
    public int Time = 0;

    private void Awake()
    {
        // instanceがすでにあったら自分を消去する。
        if (instance && this != instance)
        {
            Destroy(this.gameObject);
        }

        instance = this;

        // Scene遷移で破棄されなようにする。      
        DontDestroyOnLoad(this);
    }
}

// "GameScene"側
public class ScoreChanger
{
    public void ScorePlusOne()
    {
        GameScoreSingleton.Instance.Score++;
    }

    public void SetScore(int score)
    {
        GameScoreSingleton.Instance.Score = score;
    }

    public void SetTime(int time)
    {
        GameScoreSingleton.Instance.Time = time;
    }
}

// "ResultScene"側
public class ScoreGetter
{
    public int GetScore()
    {
        return GameScoreSingleton.Instance.Score;
    }

    public int GetTime()
    {
        return GameScoreSingleton.Instance.Time;
    }
}
