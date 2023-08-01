using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// "SelectScene"でGameObjectにアタッチされてる想定
public sealed class SelectModeSingleton : MonoBehaviour
{
    private static SelectModeSingleton instance;
    public static SelectModeSingleton Instance => instance;

    public string Mode = "Simple";
    public string Level = "Easy";

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

// "SelectScene"側
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

// "GameScene"側
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