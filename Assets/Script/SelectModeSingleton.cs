using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// "SelectScene"��GameObject�ɃA�^�b�`����Ă�z��
public sealed class SelectModeSingleton : MonoBehaviour
{
    private static SelectModeSingleton instance;
    public static SelectModeSingleton Instance => instance;

    public string Mode = "Simple";
    public string Level = "Easy";

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

// "SelectScene"��
public class ModeChanger
{
    public void SetMode(string mode)
    {
        SelectModeSingleton.Instance.Mode = mode;
    }

    public void SetLevel(string level)
    {
        SelectModeSingleton.Instance.Level = level;
    }
}

// "GameScene"��
public class ModeGetter
{
    public string GetMode()
    {
        return SelectModeSingleton.Instance.Mode;
    }

    public string GetLevel()
    {
        return SelectModeSingleton.Instance.Level;
    }
}