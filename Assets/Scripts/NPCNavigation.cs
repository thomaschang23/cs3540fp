using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCNavigation : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        StartWalk,
        Walk,
        StopWalk,
    }

    public float stopDistance = 10.0f;
    public float hailDistance = 25.0f;
    public GameObject player;
    public Transform[] wanderPoints;

    private bool fwd;
    private float speed;
    private FSMStates currentState = FSMStates.Idle;
    private Animator anim;
    private Vector3 nextDestination;
    private float distanceToPlayer;
    private float elapsedTime = 0.0f;
    private int currentDestinationIndex = 0;
    private NavMeshAgent agent;


    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        agent.speed = 0;
        FindNextPoint();
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        elapsedTime += Time.deltaTime;
        switch (currentState)
        {
            case FSMStates.Idle:
                UpdateIdleState();
                break;
            case FSMStates.StartWalk:
                UpdateStartWalkState();
                break;
            case FSMStates.Walk:
                UpdateWalkState();
                break;
            case FSMStates.StopWalk:
                UpdateStopWalkState();
                break;
        }
    }

    void UpdateIdleState()
    {
        anim.SetInteger("animState", 1);
        if (distanceToPlayer <= hailDistance)
        {
            FaceTarget(player.transform.position);
        }
        else if (FlagManager.CheckFlag("meetRo") && distanceToPlayer > hailDistance)
        {
            elapsedTime = 0.0f;
            currentState = FSMStates.StartWalk;
        }
    }

    void UpdateStartWalkState()
    {
        // anim.SetInteger("animState", 2);
        anim.SetInteger("animState", 3);
        if (agent.speed < speed - 0.2f)
        {
            agent.speed += 0.2f;
        }
        FaceTarget(nextDestination);
        agent.SetDestination(nextDestination);
        if (elapsedTime > 1.1f)
        {
            agent.speed = speed;
            currentState = FSMStates.Walk;
        }
    }

    void UpdateWalkState()
    {
        // anim.SetInteger("animState", 3);
        if (Vector3.Distance(transform.position, nextDestination) < stopDistance)
        {
            FindNextPoint();
        }
        else if (distanceToPlayer <= hailDistance)
        {
            elapsedTime = 0.0f;
            currentState = FSMStates.StopWalk;
        }
        FaceTarget(nextDestination);
        agent.SetDestination(nextDestination);
    }

    void UpdateStopWalkState()
    {
        // anim.SetInteger("animState", 4);
        if (agent.speed > 0.2f)
        {
            agent.speed -= 0.2f;
        }
        if (elapsedTime > 1.1f)
        {
            agent.speed = 0.0f;
            currentState = FSMStates.Idle;
        }
    }

    void FindNextPoint()
    {
        fwd = !(currentDestinationIndex == wanderPoints.Length - 1) && (fwd || currentDestinationIndex == 0);
        currentDestinationIndex += fwd ? 1 : -1;
        nextDestination = wanderPoints[currentDestinationIndex].position;
        agent.SetDestination(nextDestination);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0.0f;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        if (Quaternion.Angle(transform.rotation, lookRotation) > 20.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
        }
    }
}