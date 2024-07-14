using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenManager : MonoBehaviour
{
    private Image whiteScreen;
    [Range(0f, 1f)]
    [SerializeField] private float soundOffset = 0.5f;

    [Header("Game Over Screen")]
    [SerializeField] private float gameOverScreenDelay = 1f;

    private void Start()
    {

        //this could be inheritance instead :V
        void InvokeGameOverScreen()
        {
            LeanTween.delayedCall(gameOverScreenDelay, () =>
            {
                Parameters p = new Parameters();
                p.PutExtra("HasWon", true);
                EventBroadcaster.Instance.PostEvent(EventNames.UI_EVENTS.ON_GAME_OVER_SCREEN_INVOCATION, p);
            });
        }

        LeanTween.delayedCall(soundOffset, () => AudioManager.instance.PlaySound2D("Collected All", InvokeGameOverScreen));
      
        whiteScreen = GetComponent<Image>();
        LeanTween.value(gameObject, 0f, 1f, 2f).setOnUpdate((float value) =>
        {
            Color c = whiteScreen.color;
            c.a= value;
            whiteScreen.color= c;
        }).setEaseInCirc();

    
    }
}
