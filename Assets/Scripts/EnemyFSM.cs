using EasyRoads3Dv3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyFSM : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool inChaseRange;
    private bool inAttackRange;
    private float lastFireTime;

    public float wanderRadius = 10;
    public float fireCooldown = 1;
    public EnemyPlayerSensor chasePlayerSensor;
    public EnemyPlayerSensor attackPlayerSensor;
    public GameObject playerObject;
    public GameObject enemyShootPoint;
    public GameObject enemyBulletPrefab;
    public enum EnemyStates
    {
        Wander,
        ChasePlayer,
        AttackPlayer
    }

    public EnemyStates currentState = EnemyStates.Wander;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lastFireTime = Time.time;
        chasePlayerSensor.OnPlayerEnter += ChasePlayerSensor_OnPlayerEnter;
        chasePlayerSensor.OnPlayerExit += ChasePlayerSensor_OnPlayerExit;
        attackPlayerSensor.OnPlayerEnter += AttackPlayerSensor_OnPlayerEnter;
        attackPlayerSensor.OnPlayerExit += AttackPlayerSensor_OnPlayerExit;
    }

    void Update()
    {
        if (currentState == EnemyStates.Wander) { Wander(); }
        else if (currentState == EnemyStates.ChasePlayer) { ChasePlayer(); }
        else { AttackPlayer(); }
    }

    private void Wander()
    {
        agent.enabled = true;
        agent.isStopped = false;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
        }

        if (inChaseRange) { currentState = EnemyStates.ChasePlayer; }
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;

        agent.SetDestination(playerObject.transform.position);

        if (!inChaseRange) { currentState = EnemyStates.Wander; }
        else if (inAttackRange) {  currentState = EnemyStates.AttackPlayer;}
    }

    private void AttackPlayer()
    {
        agent.isStopped = true;

        if (lastFireTime + fireCooldown < Time.time)
        {
            lastFireTime = Time.time;
            transform.LookAt(playerObject.transform.position);
            GameObject enemyBullet = Instantiate(enemyBulletPrefab);
            enemyBullet.transform.SetPositionAndRotation(enemyShootPoint.transform.position, enemyShootPoint.transform.rotation);
        }

        if (!inAttackRange)
        {
            if (inChaseRange) { currentState = EnemyStates.ChasePlayer; }
            else { currentState = EnemyStates.Wander; }
        }
    }

    private void ChasePlayerSensor_OnPlayerEnter(Transform player) => inChaseRange = true;
    private void ChasePlayerSensor_OnPlayerExit(Vector3 lastKnownPosition) => inChaseRange = false;
    private void AttackPlayerSensor_OnPlayerEnter(Transform player) => inAttackRange = true;
    private void AttackPlayerSensor_OnPlayerExit(Vector3 lastKnownPosition) => inAttackRange = false;

    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, dist, layermask);

        return navHit.position;
    }

}
