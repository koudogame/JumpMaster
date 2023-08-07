using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrampolineGenerator : MonoBehaviour
{
    public GameObject Trampoline;
    Vector3[] allObjPos;
    float posY = 1f;
    public float span = 0.1f;
    float delta = 0f;
    bool initFlag = true;

    [Header("Position Range")]
    [SerializeField] float minRange = -10f;
    [SerializeField] float maxRange = 10f;
    [SerializeField] float originRange = 10f;

    [Header("CreateCount")]
    [SerializeField] int nowCreateCnt = 0;
    [SerializeField] int maxCreateCnt = 10;

    [Header("Player")]
    [SerializeField] GameObject player;
    [SerializeField] bool playerIsJump = false;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("TrampolineGenerator : player�ɃI�u�W�F�N�g���ݒ肳��Ă��܂���");
            return;
        }

        allObjPos = new Vector3[ maxCreateCnt ];
    }

    // Update is called once per frame
    void Update()
    {
        if (initFlag) span = 0.1f;
        else span = 0.5f;

        if( nowCreateCnt > maxCreateCnt ) nowCreateCnt = maxCreateCnt;

        delta += Time.deltaTime;
        if(delta > span)
        {
            if(initFlag && nowCreateCnt >= maxCreateCnt - 1) initFlag = false;
            delta = 0f;
            CreateInstance(Trampoline);
        }
    }

    // �C���X�^���X�̐����B�����œn����Prefab�̃C���X�^���X�����������
    private void CreateInstance(GameObject prefab)
    {
        // ���݂̐��������ő吶�����ȏ�̎��A�������s��Ȃ�
        if (nowCreateCnt >= maxCreateCnt) return;

        GameObject go = Instantiate(prefab);
        float minX = minRange;
        float maxX = maxRange;
        float minZ = minRange;
        float maxZ = maxRange;

        if ( playerIsJump )
        {
            minX = player.transform.position.x - 5f;
            maxX = player.transform.position.x + 5f;
            minZ = player.transform.position.z - 5f;
            maxZ = player.transform.position.z + 5f;
            playerIsJump = false;
        }

        // �����͈͓��ŁA�����_���ɍ��W�����߂�
        float px = Random.Range(minX, maxX);
        float pz = Random.Range(minZ, maxZ);
        go.transform.position = new Vector3(px, posY, pz);

        // ���݂̐�������0���傫���Ƃ�
        if (nowCreateCnt > 0)
        {
            Vector3 halfExtents = new Vector3(1f, 0.05f, 1f);
            // ---
            // nowCreateCnt��܂Ŏ���
            for (int n = 0; n < nowCreateCnt; ++n)
            {
                Vector3 pos;
                do
                {
                    // �����_���̈ʒu
                    px = Random.Range(minX, maxX);
                    pz = Random.Range(minZ, maxZ);
                    pos = new Vector3(px, posY, pz);
                } while (pos == player.transform.position);

                // �{�b�N�X�ƃA�C�e�����d�Ȃ�Ȃ��Ƃ�
                if (!Physics.CheckBox(pos, halfExtents, Quaternion.identity))
                {
                    // ����ɁA�ڒn���Ă���Ƃ�
                    if (Physics.Raycast(pos, Vector3.down, 1f))
                    {
                        //Debug.Log("���C���q�b�g");
                        go.transform.position = pos;
                        break;
                    }
                }
            }
        }

        allObjPos[nowCreateCnt] = go.transform.position;
        ++nowCreateCnt;
    }

    // �g�����|�����I�u�W�F�N�g�̔j���̍ۂɌĂԁB�����J�E���g��1���炷
    public void DestroyCnt( Vector3 Position )
    {
        bool flg = false; // �O�l�ߊJ�n�t���O
        int startNum = 0; // �O�l�ߊJ�n�ʒu����

        // �󂯎�������W�Ɠ����l��z����̗v�f����T���o��
        // nowCreateCnt��܂Ŏ���
        for (int n = 0; n < nowCreateCnt; ++n)
        {
            if( allObjPos[ n ] == Position )
            {
                // ��v�����v�f���Ō���łȂ��ꍇ
                if (n < nowCreateCnt - 1)
                {
                    allObjPos[n] = allObjPos[n + 1];
                    startNum = n + 1;
                    flg = true;
                    break;
                }
                // ��v�����v�f���Ō���̏ꍇ
                else { allObjPos[n] = Vector3.zero; break; }
            }
        }

        // �t���O��ture�̎��̂ݒʂ�A�v�f���f�[�^�̑O�l��
        // nowCreateCnt��܂Ŏ���
        if (flg)
        {
            for (int n = startNum; n < nowCreateCnt; ++n)
            {
                // ���݌��Ă���v�f���Ō���łȂ��ꍇ
                if (n < nowCreateCnt - 1) allObjPos[n] = allObjPos[n + 1];

                // ���݌��Ă���v�f���Ō���̏ꍇ
                else { allObjPos[n] = Vector3.zero; break; }
            }
        }

        --nowCreateCnt;
        playerIsJump = true;
    }
}
