using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StunGunManager : MonoBehaviour
{
    [SerializeField] private StunGunCollision collisionHandler;
    [SerializeField] private GameObject enemy;

    [Header("Stun Stats")]
    [SerializeField] private float stunDuration = 6f;


    private float enemyWalkSpeed;
    private NavMeshAgent agent;

    private void Start()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.UI_EVENTS.ON_STUN_GUN_INVOCATION, ()=>
            {

                if (collisionHandler.IsEnemyWithinRange)
                {
                    StopAllCoroutines();
                    StartCoroutine(StunEnemy());
                }

            }
        );

        agent = enemy.GetComponent<NavMeshAgent>();
        enemyWalkSpeed = agent.speed;
    }

    private IEnumerator StunEnemy()
    {
        void SetEnemyComponentsActive(bool value)
        {
            enemy.GetComponent<Light>().enabled = !value;
            enemy.GetComponent<SpriteRenderer>().color = value ? Color.white : Color.red;
            //set enemy state machine here
            enemy.GetComponent<CapsuleCollider>().enabled = value;
            agent.speed = value ? enemyWalkSpeed : 0f;  
        }

        SetEnemyComponentsActive(false);
        agent.velocity = Vector3.zero;
        yield return new WaitForSeconds(stunDuration);
        SetEnemyComponentsActive(true);


    }



}
