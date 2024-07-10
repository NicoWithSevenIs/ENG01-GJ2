using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalInvoker : MonoBehaviour
{
    public void Invoke()
    {
        Parameters p = new Parameters();
        p.PutExtra("willShow", true);
        EventBroadcaster.Instance.PostEvent(EventNames.UI_EVENTS.ON_JOURNAL_INVOCATION, p);

    }
}
