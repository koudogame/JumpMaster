//
//  �V�[���Ԃ̃f�[�^�󂯓n�����ɂ��V�[���̃Z�b�e�B���O�Ǘ�
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// "GameScene"��GameObject�ɃA�^�b�`����Ă�z��
public sealed class GameScoreSingleton : MonoBehaviour
{
    private static GameScoreSingleton instance;
    public static GameScoreSingleton Instance => instance;

    public int Score = 0;
    public int Time = 0;

    private void Awake()
    {
        // instance�����łɂ������玩������������B
        if (instance && this != instance)
        {
            Destroy(this.gameObject);
        }

        instance = this;

        // Scene�J�ڂŔj������Ȃ悤�ɂ���B      
        DontDestroyOnLoad(this);
    }
}

// "GameScene"��
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

// "ResultScene"��
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
