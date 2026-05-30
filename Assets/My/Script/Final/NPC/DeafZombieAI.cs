using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DeafZombieAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    private int currentWaypointIndex = 0;

    [Header("Vision (Deaf Zombie)")]
    public float visionRange = 15f;
    public float visionAngle = 60f;
    public LayerMask obstacleMask;

    [Header("Chase Settings")]
    public float chaseSpeed = 5f;
    private Vector3 lastKnownPosition;
    private bool isChasing = false;
    private bool isInvestigating = false;

    [Header("Movement Audio")]
    public AudioSource movementAudio;
    public AudioClip[] footstepSounds;
    public float stepInterval = 0.5f;
    private float stepTimer = 0f;

    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        if (waypoints.Length > 0) agent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        if (player == null) return;

        HandleVision();
        HandleMovementLogic();
        PlayFootsteps();
    }

    void HandleVision()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        bool canSee = false;

        if (dist <= visionRange)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < visionAngle / 2f)
            {
                if (!Physics.Linecast(transform.position + Vector3.up, player.position + Vector3.up, obstacleMask))
                {
                    canSee = true;
                }
            }
        }

        if (canSee)
        {
            isChasing = true;
            isInvestigating = false;
            lastKnownPosition = player.position;
        }
        else if (isChasing)
        {
            isChasing = false;
            isInvestigating = true;
        }
    }

    void HandleMovementLogic()
    {
        if (isChasing)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }
        else if (isInvestigating)
        {
            agent.speed = patrolSpeed;
            agent.SetDestination(lastKnownPosition);
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                isInvestigating = false;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
        else
        {
            agent.speed = patrolSpeed;
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
    }

    void PlayFootsteps()
    {
        if (movementAudio == null || footstepSounds == null || footstepSounds.Length == 0) return;
        if (agent.velocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                movementAudio.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);
                stepTimer = stepInterval;
            }
        }
    }
}