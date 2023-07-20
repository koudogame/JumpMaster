using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] string targetName = "Player";
    [SerializeField] string targetTagName = "Player";

    [Header("Jumping")]
    [SerializeField] float jumpForce = 10;
    [SerializeField] bool  jumpFlag = false;

    GameObject generator;
    GameObject director;

    // Start is called before the first frame update
    void Start()
    {
        generator = GameObject.Find("TrampolineGenerator");
        director = GameObject.Find("GameDirector");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == targetName || collision.gameObject.tag == targetTagName)
        {
            //Ray ray = new Ray(this.transform.position, Vector3.up);
            //RaycastHit hit;
            //if(Physics.Raycast(ray, out hit, 1f))
            //{
            //    collision.rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            //}
            if(collision.transform.position.y > this.transform.position.y && !jumpFlag)
            {
                jumpFlag = true;
                collision.rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == targetName || collision.gameObject.tag == targetTagName)
        {
            //Ray ray = new Ray(this.transform.position, Vector3.up);
            //RaycastHit hit;
            //if(Physics.Raycast(ray, out hit, 1f))
            //{
            //    collision.rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            //}
            if (collision.transform.position.y > this.transform.position.y && jumpFlag)
            {
                jumpFlag = false;
                generator.GetComponent<TrampolineGenerator>().DestroyCnt(this.transform.position);
                director.GetComponent<GameDirector>().ScoreCount();
                Destroy(gameObject);
            }
        }
    }
}
