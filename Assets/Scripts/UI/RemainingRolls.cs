using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemainingRolls : MonoBehaviour
{
    private TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_MUSIC_ROLL_REFRESHED, (Parameters p) => 
             text.text = $"{p.GetIntExtra("RollCount", 0).ToString()} Remaining"
        );
    }




}
