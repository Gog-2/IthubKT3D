using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BlindZombieAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    private int currentWaypointIndex = 0;

    [Header("Hearing (Blind Zombie)")]
    public Transform player;
    public FirstPersonController playerController; // Ссылка на скрипт игрока
    public float hearingMultiplier = 1f;

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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) 
            {
                player = playerObj.transform;
                playerController = player.GetComponent<FirstPersonController>();
            }
        }
        if (waypoints.Length > 0) agent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        if (player == null || playerController == null) return;

        HandleHearing();
        HandleMovementLogic();
        PlayFootsteps();
    }

    void HandleHearing()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        float noiseRadius = playerController.GetNoiseRadius() * hearingMultiplier;
        bool canHear = dist <= noiseRadius && noiseRadius > 0;

        if (canHear)
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