using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineGenerator : MonoBehaviour
{
    public GameObject Trampoline;
    Vector3[] allObjPos;
    float posY = 1.6f;
    float span = 1.0f;
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

        minRange = -25f;
        maxRange = 25f;

        nowNumber = 5;
        maxNumber = 10;

        allObjPos = new Vector3[maxNumber];
    }

    // Update is called once per frame
    void Update()
    {
        this.delta += Time.deltaTime;
        if(this.delta > this.span)
        {
            this.delta = 0f;
            CreateInstance(Trampoline);
        }
    }

    private void CreateInstance(GameObject prefab)
    {
        if (createCnt >= maxNumber) return;

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
        }

        allObjPos[createCnt] = go.transform.position;
        createCnt++;
    }
}
