using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGunCollision : MonoBehaviour
{
    [SerializeField] private bool isEnemyWithinRange = false;
    public bool IsEnemyWithinRange { get { return isEnemyWithinRange; } }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ghost")
            isEnemyWithinRange= true; 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ghost")
            isEnemyWithinRange = false;
    }
}
