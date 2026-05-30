using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TurretAI : MonoBehaviour
{
    [Header("Power Settings")]
    public bool isPowered = true;
    [Header("Patrol Settings")]
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public float scanDuration = 5f; // Время сканирования на точке
    private int currentWaypointIndex = 0;
    private bool isScanning = false;
    private float scanTimer = 0f;

    [Header("Turret & Laser")]
    public Transform turretHead; // Дочерний объект башни
    public float rotationSpeed = 50f;
    public float laserRange = 30f;
    public LayerMask obstacleMask; // Слой стен (препятствий)
    public LineRenderer lineRenderer; 
    public Color laserColorIdle = Color.red;
    public Color laserColorActive = Color.yellow;

    [Header("Combat")]
    public float fireRate = 0.5f;
    public float damage = 10f;
    public float attackRange = 25f;
    private float nextFireTime = 0f;

    [Header("Movement Audio")]
    public AudioSource movementAudio;
    public AudioClip movementSound;

    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        if (waypoints.Length > 0) agent.SetDestination(waypoints[0].position);

        // ИСПРАВЛЕНИЕ 1: Делаем лазер тонким
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2; 
            lineRenderer.startWidth = 0.1f; // Тонкий луч
            lineRenderer.endWidth = 0.1f;   // Тонкий луч
            lineRenderer.useWorldSpace = true;
        }

        if (movementAudio != null && movementSound != null)
        {
            movementAudio.clip = movementSound;
            movementAudio.loop = true;
            movementAudio.Play();
        }
    }

    void Update()
    {
        if (waypoints.Length == 0) return;
        
        if (!isPowered)
        {
            agent.isStopped = true;
            if (lineRenderer != null) lineRenderer.enabled = false;
            return;
        }

        agent.isStopped = false;
        HandlePatrolAndScan();
        HandleCombat();
    }

    void HandlePatrolAndScan()
    {
        if (isScanning)
        {
            agent.isStopped = true;
            // Башня постоянно крутится, сканируя пространство
            if (turretHead != null)
            {
                turretHead.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
            }
            
            scanTimer -= Time.deltaTime;
            if (scanTimer <= 0f)
            {
                isScanning = false;
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
                agent.isStopped = false;
            }
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            isScanning = true;
            scanTimer = scanDuration;
        }
    }

    void HandleCombat()
    {
        if (player == null) return;

        Vector3 rayOrigin = turretHead != null ? turretHead.position : transform.position;
        // Направление СТРОГО вперед из башни
        Vector3 rayDirection = turretHead != null ? turretHead.forward : transform.forward;
        
        bool playerDetected = false;

        // 1. Визуальный лазер (LineRenderer)
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.startColor = laserColorIdle;
            lineRenderer.endColor = laserColorIdle;
            lineRenderer.SetPosition(0, rayOrigin);
            
            // Визуальный луч обрывается, если попадает в стену (obstacleMask)
            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitVisual, laserRange, obstacleMask))
                lineRenderer.SetPosition(1, hitVisual.point);
            else
                lineRenderer.SetPosition(1, rayOrigin + rayDirection * laserRange);
        }

        // 2. Боевой Raycast (ИСПРАВЛЕНИЕ 2: Стрельба только вперед!)
        // Мы пускаем луч в направлении башни (rayDirection), а не на игрока!
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitCombat, attackRange))
        {
            // Если луч во что-то попал, проверяем, не игрок ли это.
            // (Если между турелью и игроком стена, луч попадет в стену, а не в игрока)
            if (hitCombat.transform.CompareTag("Player"))
            {
                playerDetected = true;
            }
        }

        // 3. Реакция на обнаружение игрока
        if (playerDetected)
        {
            // Меняем цвет лазера на боевой
            if (lineRenderer != null)
            {
                lineRenderer.startColor = laserColorActive;
                lineRenderer.endColor = laserColorActive;
            }

            // Стрельба
            if (Time.time >= nextFireTime)
            {
                Debug.Log("Турель стреляет по игроку!");
        
                // Наносим урон игроку
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage((int)damage);
                }
                
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }
}