using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [Header("TimeLimit")]
    [SerializeField] float endTime = 60f;
    [SerializeField] float nowTime = 0f;
    [SerializeField] bool endFlag = false;

    GameObject textUi;

    // Start is called before the first frame update
    void Start()
    {
        textUi = GameObject.Find("Text (TMP)");
    }

    // Update is called once per frame
    void Update()
    {
        nowTime += Time.deltaTime;

        if(nowTime >= endTime)
        {
            //textUi.GetComponent<Text>().text = "GameOver";
            endFlag = true;
        }
    }
}
