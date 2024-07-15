using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    private GameObject _player = null;
    public GameObject Player { get { return _player; } }

    private List<Lamp> _targetLamps;

    public List<Lamp> TargetLamps { get { return _targetLamps; } }

    private void Start()
    {
        _targetLamps = new List<Lamp>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            _player = other.gameObject;
            return;
        }

        Lamp lamp = other.GetComponent<Lamp>();

        if(lamp != null)
            _targetLamps.Add(lamp);
        

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _player = null;
            return;
        }

        Lamp lamp = other.GetComponent<Lamp>();

        if(lamp != null)
            _targetLamps.Remove(lamp);

    }
}
