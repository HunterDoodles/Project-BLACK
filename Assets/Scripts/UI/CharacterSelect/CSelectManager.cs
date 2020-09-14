using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CSelectManager : MonoBehaviour
{

    public GameObject[] contestantCams;
    GameObject currentContestant;
    int contestantIndex;
    public Button nextCharacter;
    public Button previousCharacter;

    public CinemachineVirtualCamera currentCamera;

    void Start() 
    {     
        Button next = nextCharacter.GetComponent<Button>();
        next.onClick.AddListener(TaskOnClickNext);

        Button prev = previousCharacter.GetComponent<Button>();
        prev.onClick.AddListener(TaskOnClickPrev);
        
        contestantIndex = 0;
        currentContestant = contestantCams[0];
    }

    void TaskOnClickNext()
    {
        Debug.Log("Next character.");
        contestantIndex++;
        if (contestantIndex > (contestantCams.Length - 1))
        {
            contestantIndex = 0;
        }
    }

    void TaskOnClickPrev()
    {
        Debug.Log("Previous character.");
        contestantIndex--;
        if(contestantIndex < 0)
        {
            contestantIndex = contestantCams.Length - 1;
        }
    }

    void Update()
    {
        
    }

}
