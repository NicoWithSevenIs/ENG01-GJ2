using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FurnitureSFX : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    public void OnSearchStarted()
    {
        if (canvas.enabled)
        {
            int sfxVariant = Random.Range(1, 6);

            print(sfxVariant);

            AudioManager.instance.StopPlaying(gameObject);
            AudioManager.instance.PlaySound(gameObject, "Search " + sfxVariant.ToString(), null, true);

            Parameters p = new Parameters();
            p.PutObjectExtra("Position", transform.position);
            EventBroadcaster.Instance.PostEvent(EventNames.PLAYER_ACTIONS.ON_PLAYER_RIFLING, p);
        }
            
    }

    public void OnSearchStopped()
    {
        AudioManager.instance.StopPlaying(gameObject);
    }
}
