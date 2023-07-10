using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineGenerator : MonoBehaviour
{
    public GameObject Trampoline;
    float span = 1.0f;
    float delta = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.delta += Time.deltaTime;
        if(this.delta> this.span)
        {
            this.delta = 0f;
            GameObject go = Instantiate(Trampoline);
            int px = Random.Range(-6, 7);
            go.transform.position = new Vector3(px, 0, 0);
        }
    }
}
