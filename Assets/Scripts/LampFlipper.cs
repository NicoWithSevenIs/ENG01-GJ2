using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampFlipper : MonoBehaviour
{
    [SerializeField] private Lamp lamp;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lamp.flipSwitch();
        }
    }
}
