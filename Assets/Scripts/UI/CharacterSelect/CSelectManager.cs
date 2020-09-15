using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CSelectManager : MonoBehaviour
{

    public List<Transform> contestantCams;
    public Transform parent;
    GameObject currentContestant;
    int contestantIndex;
    public Button nextCharacter;
    public Button previousCharacter;

    public CinemachineVirtualCamera currentCamera;

    void Start() 
    {
        foreach (Transform child in parent)
        {
            contestantCams.Add(child);
        }
        Button next = nextCharacter.GetComponent<Button>();
        next.onClick.AddListener(TaskOnClickNext);

        Button prev = previousCharacter.GetComponent<Button>();
        prev.onClick.AddListener(TaskOnClickPrev);
        
        contestantIndex = 0;
        currentContestant = contestantCams[0].gameObject;
    }

    void TaskOnClickNext()
    {
        Debug.Log("Next character.");
        currentCamera.Priority--;
        contestantIndex++;
        if (contestantIndex > (contestantCams.Count))
        {
            contestantIndex = 0;
        }
        currentCamera = contestantCams[contestantIndex].GetComponent<CinemachineVirtualCamera>();

    }

    void TaskOnClickPrev()
    {
        Debug.Log("Previous character.");
   
        contestantIndex--;
        if (contestantIndex < 0)
        {
            contestantIndex = contestantCams.Count;
        }
        
    }

    void Update()
    {
        
    }

}
