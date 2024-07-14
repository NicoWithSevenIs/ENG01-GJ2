using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        EventBroadcaster.Instance.RemoveAllObservers();
        EventBroadcaster.Instance.AddObserver(EventNames.APPLICATION_EVENTS.ON_PLAY, ApplicationActions.Play);
        EventBroadcaster.Instance.AddObserver(EventNames.APPLICATION_EVENTS.ON_EXIT, ApplicationActions.Quit);
    }
}
