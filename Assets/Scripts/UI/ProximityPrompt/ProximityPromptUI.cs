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

    private bool playerInRange;


    [Header("Hold")]
    [SerializeField] private float holdDuration = 3f;
    private float holdTime;

    [Header("Actions")]
    public UnityEvent onCompletion;
    public UnityEvent onActivationStarted;
    public UnityEvent onActivationAborted;

    // [Components] //

    private Image promiximityPromptFrameGUI;
    float frameAlpha;


    private void Awake()
    {
        keyTextGUI.text = actionKey.ToString();
        actionTextGUI.text = actionText;
        holdTime = 0;

        promiximityPromptFrameGUI = GetComponent<Image>();
        frameAlpha = promiximityPromptFrameGUI.color.a;

        isKeyHeld = false;
        isActivatable = true;

        playerInRange = false;
    }

    public void setPlayerInRange(bool value)
    {
        if (!isActivatable)
            return;

        playerInRange = value;

        if (!value)
            holdTime = 0f;

        LeanTween.cancel(keyFrameGUI);
        LeanTween.cancel(gameObject);

        TweenAlpha(promiximityPromptFrameGUI.color.a, !value ? 0f : 1f, promiximityPromptFrameGUI, 0.1f);
        setAllChildrenInvisible(!value, promiximityPromptFrameGUI.transform, 0.1f, true);

      
    }

   
    private bool canBeActivated()
    {
        bool withinBounds(float value)
        {
            return value >= 0 && value <= 1;
        }
        Vector3 viewportCoord = Camera.main.WorldToViewportPoint(transform.position);

       

        bool withinCamera = withinBounds(viewportCoord.x) && withinBounds(viewportCoord.y) && viewportCoord.z > 0;



        return Input.GetKeyDown(actionKey) && holdTime < holdDuration && isActivatable && withinCamera;
    }
   

    private void Update()
    {


        progressGUI.fillAmount = holdTime / holdDuration;

        if (!playerInRange)
            return;
            
        if (Input.GetKeyUp(actionKey))
        {   
            isKeyHeld = false;

            if (isToggle)
                isActivatable = true;
        }

        if(canBeActivated())
        {
            onActivationStarted?.Invoke();
            isKeyHeld = true;
        }


        if (isKeyHeld)
        {
            TriggerTween(true, false);
        }
        else
        {
            //if not toggle then revert size
            if (isActivatable)
            {
                TriggerTween(false, true);
            }
        }


        if (!isActivatable)
            return;

        holdTime += isKeyHeld ? Time.deltaTime : -holdDuration * Time.deltaTime;
        holdTime = Mathf.Clamp(holdTime, 0, holdDuration);


        if(holdTime == holdDuration)
        {
            TriggerTween(true, true);
            isActivatable = false;
            holdTime = 0f;
            onCompletion?.Invoke();
        }else if(holdTime < holdDuration) {
            if (Input.GetKeyUp(actionKey))
            {
                print("Aborted");
                onActivationAborted?.Invoke();
            }
        }
        
        

    }

    //optimize this spaghetti
    public void TriggerTween(bool value, bool affectsKey)
    {
        LeanTween.cancel(keyFrameGUI);

        LeanTween.scale(keyFrameGUI, Vector3.one * (value ? 1.3f : 1f), 0.35f).setIgnoreTimeScale(true);

        TweenAlpha(promiximityPromptFrameGUI.color.a, value ? 0f : 1f, promiximityPromptFrameGUI, 0.1f);
        TweenAlpha(actionTextGUI.color.a, value ? 0f : 1f, actionTextGUI, 0.1f);

        if (!affectsKey)
            return;

        Image keyFrame = keyFrameGUI.GetComponent<Image>();

        TweenAlpha(keyFrame.color.a, value ? 0f : 1f, keyFrame, 0.1f);


        setAllChildrenInvisible(value, keyFrameGUI.transform, 0.3f, false);
    }

    public void setAllChildrenInvisible(bool value, Transform parent, float duration, bool isRecursive)
    {
        foreach (Transform t in parent)
        {

            Image i = t.gameObject.GetComponent<Image>();
            TextMeshProUGUI text = t.GetComponent<TextMeshProUGUI>();

            if (i != null)
                TweenAlpha(i.color.a, value ? 0f : 1f, i, duration);

            if (text != null)
                TweenAlpha(text.color.a, value ? 0f : 1f, text, duration);

            if (isRecursive)
                setAllChildrenInvisible(value, t, duration, true);

        }
    }

  
    private void TweenAlpha(float initial, float goal, Image image, float duration)
    {
        LeanTween.value(gameObject, initial, goal, duration).setOnUpdate((value) => {
            Color c = image.color;
            c.a = value;
            image.color = c;
        }).setIgnoreTimeScale(true);
    }

    private void TweenAlpha(float initial, float goal, TextMeshProUGUI text, float duration)
    {
        LeanTween.value(gameObject, initial, goal, duration).setOnUpdate((value) => {
            Color c = text.color;
            c.a = value;
            text.color = c;
        }).setIgnoreTimeScale(true);
    }



}
