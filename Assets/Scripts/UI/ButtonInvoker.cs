using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInvoker : MonoBehaviour
{

    [SerializeField] private string eventName;
    public void Invoke()
    {
        EventBroadcaster.Instance.PostEvent(eventName);
    }
}
