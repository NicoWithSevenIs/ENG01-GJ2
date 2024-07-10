using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    [SerializeField] private Transform pageContainer;
    [SerializeField] private Transform unlockableContainer;


    [SerializeField] private RectTransform next;
    [SerializeField] private RectTransform prev;
 
    [SerializeField] private int currentPage;

    [SerializeField] private List<UnlockablePage> unlockablePages;

    private List<RectTransform> pageObjects;

    private void Start()
    {
        
        pageObjects = new List<RectTransform>();

        foreach(Transform t in pageContainer)
        {
            t.gameObject.SetActive(true);
            t.AddComponent<CanvasGroup>();
            pageObjects.Add(t.gameObject.GetComponent<RectTransform>());
        }

        foreach (Transform t in unlockableContainer)
        {
            t.gameObject.SetActive(true);
            CanvasGroup g = t.AddComponent<CanvasGroup>();
            g.alpha = 0;
        }

        next.GetComponent<Button>().onClick.AddListener(incrementPage);
        prev.GetComponent<Button>().onClick.AddListener(decrementPage);

        
        setPage(0);

        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_MUSIC_ROLL_FOUND, AddPage);
    }


    private void setPage(int page)
    {

        currentPage = Mathf.Clamp(page, 0, pageObjects.Count - 1);

       
        for(int i =0; i < pageObjects.Count; i++)
            pageObjects[i].gameObject.GetComponent<CanvasGroup>().alpha = i == currentPage ? 1 : 0;
        

        prev.gameObject.SetActive(currentPage > 0);
        next.gameObject.SetActive(currentPage < pageObjects.Count - 1);


    }

    public void incrementPage()
    {
        setPage(currentPage + 1);
    }

    public void decrementPage()
    {
        setPage(currentPage - 1);
    }

    public void AddPage()
    {
        if (unlockablePages.Count == 0)
            return;

        int countBeforeAdding = pageObjects.Count;

        pageObjects.AddRange(unlockablePages[0].pages);
        unlockablePages.RemoveAt(0);

        setPage(countBeforeAdding);

        Parameters p = new Parameters();
        p.PutExtra("Notification", "Page Added");
        EventBroadcaster.Instance.PostEvent(EventNames.UI_EVENTS.ON_PLAYER_NOTIFIED, p);
    }


}
