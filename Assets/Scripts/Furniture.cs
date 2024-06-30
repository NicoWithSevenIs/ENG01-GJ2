using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public bool hasMusicRoll = false;
    
    public void Inspect()
    {

        List<string> lines = new List<string>();

        ConditionalDialogue c = GetComponent<ConditionalDialogue>();
        if (c != null)
            c.Condition = hasMusicRoll;

        GetComponent<IDialogueTrigger>()?.TriggerDialogue();
            
    }
}
