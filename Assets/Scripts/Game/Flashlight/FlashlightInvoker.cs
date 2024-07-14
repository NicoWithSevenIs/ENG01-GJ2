using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightInvoker : MonoBehaviour
{
    public void InvokeFlashlight()
    {
        EventBroadcaster.Instance.PostEvent(EventNames.UI_EVENTS.ON_FLASHLIGHT_INVOCATION);
    }
}
