using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] private GameObject[] emittables;
    [SerializeField] private GameObject l;
    [SerializeField] private bool turnedOnByDefault = false;

    private bool _isTurnedOn = true;
    public bool isTurnedOn { get { return _isTurnedOn; } set { _isTurnedOn = value; } }

    private void Start()
    {
        if(!turnedOnByDefault)
            flipSwitch();
    }

    public void flipSwitch()
    {
        isTurnedOn = !isTurnedOn;
        setTurnedOn(isTurnedOn);
    }

    public void setTurnedOn(bool value)
    {
        l.SetActive(isTurnedOn);

        foreach (var emittable in emittables)
        {
            Material mat = emittable.GetComponent<MeshRenderer>().material;

            if (isTurnedOn)
                mat.EnableKeyword("_EMISSION");
            else mat.DisableKeyword("_EMISSION");
        }
    }
}
