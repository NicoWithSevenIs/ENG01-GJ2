using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    private CanvasGroup group;
    [SerializeField] private string notificationName;

    private void Start()
    {

        group= GetComponent<CanvasGroup>();
        group.alpha = 0;
        EventBroadcaster.Instance.AddObserver(EventNames.UI_EVENTS.ON_PLAYER_NOTIFIED, (Parameters p) => {
            if (p.GetStringExtra("Notification", " ") == notificationName)
                group.alpha = 1;
        });

        EventBroadcaster.Instance.AddObserver(EventNames.UI_EVENTS.ON_NOTIFICATION_ADDRESSED, (Parameters p) => {
            if (p.GetStringExtra("Notification", " ") == notificationName)
                group.alpha = 0;
        });


    }

}
