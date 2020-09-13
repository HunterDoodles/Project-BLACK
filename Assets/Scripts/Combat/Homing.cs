using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float correctionSpeed = 30f;
    [SerializeField]
    private float heightMulti = 1.5f;
    [SerializeField]
    private float trackingDelay = 2.5f;
    [SerializeField]
    private float duration =-1f;

    private BLACK.Combat.Dumbfire _missileBase;

    void Start()
    {
        _missileBase = GetComponent<BLACK.Combat.Dumbfire>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(dummythicc.target.transform.position);
        if (_missileBase.aliveTime > trackingDelay&& _missileBase.aliveTime < (duration < 0 ? Mathf.Infinity : duration + trackingDelay)&&_missileBase.target!=null)            
        {

            Vector3 direction = (_missileBase.target.transform.position - transform.position).normalized;
            float height = transform.position.y - _missileBase.target.transform.position.y;
            transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(direction),correctionSpeed * (height > 0 ? 1 : heightMulti) * Time.deltaTime);
        }
    }
}
