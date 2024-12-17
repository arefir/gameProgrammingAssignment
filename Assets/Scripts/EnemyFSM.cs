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
    private bool inBaseRange;
    private float lastFireTime;
    private Animator animator;
    private float wanderTime;
    private bool wanderTimeSet;
    private Life life;

    public float wanderRadius = 10;
    public float fireCooldown = 1;
    public EnemyPlayerSensor chasePlayerSensor;
    public EnemyPlayerSensor attackPlayerSensor;
    public EnemyPlayerSensor attackBaseSensor;
    public GameObject playerObject;
    public GameObject enemyShootPoint;
    public GameObject enemyBulletPrefab;
    public GameObject playerBase;
    public ParticleSystem muzzleFlash;
    public AudioSource gunSfx;
    public enum EnemyStates
    {
        Wander,
        ChasePlayer,
        AttackPlayer,
        ChaseBase,
        AttackBase,
        Dead
    }

    public EnemyStates currentState = EnemyStates.ChaseBase;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.enabled = true;
        lastFireTime = Time.time;
        chasePlayerSensor.OnPlayerEnter += ChasePlayerSensor_OnPlayerEnter;
        chasePlayerSensor.OnPlayerExit += ChasePlayerSensor_OnPlayerExit;
        attackPlayerSensor.OnPlayerEnter += AttackPlayerSensor_OnPlayerEnter;
        attackPlayerSensor.OnPlayerExit += AttackPlayerSensor_OnPlayerExit;
        attackBaseSensor.OnPlayerEnter += AttackBaseSensor_OnPlayerEnter;
        attackBaseSensor.OnPlayerExit += AttackBaseSensor_OnPlayerExit;
        life = GetComponent<Life>();
    }

    void Update()
    {
        if (life.currentHp <= 0) currentState = EnemyStates.Dead;
        if (currentState == EnemyStates.Wander) { Wander(); }
        else if (currentState == EnemyStates.ChasePlayer) { ChasePlayer(); }
        else if (currentState == EnemyStates.AttackPlayer) { AttackPlayer(); }
        else if (currentState == EnemyStates.ChaseBase) { ChaseBase(); }
        else if (currentState == EnemyStates.Dead) { Dead(); }
        else { AttackBase(); }
    }

    private void Wander()
    {
        animator.Play("Walk");
        agent.enabled = true;
        agent.isStopped = false;

        if (!wanderTimeSet)
        {
            wanderTime = Time.time;
            wanderTimeSet = true;
        }


        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
        }

        if (inChaseRange) { currentState = EnemyStates.ChasePlayer; }
        if (wanderTime + 3f < Time.time)
        {
            wanderTimeSet = false;
            currentState = EnemyStates.ChaseBase;
        }

    }

    private void ChasePlayer()
    {
        animator.Play("Walk");
        agent.isStopped = false;

        agent.SetDestination(playerObject.transform.position);

        if (!inChaseRange) { currentState = EnemyStates.Wander; }
        else if (inAttackRange) { currentState = EnemyStates.AttackPlayer; }
    }

    private void AttackPlayer()
    {
        animator.Play("Shoot");
        agent.isStopped = true;

        Fire(playerObject);

        if (!inAttackRange)
        {
            if (inChaseRange) { currentState = EnemyStates.ChasePlayer; }
            else { currentState = EnemyStates.Wander; }
        }

    }

    private void ChaseBase()
    {
        animator.Play("Walk");
        agent.isStopped = false;

        agent.SetDestination(playerBase.transform.position);

        if (inBaseRange) { currentState = EnemyStates.AttackBase; }
        else if (inChaseRange) { currentState = EnemyStates.ChasePlayer; }
    }

    private void AttackBase()
    {
        animator.Play("Shoot");
        agent.isStopped = true;

        Fire(playerBase);
    }

    private void Dead()
    {
        animator.Play("Death");
        agent.isStopped = true;
        Destroy(gameObject, 3);
    }

    private void ChasePlayerSensor_OnPlayerEnter(Transform player) => inChaseRange = true;
    private void ChasePlayerSensor_OnPlayerExit(Vector3 lastKnownPosition) => inChaseRange = false;
    private void AttackPlayerSensor_OnPlayerEnter(Transform player) => inAttackRange = true;
    private void AttackPlayerSensor_OnPlayerExit(Vector3 lastKnownPosition) => inAttackRange = false;
    private void AttackBaseSensor_OnPlayerEnter(Transform player) => inBaseRange = true;
    private void AttackBaseSensor_OnPlayerExit(Vector3 lastKnownPosition) => inBaseRange = false;

    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, dist, layermask);

        return navHit.position;
    }

    private void Fire(GameObject target)
    {
        if (lastFireTime + fireCooldown < Time.time)
        {
            lastFireTime = Time.time;
            transform.LookAt(target.transform.position);
            muzzleFlash.Play();
            gunSfx.Play();
            GameObject enemyBullet = Instantiate(enemyBulletPrefab);

            enemyBullet.transform.SetPositionAndRotation(enemyShootPoint.transform.position, enemyShootPoint.transform.rotation);
        }
    }

}
