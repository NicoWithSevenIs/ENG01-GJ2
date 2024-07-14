using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardDialogue : MonoBehaviour, IDialogueTrigger
{
    [SerializeField] private List<string> lines;
    [SerializeField] private bool WillOverride = false;

    public void TriggerDialogue()
    {
        Parameters p = new Parameters();
        p.PutExtra("Lines", lines.ToArray());
        p.PutExtra("WillOverride", WillOverride);

        EventBroadcaster.Instance.PostEvent(EventNames.DIALOGUE_EVENTS.ON_DIALOGUE_INSERTION, p);
    }
}
