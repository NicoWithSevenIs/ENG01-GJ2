using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform player;

    private Vector3 playerPos;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerPos = player.position;
        agent.SetDestination(playerPos);
    }

    // Update is called once per frame
    void Update()
    {
        if(player.position != playerPos) {
            agent.SetDestination(player.position);
            playerPos = player.position;
        }
    }
}
