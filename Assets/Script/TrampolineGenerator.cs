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
            Debug.LogWarning("TrampolineGenerator : playerにオブジェクトが設定されていません");
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

    // インスタンスの生成。引数で渡したPrefabのインスタンスが生成される
    private void CreateInstance(GameObject prefab)
    {
        // 現在の生成数が最大生成数以上の時、処理を行わない
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

        // 生成範囲内で、ランダムに座標を決める
        float px = Random.Range(minX, maxX);
        float pz = Random.Range(minZ, maxZ);
        go.transform.position = new Vector3(px, posY, pz);

        // 現在の生成数が0より大きいとき
        if (nowCreateCnt > 0)
        {
            Vector3 halfExtents = new Vector3(1f, 0.05f, 1f);
            // ---
            // nowCreateCnt回まで試す
            for (int n = 0; n < nowCreateCnt; ++n)
            {
                Vector3 pos;
                do
                {
                    // ランダムの位置
                    px = Random.Range(minX, maxX);
                    pz = Random.Range(minZ, maxZ);
                    pos = new Vector3(px, posY, pz);
                } while (pos == player.transform.position);

                // ボックスとアイテムが重ならないとき
                if (!Physics.CheckBox(pos, halfExtents, Quaternion.identity))
                {
                    // さらに、接地しているとき
                    if (Physics.Raycast(pos, Vector3.down, 1f))
                    {
                        //Debug.Log("レイがヒット");
                        go.transform.position = pos;
                        break;
                    }
                }
            }
        }

        allObjPos[nowCreateCnt] = go.transform.position;
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
        playerIsJump = true;
    }
}
