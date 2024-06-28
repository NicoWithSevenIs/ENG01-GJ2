using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    // [On Completion] //
    private Action _onCompletion;
    public Action OnCompletion { get { return _onCompletion; } set { _onCompletion = value; } }

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

        _onCompletion = () =>
        {
            print("Yippie");
        };
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
            _onCompletion?.Invoke();
        }
        
        

    }

    //optimize this spaghetti
    private void TriggerTween(bool value, bool affectsKey)
    {
        LeanTween.cancel(keyFrameGUI);

        LeanTween.scale(keyFrameGUI, Vector3.one * (value ? 1.3f : 1f), 0.35f);

        LeanTween.value(gameObject, promiximityPromptFrameGUI.color.a, value? 0f: frameAlpha, 0.1f).setOnUpdate((value) => {
            Color c = promiximityPromptFrameGUI.color;
            c.a = value;
            promiximityPromptFrameGUI.color = c;
        });

        LeanTween.value(gameObject, actionTextGUI.color.a, value? 0f : 1f, 0.1f).setOnUpdate((value) => {
            Color c = actionTextGUI.color;
            c.a = value;
            actionTextGUI.color = c;
        });

        if(affectsKey)
        {
            Image keyFrame = keyFrameGUI.GetComponent<Image>();
            LeanTween.value(gameObject, keyFrame.color.a, value ? 0f : 1f, 0.1f).setOnUpdate((value) => {
                Color c = keyFrame.color;
                c.a = value;
                keyFrame.color = c;
            });

            foreach (Transform t in keyFrameGUI.transform)
            {

                Image i =  t.gameObject.GetComponent<Image>();
                TextMeshProUGUI text = t.GetComponent<TextMeshProUGUI>();
                if (i != null)
                {
                    LeanTween.value(gameObject, i.color.a, value ? 0f : 1f, 0.1f).setOnUpdate((value) => {
                        Color c = i.color;
                        c.a = value;
                        i.color = c;
                    });
                }

                if(text != null)
                {
                    LeanTween.value(gameObject, text.color.a, value ? 0f : 1f, 0.1f).setOnUpdate((value) => {
                        Color c = text.color;
                        c.a = value;
                        text.color = c;
                    });

                }

            }
        }
    }


}
