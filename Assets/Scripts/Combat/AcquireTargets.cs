using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcquireTargets : MonoBehaviour
{
    // Start is called before the first frame update
    private Target[] _potentialTargets;
    private Target _currentTarget;
    private float _currentTargetDistance;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentTarget = null;
        _potentialTargets = (Target[]) FindObjectsOfType(typeof(Target));
        foreach (Target target in _potentialTargets)
        {
            if (target.gameObject != gameObject)
            {
                float dist = Vector3.Distance(transform.position,target.transform.position);
                Vector3 localPos = transform.InverseTransformPoint(target.transform.position);
                /*localPos = new Vector3(localPos.x,0,localPos.z);
                float angleY = Mathf.Atan2(Vector3.Dot(Vector3.zero,Vector3.Cross(Vector3.forward,localPos)),Vector3.Dot(Vector3.forward,localPos)); //* Mathf.Rad2Deg;
                angleY *= Mathf.Rad2Deg;*/
                float angleY = Vector3.Angle(Vector3.forward,new Vector3(localPos.x,0,localPos.z).normalized);
                //print(angleY);
                Debug.DrawRay(transform.position,Quaternion.Euler(0,25,0) * transform.forward * 10);
                Debug.DrawRay(transform.position,Quaternion.Euler(0,-25,0) * transform.forward * 10);
                if (angleY < 25)
                {

                    if (!Physics.Linecast(transform.position,target.transform.position))
                    {
                        Debug.DrawLine(transform.position,target.transform.position,Color.yellow);
                        if (_currentTarget != null)
                        {

                            if (dist < _currentTargetDistance)
                            {
                             
                                _currentTarget = target;
                                _currentTargetDistance = dist;
                            }
                            else
                            {
                                
                            }
                        }
                        else
                        {
                           
                            _currentTarget = target;
                            _currentTargetDistance = dist;
                        }
                    }
                    else
                    {

                        Debug.DrawLine(transform.position,target.transform.position,Color.red);
                    }

                }
                else
                {
                    _currentTargetDistance = 0;
                }
            }
        }
        if(_currentTarget!=null)
            Debug.DrawLine(transform.position,_currentTarget.transform.position,Color.green);
        print(_currentTargetDistance);
        //  print(_currentTargetDistance);
    }
    public Target GetCurrentTarget()
    {
        return _currentTarget;
    }
}
