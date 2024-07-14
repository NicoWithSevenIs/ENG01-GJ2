using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FurnitureSFX : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    public void OnSearchStarted()
    {
        if(canvas.enabled)
            AudioManager.instance.PlaySound(gameObject, "Rifle", null, true);
    }

    public void OnSearchStopped()
    {
        print("Stopped");
        AudioManager.instance.StopPlaying(gameObject);
    }
}
