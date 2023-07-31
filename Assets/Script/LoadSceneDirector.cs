using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// シーンの情報
public struct SceneInfo
{
    public string Name;    // 現在のシーンの名前
    public int StageNo;     // 現在のステージ番号
}




// シーンの名前を保持するクラス
public class LoadSceneDirector : MonoBehaviour
{
    static SceneInfo scene;

    // Start is called before the first frame update
    void Start()
    {
        scene.Name = SceneManager.GetActiveScene().name;            // 現在のシーンの名前を格納
        scene.StageNo = 1;  // ステージ番号を初期化
    }




    //**************************************************************************************
    // Stage名を設定
    //**************************************************************************************
    public static void LoadStage(int StageNo = 1)
    {
        scene.StageNo = StageNo;        // ステージの番号を格納
        scene.Name = "Stage" + StageNo; // Stageの名前をセット
    }
    
    


    //**************************************************************************************
    // 次のシーンの名前を格納
    //**************************************************************************************
    public static void LoadSceneCheck(string SceneName = null)
    {
        scene.Name = SceneName;  // 現在のシーンの名前を取得
    }



    //**************************************************************************************
    // 現在のシーンの名前を返却
    //**************************************************************************************
    public static string LoadScene()
    {
        // 現在のシーン名を返す
        return scene.Name;
    }



    //**************************************************************************************
    // ステージ番号を返す関数
    //**************************************************************************************
    public static int GetStageNum()
    {
        return scene.StageNo;
    }
}
