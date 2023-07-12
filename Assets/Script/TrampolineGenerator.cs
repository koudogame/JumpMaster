using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrampolineGenerator : MonoBehaviour
{
    public GameObject Trampoline;
    Vector3[] allObjPos;
    float posY = 1f;
    float span = 0.1f;
    float delta = 0f;
    int createCnt = 0;

    [Header("PositionDistance")]
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;

    [Header("PositionRange")]
    [SerializeField] float minRange;
    [SerializeField] float maxRange;

    [Header("NumberOfObject")]
    [SerializeField] int nowNumber;
    [SerializeField] int maxNumber;

    // Start is called before the first frame update
    void Start()
    {
        minDistance = 2f;
        maxDistance = 4f;

        minRange = -10f;
        maxRange = 10f;

        nowNumber = 10;
        maxNumber = 10;

        allObjPos = new Vector3[maxNumber];
    }

    // Update is called once per frame
    void Update()
    {
        if(nowNumber > maxNumber) nowNumber = maxNumber;

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

        if (createCnt > 0)
        {
            for (int i = 0; i < createCnt; i++)
            {
                while (go.transform.position == allObjPos[i])
                {
                    px = Random.Range(minRange, maxRange);
                    pz = Random.Range(minRange, maxRange);
                    go.transform.position = new Vector3(px, posY, pz);
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
        }

        allObjPos[createCnt] = go.transform.position;
        createCnt++;
    }

    // トランポリンオブジェクトの破棄の際に呼ぶ。生成カウントを1減らす
    public void DestroyCnt()
    {
        createCnt--;
    }
}
