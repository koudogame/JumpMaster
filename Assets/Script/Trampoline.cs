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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Player" || collision.gameObject.tag == "Player")
        {
            collision.rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}
