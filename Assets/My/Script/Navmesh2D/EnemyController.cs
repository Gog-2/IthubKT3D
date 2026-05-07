using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [Header("Настройки преследования")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float updatePathInterval = 0.2f;
    [SerializeField] private float stoppingDistance = 0.5f;

    [Header("Ссылка на игрока")]
    [SerializeField] private Transform playerTransform;

    private NavMeshAgent agent;
    private float pathUpdateTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
        agent.stoppingDistance = stoppingDistance;
        
        if (playerTransform == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
            else
                Debug.LogWarning("EnemyController: игрок не найден! Назначь тег 'Player' или укажи вручную.");
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        pathUpdateTimer -= Time.deltaTime;

        if (pathUpdateTimer <= 0f)
        {
            pathUpdateTimer = updatePathInterval;
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        if (NavMesh.SamplePosition(playerTransform.position, out NavMeshHit hit, 1.5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}