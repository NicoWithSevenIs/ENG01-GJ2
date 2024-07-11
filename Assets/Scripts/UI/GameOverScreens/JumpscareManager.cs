using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpscareManager : MonoBehaviour
{
    [SerializeField] private RectTransform ghost;
    [SerializeField] private Image redScreen;

    [Header("Ghost Settings")]

    [SerializeField] private float scaleMultiplier = 3f;
    [Range(0f, 1f)] [SerializeField] private float delay = 0.3f;
    [Range(0.1f, 1f)] [SerializeField] private float tweenDuration = 0.4f;


    [Header("Red Screen Settings")]
    [Range(0f, 1f)][SerializeField] private float redScreenDelay = 0.3f;
    [Range(0f, 1f)][SerializeField] private float redScreenTweenDuration = 0.3f;


    private void Start()
    {
        void TweenRedScreen()
        {
            LeanTween.cancel(gameObject);
            LeanTween.delayedCall(redScreenDelay, () =>
            {
                LeanTween.value(gameObject, 0f,  1f, redScreenTweenDuration).setOnUpdate((float f) => { 
                    Color c = redScreen.color;
                    c.a = f;
                    redScreen.color = c;
                }).setEaseOutBack();
            });
        }

        void TweenGhost()
        {
            LeanTween.value(gameObject, ghost.localScale, ghost.localScale * scaleMultiplier, tweenDuration).setOnUpdate((Vector3 v) => ghost.localScale = v).setEaseOutQuint().setOnComplete(TweenRedScreen);
        }

        LeanTween.delayedCall(delay, TweenGhost);
       
    }

}
