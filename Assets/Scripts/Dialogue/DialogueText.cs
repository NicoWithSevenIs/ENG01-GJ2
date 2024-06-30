using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI tmp;
    // Start is called before the first frame update
    void Start()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.DIALOGUE_EVENTS.ON_DIALOGUE_INVOCATION, setText);
        EventBroadcaster.Instance.AddObserver(EventNames.DIALOGUE_EVENTS.ON_DIALOGUE_ENDED, () => tmp.enabled = false);
        tmp.enabled = false;
    }

    public void setText(Parameters p)
    {
        tmp.enabled = true;
        string line = p.GetStringExtra("Line", "");
        tmp.text = line;
    }
}
