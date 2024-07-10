using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGunInvoker : MonoBehaviour
{
    public void Invoke()
    {
        EventBroadcaster.Instance.PostEvent(EventNames.UI_EVENTS.ON_STUN_GUN_INVOCATION);
    }
}
