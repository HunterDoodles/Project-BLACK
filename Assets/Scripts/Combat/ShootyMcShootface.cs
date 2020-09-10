using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootyMcShootface : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform gunorigin;
    [SerializeField]
    private GameObject boolet;
    [SerializeField]
    private float spray = 40f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space"))
        {
            Random.InitState((int) (Time.frameCount* 100));
            Instantiate(boolet,gunorigin.position,Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x+Random.Range(-spray,spray),transform.rotation.eulerAngles.y + Random.Range(-spray,spray),transform.rotation.eulerAngles.z + Random.Range(-spray,spray))));
        }
    }
}
