using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HyperlinkText : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI tmp;

    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        int index = TMP_TextUtilities.FindIntersectingLink(tmp, Input.mousePosition, null);

        if(index != -1)
        {
            Application.OpenURL(tmp.textInfo.linkInfo[index].GetLinkID());
        }
    }
}
