using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalManager : MonoBehaviour
{
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject journalUI;
    [SerializeField] private GameObject dialogueUI;

    [SerializeField] private KeyCode closeKey;
    [SerializeField] private Button close;

    private float hiddenYPos;
    private bool isJournalActive;

    private bool isFirstDisable = true;

    private void Start()
    {
        LeanTween.init(1000);
        EventBroadcaster.Instance.AddObserver(EventNames.UI_EVENTS.ON_JOURNAL_INVOCATION, (Parameters p) => SetJournalActive(p.GetBoolExtra("willShow", false) , true) );

        hiddenYPos = journalUI.GetComponent<RectTransform>().localPosition.y;

        close.onClick.AddListener(() => SetJournalActive(false, true));

        LeanTween.delayedCall(0.5f, () => SetJournalActive(true, true));
    }

    
   
    private void SetJournalActive(bool willShow, bool willTween)
    {

        LeanTween.cancel(gameObject);

        isJournalActive = willShow;
        SetUIEnabled(!willShow, mainUI.GetComponent<CanvasGroup>());
        dialogueUI.GetComponent<DialogueText>()?.setVisible(!willShow);

        if (willShow)
        {
            Parameters p = new Parameters();
            p.PutExtra("Notification", "Page Added");
            EventBroadcaster.Instance.PostEvent(EventNames.UI_EVENTS.ON_NOTIFICATION_ADDRESSED, p);
        }
            

        void setYPosition(float val)
        {
            Vector3 pos = journalUI.GetComponent<RectTransform>().localPosition;
            pos.y = val;
            journalUI.GetComponent<RectTransform>().localPosition = pos;
        }

        if (willTween)
        {
        
            LeanTween.value(gameObject, journalUI.GetComponent<RectTransform>().position.y, willShow ? 0 : hiddenYPos, 0.5f).setEaseInOutBack().setOnUpdate((float val) =>
            {
                setYPosition(val);
            }).setIgnoreTimeScale(true);
          

        }
        else setYPosition(willShow ? 0 : hiddenYPos);
   
        
        Time.timeScale = willShow ? 0 : 1;

        //Cursor.visible = !willShow;
        Cursor.lockState = willShow ? CursorLockMode.None : CursorLockMode.Locked;

        if (isFirstDisable)
        {
            EventBroadcaster.Instance.PostEvent(EventNames.GAME_LOOP_EVENTS.ON_GAME_STARTED);
            isFirstDisable = false;
        }

 
    }

    private void Update()
    {

        if(Input.GetKeyDown(closeKey) && isJournalActive)
            SetJournalActive(false, true);
        
      
    }


    private void SetUIEnabled(bool value, CanvasGroup group)
    {
        group.alpha = value  ? 1 : 0;
        group.interactable = value;
        group.blocksRaycasts = value;
    }
    
}
