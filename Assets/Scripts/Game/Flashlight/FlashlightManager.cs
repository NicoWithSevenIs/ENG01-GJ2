using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightManager : MonoBehaviour
{
    [SerializeField] private Light flashlight;
    [SerializeField] private float activeDuration = 5f;

    private void Start()
    {
        flashlight.enabled = false;

        EventBroadcaster.Instance.AddObserver(EventNames.UI_EVENTS.ON_FLASHLIGHT_INVOCATION, () => { 
            StopAllCoroutines();
            StartCoroutine(turnOnFlashlight()); 
        } );
    }

    //flashlight actually lasts 1 second longer as the strong flickers dont count towards the total active duration
    private IEnumerator turnOnFlashlight()
    {
        AudioManager.instance.PlaySound2D("Flashlight");

        IEnumerator flicker(float interval, int amount, bool willTurnOff)
        {
            for(int i =0; i < amount; i++)
            {
                yield return new WaitForSeconds(interval/2);
                flashlight.enabled = false;
                yield return new WaitForSeconds(interval/2);
                flashlight.enabled = true;
            }

            flashlight.enabled = !willTurnOff;
        }

        IEnumerator weakFlicker(float interval, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                flashlight.intensity = flashlight.intensity / 2f;
                yield return new WaitForSeconds(0.3f);
                flashlight.intensity = 1;
            }
        }


        yield return flicker(0.2f, 2, false);

        yield return new WaitForSeconds(activeDuration * (2f/3f));

        //visual cue that the flashlight will turn off shortly
        StartCoroutine(weakFlicker(0.3f, 1));

        yield return new WaitForSeconds(activeDuration * (1f/3f));

        yield return flicker(0.2f, 3, true);

        Parameters p = new Parameters();
        p.PutExtra("ItemName", "Flashlight");
        EventBroadcaster.Instance.PostEvent(EventNames.UI_EVENTS.ON_COOLDOWN_INVOCATION, p);

    }

}
