using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Parameters l = new Parameters();
            l.PutExtra("HasPlayerWon", false);
            EventBroadcaster.Instance.PostEvent(EventNames.GAME_LOOP_EVENTS.ON_GAME_OVER, l);
        }
    }
}
