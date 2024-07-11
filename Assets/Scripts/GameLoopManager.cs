using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour
{


    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;

    [Header("Duration")]
    [SerializeField] private int hourDuration = 60;
    [SerializeField] private int totalHours = 6;
    private int totalDuration; //total duration in seconds
    private int currentHour = 12;

    [SerializeField] private int currentDuration;


    [Header("Roll Count")]
    [SerializeField] private int _rollCount;
    private int remainingRolls;


    private void Start()
    {
        totalDuration = totalHours * hourDuration;
        currentDuration = 0;

        remainingRolls = _rollCount;

        Parameters p = new Parameters();
        p.PutExtra("RollCount", remainingRolls);
        EventBroadcaster.Instance.PostEvent(EventNames.GAME_LOOP_EVENTS.ON_MUSIC_ROLL_REFRESHED, p);


        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_MUSIC_ROLL_FOUND, OnRollObtained);


        ListenForGameOver();

        StartCoroutine(GameTimer());
    }

    private void ListenForGameOver()
    {

        void DisableEverything()
        {

        }
    

        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_ALL_ROLLS_FOUND, () => { });
        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_PLAYER_CAPTURED, () => { });

    }


    private void OnRollObtained()
    {
        remainingRolls--;

        Parameters p = new Parameters();
        p.PutExtra("RollCount", remainingRolls);
        EventBroadcaster.Instance.PostEvent(EventNames.GAME_LOOP_EVENTS.ON_MUSIC_ROLL_REFRESHED, p);

        if (remainingRolls == 0)
        {
            Paramaters w = new Paramaters();

            EventBroadcaster.Instance.PostEvent(EventNames.GAME_LOOP_EVENTS.ON_GAME_OVER);
            StopAllCoroutines();
        }

    }

    private IEnumerator GameTimer()
    {

        int hourCounter = 0;
        while(currentDuration <= totalDuration)
        {
            yield return new WaitForSeconds(1);


            currentDuration++;
            hourCounter++;

            if(hourCounter == hourDuration )
            {

                currentHour = (currentHour + 1) % 12;

                if (currentHour == 0)
                    currentHour = 12;


                Parameters p = new Parameters();
                p.PutExtra("CurrentHour", currentHour);
                EventBroadcaster.Instance.PostEvent(EventNames.GAME_LOOP_EVENTS.ON_HOUR_PASSED, p);

                hourCounter = 0;
            }
           
        }

        EventBroadcaster.Instance.PostEvent(EventNames.GAME_LOOP_EVENTS.ON_TIMES_UP);

    }

    #region singleton
    private static GameLoopManager instance = null;

    public static GameLoopManager Instance { get { return instance; } }

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }
    #endregion
}
