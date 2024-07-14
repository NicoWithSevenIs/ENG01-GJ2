using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> dialogueDeque; //not sure if c# supports deques so im using a makeshift one

    [Range(1.5f, 4.5f)]
    [SerializeField] private float dialogueInterval = 3f; 

    private bool willDisplayNext;

    private void Start()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.DIALOGUE_EVENTS.ON_DIALOGUE_INSERTION, AddLines);
        dialogueDeque = new Queue<string>();
        willDisplayNext = true;

        EventBroadcaster.Instance.AddObserver(EventNames.DIALOGUE_EVENTS.ON_TRUNCATE_DIALOGUE, () => dialogueDeque.Clear() );
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            willDisplayNext = true;
            StopAllCoroutines();
        }

        if (!willDisplayNext)
            return;

        if (dialogueDeque.Count == 0)
        {
            EventBroadcaster.Instance.PostEvent(EventNames.DIALOGUE_EVENTS.ON_DIALOGUE_ENDED);
            return;
        }

        StartCoroutine(DisplayLine(dialogueDeque.Dequeue()));
        
    }

    private IEnumerator DisplayLine(string line)
    {
        willDisplayNext = false;
        
        Parameters p = new Parameters();
        p.PutExtra("Line", line);

        EventBroadcaster.Instance.PostEvent(EventNames.DIALOGUE_EVENTS.ON_DIALOGUE_INVOCATION, p);

        yield return new WaitForSeconds(dialogueInterval);
        willDisplayNext= true;
    
    }

    private void AddLines(Parameters p)
    {
        object[] arr = p.GetArrayExtra("Lines");
        List<string> lines = new List<string>(Array.ConvertAll<object, string>(arr, x => x.ToString()));

        bool willOverride = p.GetBoolExtra("WillOverride", false);
            
        if (willOverride)
        {
            if(dialogueDeque.Count > 0)
                lines.Add("Anyway...");
            PushFront(lines);
        }else EnqueueBatch(lines);
    }

    private void PushFront(List<string> lines)
    {
        Queue<string> temp = new Queue<string>(lines);
        while(dialogueDeque.Count > 0)
            temp.Enqueue(dialogueDeque.Dequeue()); 
        dialogueDeque = temp;
    }

    private void EnqueueBatch(List<string> lines)
    {
        foreach(string s in lines)
            dialogueDeque.Enqueue(s);
    }

}
