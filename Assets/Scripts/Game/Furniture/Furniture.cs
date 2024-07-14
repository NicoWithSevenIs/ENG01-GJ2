using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    private bool _hasMusicRoll = false;
    public  bool hasMusicRoll { get { return _hasMusicRoll; } set { _hasMusicRoll = value; } }
    
    public void Inspect()
    {

        List<string> lines = new List<string>();

        ConditionalDialogue c = GetComponent<ConditionalDialogue>();
        if (c != null)
            c.Condition = _hasMusicRoll;

        GetComponent<IDialogueTrigger>()?.TriggerDialogue();


        Parameters p = new Parameters();
        p.PutObjectExtra("Furniture", this);
        EventBroadcaster.Instance.PostEvent(EventNames.GAME_LOOP_EVENTS.ON_FURNITURE_INSPECTION, p);

        if (hasMusicRoll)
        {
            EventBroadcaster.Instance.PostEvent(EventNames.GAME_LOOP_EVENTS.ON_MUSIC_ROLL_FOUND);
        }
        //remove roll first
         
            
    }
}
