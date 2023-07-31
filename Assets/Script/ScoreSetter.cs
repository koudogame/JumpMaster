using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSetter : MonoBehaviour
{
    // ScoreŽó‚¯“n‚µ—p
    public static class GameScoreStatic
    {
        public static int Score = 0;
        public static int Time = 0;
    }

    // "GameScene"‘¤
    public class ScoreChanger
    {
        public void ScorePlusOne()
        {
            GameScoreStatic.Score++;
        }

        public void SetScore( int score)
        { 
            GameScoreStatic.Score = score; 
        }

        public void SetTime(int time)
        {
            GameScoreStatic.Time = time;
        }
    }

    // "ResultScene"‘¤
    public class ScoreGetter
    {
        public int GetScore()
        {
            return GameScoreStatic.Score;
        }

        public int GetTime()
        {
            return GameScoreStatic.Time;
        }
    }

}
