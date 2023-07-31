using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// �V�[���̏��
public struct SceneInfo
{
    public string Name;    // ���݂̃V�[���̖��O
    public int StageNo;     // ���݂̃X�e�[�W�ԍ�
}




// �V�[���̖��O��ێ�����N���X
public class LoadSceneDirector : MonoBehaviour
{
    static SceneInfo scene;

    // Start is called before the first frame update
    void Start()
    {
        scene.Name = SceneManager.GetActiveScene().name;            // ���݂̃V�[���̖��O���i�[
        scene.StageNo = 1;  // �X�e�[�W�ԍ���������
    }




    //**************************************************************************************
    // Stage����ݒ�
    //**************************************************************************************
    public static void LoadStage(int StageNo = 1)
    {
        scene.StageNo = StageNo;        // �X�e�[�W�̔ԍ����i�[
        scene.Name = "Stage" + StageNo; // Stage�̖��O���Z�b�g
    }
    
    


    //**************************************************************************************
    // ���̃V�[���̖��O���i�[
    //**************************************************************************************
    public static void LoadSceneCheck(string SceneName = null)
    {
        scene.Name = SceneName;  // ���݂̃V�[���̖��O���擾
    }



    //**************************************************************************************
    // ���݂̃V�[���̖��O��ԋp
    //**************************************************************************************
    public static string LoadScene()
    {
        // ���݂̃V�[������Ԃ�
        return scene.Name;
    }



    //**************************************************************************************
    // �X�e�[�W�ԍ���Ԃ��֐�
    //**************************************************************************************
    public static int GetStageNum()
    {
        return scene.StageNo;
    }
}
