using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FurnitureManager : MonoBehaviour
{

    

    [Header("Proximity Prompt Template")]
    [SerializeField] private GameObject promptable;

    [Header("Furnitures")]
    [SerializeField] private List<Furniture> furnitureList;


    [Header("Debugging")]
    [SerializeField] private bool debugMode = true;
    [SerializeField] private GameObject debuggable;
    private List<GameObject> debuggables;
    

    private void Initialize()
    {
        debuggables = new List<GameObject>();

        foreach (Transform i in transform)
        {

          

            GameObject p = Instantiate(promptable);
            p.transform.parent = i;
            p.transform.localPosition = Vector3.zero;

            furnitureList.Add(p.GetComponent<Furniture>());


            if (debugMode)
            {
                GameObject d = Instantiate(debuggable);
                d.transform.parent = i;
                d.name = debuggable.name;
                d.transform.localPosition = Vector3.up * 2.5f;


                d.SetActive(false);
                debuggables.Add(d);
            }
     
        }




        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_FURNITURE_INSPECTION, 
            (Parameters p) =>
            {
                Furniture f = (Furniture)p.GetObjectExtra("Furniture");
                furnitureList.Remove(f);
            } 
        );

        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_MUSIC_ROLL_REFRESHED, refreshPositions);
        

    }


    public void refreshPositions(Parameters p)
    {

        int count = p.GetIntExtra("RollCount", 0);

        if (count == 0)
        {
            Debug.Log("Empty Pool");
            return;
        }

   

        List<Furniture> pool = new List<Furniture>(furnitureList);
        List<Furniture> temp = new List<Furniture>();

        if(debugMode)
            debuggables.ForEach(p => { p.SetActive(false); });

        foreach(var f in furnitureList)
        {
            f.hasMusicRoll = false;
        }

        while(temp.Count < count && pool.Count > 0)
        {
            Furniture random = pool[Random.Range(0, pool.Count)];
            pool.Remove(random);

            temp.Add(random);
        }

        foreach(var furniture in temp)
        {
            furniture.hasMusicRoll = true;

            if (debugMode)
            {
                Transform parent = furniture.transform.parent;
                parent.Find(debuggable.name).gameObject.SetActive(true);
            }
               
        }

    }

    #region singleton
    private static FurnitureManager instance = null;

    public static FurnitureManager Instance { get { return instance; } }

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
        Initialize();
    }
    #endregion


}
