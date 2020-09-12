using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    // Start is called before the first frame update
    BLACK.Combat.Dumbfire dummythicc;

    void Start()
    {
        dummythicc = GetComponent<BLACK.Combat.Dumbfire>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(dummythicc.target.transform.position);
    }
}
