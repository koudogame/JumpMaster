using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class TrampolineGenerator : MonoBehaviour
{
    public GameObject Trampoline;
    Vector3[] allObjPos;
    float posY = 1f;
    float span = 0.1f;
    float delta = 0f;
    int createCnt = 0;

    [Header("Position Distance")]
    [SerializeField] float minDistance = 2f;
    [SerializeField] float maxDistance = 4f;

    [Header("Position Range")]
    [SerializeField] float minRange = -10f;
    [SerializeField] float maxRange = 10f;

    [Header("Number Of Object")]
    [SerializeField] int nowNumber = 10;
    [SerializeField] int maxNumber = 10;
    [SerializeField] int createNowNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        allObjPos = new Vector3[maxNumber];
    }

    // Update is called once per frame
    void Update()
    {
        if(nowNumber > maxNumber) nowNumber = maxNumber;

        createNowNumber = createCnt;
        this.delta += Time.deltaTime;
        if(this.delta > this.span)
        {
            this.delta = 0f;
            CreateInstance(Trampoline);
        }
    }

    // インスタンスの生成。引数で渡したPrefabのインスタンスが生成される
    private void CreateInstance(GameObject prefab)
    {
        if (createCnt >= nowNumber) return;

        GameObject go = Instantiate(prefab);
        float px = Random.Range(minRange, maxRange);
        float pz = Random.Range(minRange, maxRange);
        go.transform.position = new Vector3(px, posY, pz);
        //Vector3 goPos = new Vector3(px, posY, pz);

        if (createCnt > 0)
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
            // createCnt回試す
            for (int n = 0; n < createCnt; n++)
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

        allObjPos[createCnt] = go.transform.position; //goPos;
        createCnt++;
    }

    // トランポリンオブジェクトの破棄の際に呼ぶ。生成カウントを1減らす
    public void DestroyCnt()
    {
        createCnt--;
    }
}
