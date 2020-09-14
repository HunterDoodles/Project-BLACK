using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movetime : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float time = 10f;
    [SerializeField]
    Vector3 movement;
    private bool flipped;
    private float fliptime = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fliptime += Time.deltaTime;
        if (fliptime > time)
        {
            fliptime -= time;
            flipped = !flipped;
        }
        transform.position += (flipped?-movement:movement)* Time.deltaTime;
    }
}
