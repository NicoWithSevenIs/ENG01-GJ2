using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement movement; //not sure if modifying the event broadcaster's allowed so im j settling with this dependency
    [SerializeField] private Image staminaBarFrame;
    [SerializeField] private Image staminaBar;
    [SerializeField] private Image staminaBarBackground;


    [Header("Configurations")]
    [SerializeField] private float inactiveWidth;
    [SerializeField] private float tweenSpeed = 0.2f;

    private float barWidth;
    private float frameWidth;

    private void Start()
    {
        setComponentsActive(false);

        barWidth = staminaBar.rectTransform.rect.width;
        frameWidth = staminaBarFrame.rectTransform.rect.width;

        setRectX(staminaBarFrame.rectTransform, inactiveWidth);
        setRectX(staminaBar.rectTransform, 0);
        setRectX(staminaBarBackground.rectTransform, 0);

        EventBroadcaster.Instance.AddObserver(EventNames.PLAYER_ACTIONS.ON_PLAYER_SPRINT_STARTED, onSprintStart);
        EventBroadcaster.Instance.AddObserver(EventNames.PLAYER_ACTIONS.ON_PLAYER_SPRINT_ENDED, onSprintEnd);
    }

    private void Update()
    {
        if (staminaBar.enabled)
        {
            staminaBar.fillAmount = movement.CurrentStamina / movement.MaxStamina;

            Color c = getRed(staminaBar.fillAmount);
            staminaBar.color = c;
            staminaBarFrame.color = c;

            c.a = staminaBarBackground.color.a;
            staminaBarBackground.color = c;
        }
            
    }

    private Color getRed(float fillAmount)
    {
        if(fillAmount > 0.5f) {
            return Color.white;
        }

        float hue, saturation, value;

        Color.RGBToHSV(Color.white, out hue, out saturation, out value);

        return Color.HSVToRGB(hue, 1 - fillAmount - 0.2f, value);

    }


    private void onSprintStart()
    {
        LeanTween.cancel(gameObject);
        setComponentsActive(true);
        tweenRectX(staminaBarFrame.rectTransform, frameWidth, tweenSpeed);
        tweenRectX(staminaBar.rectTransform, barWidth, tweenSpeed);
        tweenRectX(staminaBarBackground.rectTransform, barWidth, tweenSpeed);
    }

    private void onSprintEnd()
    {
        StopAllCoroutines();
        StartCoroutine(delayedDeactivate());
    }

    private IEnumerator delayedDeactivate()
    {

        while(movement.CurrentStamina < movement.MaxStamina)
           yield return null;
        
        LeanTween.cancel(gameObject);
        tweenRectX(staminaBarFrame.rectTransform, inactiveWidth, tweenSpeed).setOnComplete(() => setComponentsActive(false));
        tweenRectX(staminaBar.rectTransform, 0, tweenSpeed);
        tweenRectX(staminaBarBackground.rectTransform, 0, tweenSpeed);
        

    }


    private void setRectX(RectTransform r, float x)
    {
        Vector2 v = r.sizeDelta;
        v.x = x;
        r.sizeDelta = v;
    }

    private LTDescr tweenRectX(RectTransform r, float x, float duration)
    {
        Vector2 from = r.sizeDelta;
        Vector2 to = r.sizeDelta;
        to.x = x;

        LTDescr tween = LeanTween.value(gameObject, from, to, duration).setOnUpdate((Vector2 v) => r.sizeDelta = v).setEaseInOutCubic();
        return tween;
    }

    private void setComponentsActive(bool value)
    {
        staminaBar.enabled = value;
        staminaBarBackground.enabled = value;
        staminaBarFrame.enabled = value;

    }

}

