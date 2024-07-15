using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{

    [Header("Target")]
    [SerializeField] private GameObject player;

    [Header("Enemy")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private EnemySensor sightArea;
    [SerializeField] private float turnOffLampDuration = 3f;
    private NavMeshAgent agent;


    [Header("Targets")]
    private WaypointManager waypoints;
    private Lamp _targetLamp;
    private Transform _targetWaypoint = null;

    Coroutine TurnOffAction = null;


    private enum EnemyState { Roaming, Investigating, Chasing, TurningOff};

    private EnemyState currentState = EnemyState.Roaming;

    private void Start()
    {
        enemy.SetActive(false);
        agent = enemy.GetComponent<NavMeshAgent>();
        waypoints = GetComponent<WaypointManager>();


        //EventBroadcaster.Instance.AddObserver(EventNames.)
    }


    private void Update()
    {

        if (!enemy.activeInHierarchy)
            return;
        
        switch(currentState)
        {
            case EnemyState.Roaming: Roam(); break;
            case EnemyState.Chasing: ChasePlayer(); break;
            case EnemyState.TurningOff:

                if(TurnOffAction == null)
                    StartCoroutine(turnOffLamp());
                 
                break;
        }


    }


    private void Roam()
    {

        void SetTargetWaypoint()
        {
            Transform nearestWaypoint = waypoints.getNearestWaypoint(enemy.transform.position);

            List<Transform> adjacent = waypoints.getAdjacentWaypoints(nearestWaypoint);

            _targetWaypoint = adjacent[Random.Range(0, adjacent.Count)];
            foreach (Transform t in adjacent)
            {
                if (t.GetComponent<Lamp>().isTurnedOn)
                    _targetWaypoint = t;
            }
        }


        if (_targetWaypoint == null) 
            SetTargetWaypoint();
        else
        {

            agent.SetDestination(_targetWaypoint.position);

            if (Vector3.Distance(_targetWaypoint.position, enemy.transform.position) < 2f)
            {

                Lamp lamp = _targetWaypoint.GetComponent<Lamp>();
                if (lamp.isTurnedOn)
                    currentState = EnemyState.TurningOff;
                else _targetWaypoint = null;
            }
                
        }

        if(sightArea.Player != null)
        {
            currentState = EnemyState.Chasing;
        }

    }

    private void ChasePlayer()
    {
        if (sightArea.Player != null)
            agent.SetDestination(player.transform.position);
        else currentState = EnemyState.Roaming;


        foreach (var lamp in sightArea.TargetLamps)
        {
            if (lamp.isTurnedOn)
            {
                _targetLamp = lamp;
                currentState = EnemyState.TurningOff;
                break;
            }
        }

    }

    private IEnumerator turnOffLamp()
    {
        if (_targetLamp == null)
            yield break;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        enemy.transform.forward = _targetLamp.gameObject.transform.position;

        yield return new WaitForSeconds(turnOffLampDuration);

        if(_targetLamp != null)
            _targetLamp.setTurnedOn(false);
        

        agent.isStopped = false;

        if(sightArea.Player != null)
        {
            currentState = EnemyState.Chasing;
        }
        else
        {
            currentState = EnemyState.Roaming;
        }

        TurnOffAction = null;
        _targetWaypoint = null;
    }

}
