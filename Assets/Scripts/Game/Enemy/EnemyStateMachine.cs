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
    [SerializeField] private float hearingRadius = 5f;
    private NavMeshAgent agent;


    [Header("Targets")]
    private WaypointManager waypoints;
    private Lamp _targetLamp;
    private Vector3 investigationTarget;


    [Header("Debuggin")]
    [SerializeField] private Transform _targetWaypoint = null;
    [SerializeField] private CircularBuffer<Transform> pathHistory;

    Coroutine TurnOffAction = null;
    Coroutine InvestigationAction = null;



    private enum EnemyState { Roaming, Chasing, TurningOff, Hunting, Investigating};
    

    [SerializeField] private EnemyState currentState = EnemyState.Roaming;

    private void Start()
    {
        enemy.SetActive(false);
        agent = enemy.GetComponent<NavMeshAgent>();
        waypoints = GetComponent<WaypointManager>();

        pathHistory = new CircularBuffer<Transform>(3);

        Transform nearestOnSpawn = waypoints.getNearestWaypoint(enemy.transform.position);
        pathHistory.Push(nearestOnSpawn);


        void SpawnEnemy()
        {
            if (enemy.activeInHierarchy)
                return;

            //Spawns the enemy n tiles away from the waypoint closest to the player. In this case, it's hard coded to 2
            List<Transform> waypointList = waypoints.getAllWaypointFromDistanceN(waypoints.getNearestWaypoint(player.transform.position), 2);
            enemy.transform.position = waypointList[Random.Range(0, waypointList.Count)].position;
            enemy.SetActive(true);

            GetComponent<StandardDialogue>().TriggerDialogue();
        }

        //Spawn enemy if it's 1 AM 
        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_HOUR_PASSED, (Parameters p) => {
            if (p.GetIntExtra("CurrentHour", 1) == 1)
                SpawnEnemy();
        });

        //Spawn enemy when the first music roll has been found
        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_MUSIC_ROLL_REFRESHED, (Parameters p) => { 
            if(p.GetIntExtra("RollCount", 1) == 3)
                SpawnEnemy(); 
        });

        //When it's 6AM, abort all actions and hunt for the player
        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_TIMES_UP, () => {
            StopAllCoroutines();
            currentState = EnemyState.Hunting;
            agent.speed *= 1.5f;   
        });

        EventBroadcaster.Instance.AddObserver(EventNames.PLAYER_ACTIONS.ON_PLAYER_RIFLING, TriggerInvestigation);

        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_GAME_OVER, () => StopAllCoroutines());
    }


    private void Update()
    {

        if (!enemy.activeInHierarchy)
            return;
        
    
        switch(currentState)
        {
            case EnemyState.Investigating: /*Investigate();*/ break;
            case EnemyState.Hunting: HuntPlayer(); break;
            case EnemyState.Roaming: Roam(); break;
            case EnemyState.Chasing: ChasePlayer(); break;
            case EnemyState.TurningOff: TurnOffLamp(); break;
        }


    }

    #region Investigating

    private void TriggerInvestigation(Parameters p)
    {

        if (currentState == EnemyState.TurningOff)
            return;

        Vector3 furniturePos = (Vector3)p.GetObjectExtra("Position");

        Vector3 agentPos = agent.gameObject.transform.position;

        if (Vector3.Distance(furniturePos, agentPos) <= hearingRadius)
        {
            investigationTarget= furniturePos;
            currentState = EnemyState.Investigating;

            if(InvestigationAction != null)
                StopCoroutine(InvestigationAction);
            InvestigationAction = StartCoroutine(InvestigateNoise());
        }
    }

    /*
    private void Investigate()
    {
        if (InvestigationAction == null)
            InvestigationAction = StartCoroutine(InvestigateNoise());
    }
    */

    private IEnumerator InvestigateNoise()
    {
        void setAgentEnabled(bool value)
        {
            if (!value)
                agent.velocity = Vector3.zero;
            agent.isStopped = !value;
        }


        //in case the coroutine gets overridden by another investigation call
        setAgentEnabled(true);

     
        //Walk over to the noise location
        agent.SetDestination(investigationTarget);


        //Wait until the agent has arrived at the target location. Can be distracted by active lamps in its path.
        while(agent.remainingDistance > 4f)
        {
            _targetWaypoint = waypoints.getNearestWaypoint(agent.gameObject.transform.position);
            if (checkPivotToLamp())
            {
                currentState = EnemyState.Roaming; //Go back to roaming, since turning off lamp immobilizes the agent
                InvestigationAction = null;
                yield break;
            }

            if (sightArea.Player != null)
            {
                currentState = EnemyState.Chasing;
                InvestigationAction = null;
                yield break;
            }

            yield return null; 
        }

        //camps the area for 3 seconds 
        float patrolTime = Time.time + 3f;

        setAgentEnabled(false);
        while (Time.time < patrolTime)
        {
            if (sightArea.Player != null)
            {
                currentState = EnemyState.Chasing;
                setAgentEnabled(true);
                InvestigationAction = null;
                yield break;
            }

            yield return null;
        }

        //if the player hasnt been spotted, return to roaming
        currentState = EnemyState.Roaming;
        InvestigationAction = null;
        setAgentEnabled(true);


    }

    #endregion

    #region Hunt
    private void HuntPlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }

    #endregion Hunt
    #region Roam
    private void Roam()
    {

        void SetTargetWaypoint()
        {
            Transform nearestWaypoint = waypoints.getNearestWaypoint(enemy.transform.position);

            List<Transform> adjacent = waypoints.getAdjacentWaypoints(nearestWaypoint);

            foreach(var waypoint in pathHistory.List)
                adjacent.Remove(waypoint);
            
           

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

            if (Vector3.Distance(_targetWaypoint.position, enemy.transform.position) < 5f)
            {

                Lamp lamp = _targetWaypoint.GetComponent<Lamp>();
                if (lamp.isTurnedOn)
                {
                    _targetLamp = lamp;
                    currentState = EnemyState.TurningOff;
                }else
                {
                    pathHistory.Push(_targetWaypoint);
                    _targetWaypoint = null;
                }
            }
                
        }

        if(sightArea.Player != null)
        {
            currentState = EnemyState.Chasing;
        }

    }

    #endregion
    #region Chase Player

    private bool checkPivotToLamp()
    {
        foreach (var lamp in sightArea.TargetLamps)
        {
            if (!lamp.isTurnedOn)
                continue;

            agent.SetDestination(lamp.transform.position);


            if (Vector3.Distance(lamp.transform.position, enemy.transform.position) < 5f)
            {
                _targetLamp = lamp;
                currentState = EnemyState.TurningOff;
                return true;
            }
            
        }

        return false;
    }

    private void ChasePlayer()
    {
        if (sightArea.Player != null)
            agent.SetDestination(player.transform.position);
        else currentState = EnemyState.Roaming;

        checkPivotToLamp();
      
    }
    #endregion
    #region Turn Off Lamp

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

    #endregion
}
