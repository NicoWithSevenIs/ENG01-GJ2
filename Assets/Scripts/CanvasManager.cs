using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    [Header("Canvas")]
    [SerializeField] private Canvas GameCanvas;
    [SerializeField] private Canvas EndScreen;

    [Header("Game Over Screen")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    private void Start()
    {
        GameCanvas.gameObject.SetActive(true);
        EndScreen.gameObject.SetActive(true);

        winScreen.SetActive(false);
        loseScreen.SetActive(false);

        GameCanvas.enabled = true;
        EndScreen.enabled = false;

        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_GAME_OVER, (Parameters p) =>
        {
            GameCanvas.enabled = false;
            EndScreen.enabled = true;

            if(p.GetBoolExtra("HasPlayerWon", true))
                winScreen.SetActive(true);   
            else
                loseScreen.SetActive(true);
            
        });
    }

 
}
