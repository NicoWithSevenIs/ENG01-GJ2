using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HourTimer : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_HOUR_PASSED, TimeUpdater);
    }

    private void TimeUpdater(Parameters p)
    {

        int currentHour = p.GetIntExtra("CurrentHour", -1);
        text.text = currentHour.ToString() + " AM";
    }
}
