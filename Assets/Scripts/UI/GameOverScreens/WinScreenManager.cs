using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenManager : MonoBehaviour
{
    private Image whiteScreen;
    private void Start()
    {

        LeanTween.delayedCall(0.5f, () => AudioManager.instance.PlaySound2D("Collected All"));
      
        whiteScreen = GetComponent<Image>();
        LeanTween.value(gameObject, 0f, 1f, 2f).setOnUpdate((float value) =>
        {
            Color c = whiteScreen.color;
            c.a= value;
            whiteScreen.color= c;
        }).setEaseInCirc();

    
    }
}
