using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineGenerator : MonoBehaviour
{
    [Header("PositionDistance")]
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;

    [Header("PositionRange")]
    [SerializeField] float minRange;
    [SerializeField] float maxRange;

    [Header("NumberOfObject")]
    [SerializeField] int nowNumber;
    [SerializeField] int maxNumber;

    public GameObject Trampoline;
    Vector3[] allObjPos;
    float span = 1.0f;
    float delta = 0f;
    int createCnt = 0;

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
            GameObject go = Instantiate(Trampoline);
            int px = Random.Range(-6, 7);
            go.transform.position = new Vector3(px, 0, 0);
        }
    }

    private void CreateInstance(GameObject prefab)
    {
        GameObject go = Instantiate(prefab);
        float px = Random.Range(minRange, maxRange);
        float pz = Random.Range(minRange, maxRange);
        go.transform.position = new Vector3(px, 0, pz);

        for( int i = 0; i < maxNumber; i++ )
        {
            if(go.transform.position == allObjPos[i])
            {

            }
        }

        allObjPos[createCnt] = go.transform.position;
        createCnt++;
    }
}
