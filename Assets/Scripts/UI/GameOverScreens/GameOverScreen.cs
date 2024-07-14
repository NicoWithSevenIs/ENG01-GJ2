using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI text;
    private CanvasGroup group;

    private void Start()
    {
        group = GetComponent<CanvasGroup>();

        void setInteractable(bool value)
        {
            group.alpha = value? 1f :0f;
            group.interactable = value;
            group.blocksRaycasts= value;
        }

     

        setInteractable(false);

        EventBroadcaster.Instance.AddObserver(EventNames.UI_EVENTS.ON_GAME_OVER_SCREEN_INVOCATION, (Parameters p) =>
        {
            bool hasWon = p.GetBoolExtra("HasWon", false);
            text.text = hasWon ? "Escaped" : "You Died";
         

            void setColor(Transform trans)
            {
                foreach (Transform t in trans)
                {
                    TextMeshProUGUI tmp = t.GetComponent<TextMeshProUGUI>();
                    Image i = t.GetComponent<Image>();

                    Color c = hasWon ? Color.black : Color.white;

                    if (tmp != null) tmp.color = c;
                    if (i != null) i.color = c;

                    setColor(t);

                }
            }

            setColor(transform);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            setInteractable(true);
        });
    }

    
}
