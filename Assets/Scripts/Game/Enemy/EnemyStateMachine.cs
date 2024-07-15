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


    private enum EnemyState { Roaming, Chasing, TurningOff};

    [SerializeField] private EnemyState currentState = EnemyState.Roaming;

    private void Start()
    {
        enemy.SetActive(false);
        agent = enemy.GetComponent<NavMeshAgent>();
        waypoints = GetComponent<WaypointManager>();

        void SpawnEnemy()
        {
            if (enemy.activeInHierarchy)
                return;

            List<Transform> waypointList = waypoints.getAllWaypointFromDistanceN(waypoints.getNearestWaypoint(player.transform.position), 2);
            enemy.transform.position = waypointList[Random.Range(0, waypointList.Count)].position;
            enemy.SetActive(true);

            GetComponent<StandardDialogue>().TriggerDialogue();
        }


        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_HOUR_PASSED, (Parameters p) => {
            if (p.GetIntExtra("CurrentHour", 1) == 1)
                SpawnEnemy();
        });

        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_MUSIC_ROLL_REFRESHED, (Parameters p) => { 
            if(p.GetIntExtra("RollCount", 1) == 3)
                SpawnEnemy(); 
        });
    }


    private void Update()
    {

        if (!enemy.activeInHierarchy)
            return;
        
        switch(currentState)
        {
            case EnemyState.Roaming: Roam(); break;
            case EnemyState.Chasing: ChasePlayer(); break;
            case EnemyState.TurningOff: TurnOffLamp(); break;
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
                {
                    _targetLamp = lamp;
                    currentState = EnemyState.TurningOff;
                }
                    
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

                agent.SetDestination(lamp.transform.position);


                if (Vector3.Distance(lamp.transform.position, enemy.transform.position) < 2f)
                {
                    _targetLamp = lamp;
                    currentState = EnemyState.TurningOff;
                }
                
                break;
            }
        }

    }


    private void TurnOffLamp()
    {
        if (TurnOffAction == null)
            TurnOffAction = StartCoroutine(turnOffLamp());
    }

    private IEnumerator turnOffLamp()
    {

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        enemy.transform.forward = _targetLamp.gameObject.transform.position;

        yield return new WaitForSeconds(turnOffLampDuration);


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
        _targetLamp = null;
        _targetWaypoint = null;
    }

}
