using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    public void setDestination()
    {
        agent.SetDestination(player.position);
    }
}
