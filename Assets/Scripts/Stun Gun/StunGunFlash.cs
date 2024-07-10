using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StunGunFlash : MonoBehaviour
{
    [SerializeField] private Image flashScreen;
    [SerializeField] private float flashDuration;

    [SerializeField] private float alphaCeiling = 1f;
    [SerializeField] private float alphaFloor = 0f;
  
    private void Start()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.UI_EVENTS.ON_STUN_GUN_INVOCATION, () => {        
            StopAllCoroutines();
            LeanTween.cancel(gameObject);
            TweenFlash(alphaFloor, alphaCeiling, 0.2f).setOnComplete(() => StartCoroutine(DelayedReversal()));
        });


    }

    private LTDescr TweenFlash(float from, float to, float duration)
    {
        return LeanTween.value(gameObject, from, to, duration).setOnUpdate((float value) => {
            Color c = flashScreen.color;
            c.a = value;
            flashScreen.color = c;
        }).setEaseOutCirc();

    }
    //ill make a task library later :'3
    private IEnumerator DelayedReversal()
    {
        yield return new WaitForSeconds(flashDuration);
        TweenFlash(alphaCeiling, alphaFloor, 0.2f);
    }


}
