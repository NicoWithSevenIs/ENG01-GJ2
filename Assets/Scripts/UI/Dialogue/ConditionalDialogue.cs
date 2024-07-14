using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class ConditionalDialogue : MonoBehaviour, IDialogueTrigger
{
    [SerializeField] private List<string> ifTrue;
    [SerializeField] private List<string> ifFalse;
    [SerializeField] private bool WillOverride = false;

    private bool _Condition = false;
    public bool Condition { get { return _Condition; } set { _Condition = value; } }

    public void TriggerDialogue()
    {
        Parameters p = new Parameters();
        p.PutExtra("Lines", _Condition ? ifTrue.ToArray() : ifFalse.ToArray());
        p.PutExtra("WillOverride", WillOverride);

        EventBroadcaster.Instance.PostEvent(EventNames.DIALOGUE_EVENTS.ON_DIALOGUE_INSERTION, p);
    }
}
