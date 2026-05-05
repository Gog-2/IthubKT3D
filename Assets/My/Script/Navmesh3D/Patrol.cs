using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Patrol : MonoBehaviour {

    public Transform[] points;
    [SerializeField] private TMPro.TMP_Text text;
    private int _destPoint = 0;
    private NavMeshAgent _agent;
    private PatrolState patrolState
    {
        get { return _patrolState; }
        set
        {
            _patrolState = value; 
            State();
        }
    }
    private PatrolState _patrolState;
    private int _enemy;


    void Start () {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = false;
        patrolState = PatrolState.ShortPath;
        GotoNextPoint();
        _enemy = NavMesh.GetAreaFromName("Enemy");
    }


    void GotoNextPoint() {
        if (points.Length == 0)
                return;
        _agent.destination = points[_destPoint].position;
            
        _destPoint = (_destPoint + 1) % points.Length;
    }


    void Update ()
    {
        ChangeState();
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }

    private void ChangeState()
    {
        if (Input.GetKeyDown(KeyCode.A)) patrolState = PatrolState.ShortPath;
        if (Input.GetKeyDown(KeyCode.S)) patrolState = PatrolState.Stealth;
    }

    private void State()
    {
        switch (patrolState)
        {
            case PatrolState.ShortPath:
                _agent.SetAreaCost(_enemy,1);
                text.text = "Status: Short Path";
                break;
            case PatrolState.Stealth:
                _agent.SetAreaCost(_enemy, 15);
                text.text = "Status: Stealth";
                break;
        }
    }
}

public enum PatrolState
{
    ShortPath,
    Stealth,
}
