using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    [SerializeField] private Transform pageContainer;

    [SerializeField] private RectTransform next;

    [SerializeField] private RectTransform prev;
 
    [SerializeField] private int currentPage;

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

        next.GetComponent<Button>().onClick.AddListener(incrementPage);
        prev.GetComponent<Button>().onClick.AddListener(decrementPage);

        
        setPage(0);
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


}
