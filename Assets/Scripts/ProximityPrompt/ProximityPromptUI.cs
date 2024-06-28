using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProximityPromptUI : MonoBehaviour
{

    [Header("Input Data")]
    [SerializeField] private KeyCode actionKey;
    [SerializeField] private string actionText;
    [SerializeField] private bool isToggle;

    bool isKeyHeld;
    bool isActivatable;

    [Header("References")]
    [SerializeField] private GameObject keyFrameGUI;
    [SerializeField] private TextMeshProUGUI keyTextGUI;
    [SerializeField] private TextMeshProUGUI actionTextGUI;
    [SerializeField] private Image progressGUI;


    [Header("Hold")]
    [SerializeField] private float holdDuration = 3f;
    private float holdTime;

    [Header("Actions")]
    public UnityEvent onCompletion;

    // [Components] //

    private Image promiximityPromptFrameGUI;
    float frameAlpha;


    private void Start()
    {
        keyTextGUI.text = actionKey.ToString();
        actionTextGUI.text = actionText;
        holdTime = 0;

        promiximityPromptFrameGUI = GetComponent<Image>();
        frameAlpha = promiximityPromptFrameGUI.color.a;

        isKeyHeld = false;
        isActivatable = true;

 
    }

    private void Update()
    {



        Color originalColor = GetComponent<Image>().color;

        if (Input.GetKey(actionKey))
        {
            isKeyHeld = true;
            TriggerTween(true, false);
        }
           

        if (Input.GetKeyUp(actionKey))
        {
            isKeyHeld = false;
           

            if (isToggle)
                isActivatable = true;

            if(isActivatable)
                TriggerTween(false, true);

        }

        if (!isActivatable)
            return;



        holdTime += isKeyHeld ? Time.deltaTime : -holdDuration * Time.deltaTime;
        holdTime = Mathf.Clamp(holdTime, 0, holdDuration);

        progressGUI.fillAmount = holdTime / holdDuration;


        if(holdTime == holdDuration)
        {
            TriggerTween(true, true);
            isActivatable = false;
            holdTime = 0f;
            onCompletion?.Invoke();
        }
        
        

    }

    //optimize this spaghetti
    private void TriggerTween(bool value, bool affectsKey)
    {
        LeanTween.cancel(keyFrameGUI);

        LeanTween.scale(keyFrameGUI, Vector3.one * (value ? 1.3f : 1f), 0.35f);

        TweenAlpha(promiximityPromptFrameGUI.color.a, value ? 0f : 1f, promiximityPromptFrameGUI);
        TweenAlpha(actionTextGUI.color.a, value ? 0f : 1f, actionTextGUI);

        if (!affectsKey)
            return;

        Image keyFrame = keyFrameGUI.GetComponent<Image>();

        TweenAlpha(keyFrame.color.a, value ? 0f : 1f, keyFrame);

        foreach (Transform t in keyFrameGUI.transform)
        {

            Image i =  t.gameObject.GetComponent<Image>();
            TextMeshProUGUI text = t.GetComponent<TextMeshProUGUI>();

            if (i != null)
                TweenAlpha(i.color.a, value ? 0f : 1f, i);

            if (text != null)       
                TweenAlpha(text.color.a, value ? 0f : 1f, text);

        }
        
    }

  
    private void TweenAlpha(float initial, float goal, Image image)
    {
        LeanTween.value(gameObject, initial, goal, 0.1f).setOnUpdate((value) => {
            Color c = image.color;
            c.a = value;
            image.color = c;
        });
    }

    private void TweenAlpha(float initial, float goal, TextMeshProUGUI text)
    {
        LeanTween.value(gameObject, initial, goal, 0.1f).setOnUpdate((value) => {
            Color c = text.color;
            c.a = value;
            text.color = c;
        });
    }

}
