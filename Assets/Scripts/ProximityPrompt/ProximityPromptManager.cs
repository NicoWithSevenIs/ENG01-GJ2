using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityPromptManager : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private List<GameObject> promptQueue; // priority queue would be better here but ehhhhhh

    [SerializeField] private RectTransform promptUI;

    private void Start()
    {
        Action<Parameters> pushPrompt = (Parameters p) =>
        {
            print("Adding");
            GameObject promptSubject = (GameObject) p.GetObjectExtra("PROMPT_SUBJECT") ;

            print(promptSubject);
            if(promptSubject != null && !promptQueue.Contains(promptSubject)) 
                promptQueue.Add(promptSubject);
        };

        Action<Parameters> removePrompt = (Parameters p) =>
        {
            GameObject promptSubject = (GameObject) p.GetObjectExtra("PROMPT_SUBJECT");

            if (promptSubject != null && promptQueue.Contains(promptSubject))
            {
                promptQueue.Remove(promptSubject);
            }

        };

        EventBroadcaster.Instance.AddObserver(EventNames.UI_EVENTS.ON_PROXIMITY_PROMPT_ENTER, pushPrompt);
        EventBroadcaster.Instance.AddObserver(EventNames.UI_EVENTS.ON_PROXIMITY_PROMPT_EXIT, removePrompt);

    }

    private void Update()
    {
        
    }



    private GameObject getNearestPrompt()
    {
        GameObject nearest = null;

        /*
        foreach(var prompt in promptQueue) {

            GameObject promptSubject = prompt.GetObjectExtra("PROMPT_SUBJECT") as GameObject;

            if(promptSubject == null)
                continue;
            
            if (nearest == null)
            {
                nearest = promptSubject;
                continue;
            }

            float nearestDistance = Vector3.Distance(player.position, nearest.transform.position);
            float subjectDistance = Vector3.Distance(player.position, promptSubject.transform.position);
            
            if(subjectDistance < nearestDistance)
                nearest = promptSubject;
        }
         */
        return nearest;
       
    }

    #region singleton

    public ProximityPromptManager instance = null;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else Destroy(gameObject);
        }

    #endregion
}
