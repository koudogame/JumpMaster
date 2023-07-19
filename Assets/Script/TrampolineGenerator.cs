using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

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

    [Header("CreateCount")]
    [SerializeField] int nowCreateCnt = 0;
    [SerializeField] int maxCreateCnt = 10;

    // Start is called before the first frame update
    void Start()
    {
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

        // �����͈͓��ŁA�����_���ɍ��W�����߂�
        float px = Random.Range(minRange, maxRange);
        float pz = Random.Range(minRange, maxRange);
        go.transform.position = new Vector3(px, posY, pz);
        //Vector3 goPos = new Vector3(px, posY, pz);

        // ���݂̐�������0���傫���Ƃ�
        if (nowCreateCnt > 0)
        {
            //for (int i = 0; i < createCnt; i++)
            //{
            //    while (go.transform.position == allObjPos[i])
            //    {
            //        px = Random.Range(minRange, maxRange);
            //        pz = Random.Range(minRange, maxRange);
            //        go.transform.position = new Vector3(px, posY, pz);
            //    }
            //}

            Vector3 halfExtents = new Vector3(1f, 0.05f, 1f);
            // ---
            // nowCreateCnt��܂Ŏ���
            for (int n = 0; n < nowCreateCnt; ++n)
            {
                // �����_���̈ʒu
                px = Random.Range(minRange, maxRange);
                pz = Random.Range(minRange, maxRange);
                go.transform.position = new Vector3(px, posY, pz);
                Vector3 pos = go.transform.position;

                // �{�b�N�X�ƃA�C�e�����d�Ȃ�Ȃ��Ƃ�
                if (!Physics.CheckBox(pos, halfExtents, Quaternion.identity))
                {
                    go.transform.position = pos;
                    break;
                }
            }
            //Camera camera = GetComponent<Camera>();
            //float distance = 20.0f;
            //Ray ray = camera.ScreenPointToRay(go.transform.position);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit, distance))
            //{
            //    Vector3 movement = Vector3.Scale(prefab.transform.localScale, hit.normal) / 2;
            //    go = Instantiate(prefab, hit.point, Quaternion.identity);
            //    go.transform.position = new Vector3(hit.point.x + movement.x, hit.point.y + movement.y, hit.point.z + movement.z);
            //}

            //float distance = 2f;
            //for (float i = 0; i < distance; i++)
            //{
            //    for (float j = 0; j < distance; j++)
            //    {

            //    }
            //}
        }

        allObjPos[nowCreateCnt] = go.transform.position; //goPos;
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
    }
}
