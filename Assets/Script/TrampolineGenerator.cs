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

    // インスタンスの生成。引数で渡したPrefabのインスタンスが生成される
    private void CreateInstance(GameObject prefab)
    {
        // 現在の生成数が最大生成数以上の時、処理を行わない
        if (nowCreateCnt >= maxCreateCnt) return;

        GameObject go = Instantiate(prefab);

        // 生成範囲内で、ランダムに座標を決める
        float px = Random.Range(minRange, maxRange);
        float pz = Random.Range(minRange, maxRange);
        go.transform.position = new Vector3(px, posY, pz);
        //Vector3 goPos = new Vector3(px, posY, pz);

        // 現在の生成数が0より大きいとき
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
            // nowCreateCnt回まで試す
            for (int n = 0; n < nowCreateCnt; ++n)
            {
                // ランダムの位置
                px = Random.Range(minRange, maxRange);
                pz = Random.Range(minRange, maxRange);
                go.transform.position = new Vector3(px, posY, pz);
                Vector3 pos = go.transform.position;

                // ボックスとアイテムが重ならないとき
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

    // トランポリンオブジェクトの破棄の際に呼ぶ。生成カウントを1減らす
    public void DestroyCnt( Vector3 Position )
    {
        bool flg = false; // 前詰め開始フラグ
        int startNum = 0; // 前詰め開始位置を代入

        // 受け取った座標と同じ値を配列内の要素から探し出す
        // nowCreateCnt回まで試す
        for (int n = 0; n < nowCreateCnt; ++n)
        {
            if( allObjPos[ n ] == Position )
            {
                // 一致した要素が最後尾でない場合
                if (n < nowCreateCnt - 1)
                {
                    allObjPos[n] = allObjPos[n + 1];
                    startNum = n + 1;
                    flg = true;
                    break;
                }
                // 一致した要素が最後尾の場合
                else { allObjPos[n] = Vector3.zero; break; }
            }
        }

        // フラグがtureの時のみ通る、要素内データの前詰め
        // nowCreateCnt回まで試す
        if (flg)
        {
            for (int n = startNum; n < nowCreateCnt; ++n)
            {
                // 現在見ている要素が最後尾でない場合
                if (n < nowCreateCnt - 1) allObjPos[n] = allObjPos[n + 1];

                // 現在見ている要素が最後尾の場合
                else { allObjPos[n] = Vector3.zero; break; }
            }
        }

        --nowCreateCnt;
    }
}
