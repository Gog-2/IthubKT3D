using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float moveSpeed = 3.5f;
    private NavMeshAgent agent;
    private Camera mainCamera;
    private GameObject currentMarker;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
    }

    void Update()
    {
        HandleClickInput();
    }

    private void HandleClickInput()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        
        if (NavMesh.SamplePosition(worldPos, out NavMeshHit hit, 1.5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
    
    public Vector3 GetPosition() => transform.position;
}