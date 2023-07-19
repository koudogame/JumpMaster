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

    GameObject generator;

    // Start is called before the first frame update
    void Start()
    {
        generator = GameObject.Find("TrampolineGenerator");
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
            if(collision.transform.position.y > this.transform.position.y)
            {
                collision.rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                generator.GetComponent<TrampolineGenerator>().DestroyCnt( this.transform.position );
                Destroy(gameObject);
            }
        }
    }
}
