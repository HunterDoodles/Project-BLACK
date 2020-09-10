using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MISLESHT : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = transform.forward * 100f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Random.InitState((int)(Time.time*100));
        _rb.AddForce(transform.forward * 200f);//+Vector3.down*Random.Range(-.2f,.2f) + Vector3.right * Random.Range(-.2f,.2f));
    }
}
