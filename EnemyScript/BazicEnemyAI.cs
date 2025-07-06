using UnityEngine;
using UnityEngine.AI;

public class BazicEnemyAI : MonoBehaviour
{
    private static BazicEnemyAI instance;

    public static BazicEnemyAI Instance
    {
        get { return instance; }
    }

    public enum State { Patrol, Chase, Return }
    public State currentState = State.Patrol;

    public NavMeshAgent agent;
    public Transform[] patrolPoints;
    public int currentPatrolIndex = 0;

    public float waitTimeAtPatrolPoint;
    public float waitTimer = 0;
    public bool isWaiting = false;

    public Transform player;
    public float chaseRange = 7f;
    public float loseSightTime = 3f;

    private float loseTimer = 0f;
    private Vector3 lastSeenPosition;

    private State lastState = State.Patrol;

    protected bool canMove = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (PlayerMove.Instance !=null)
        {
            player = PlayerMove.Instance.transform;
        }

        EnemyBackGroundMusic.Instance.RegisterEnemy(this);

        GoToNextPatrolPoint();
    }

    public virtual void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (currentState != lastState)
        {
            lastState = currentState;
            EnemyBackGroundMusic.Instance.CheckEnemyStates();
        }

        bool playerVisible = IsPlayerVisible();

        switch (currentState)
        {
            case State.Patrol:
                Patrol();

                if (distanceToPlayer <= chaseRange && playerVisible)
                {
                    currentState = State.Chase;
                    Debug.Log("�÷��̾� �߰�!");
                }
                break;

            case State.Chase:
                ChasePlayer(distanceToPlayer);
                break;

            case State.Return:
                ReturnToPatrol();

                if (distanceToPlayer <= chaseRange && playerVisible)
                {
                    currentState = State.Chase;
                    Debug.Log("�÷��̾� �߰�!");
                }
                break;
        }
    }

    public virtual void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isWaiting)
        {
            // ���� �� ��� ����
            isWaiting = true;
            waitTimeAtPatrolPoint = Random.Range(3f, 5f);
            waitTimer = waitTimeAtPatrolPoint;
            agent.ResetPath(); // �̵� ���߱�
        }

        // ��� ���̸� Ÿ�̸� ����
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                GoToNextPatrolPoint(); // ���� ���� �������� �̵�
            }
        }
    }

    public virtual void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) 
            return;

        currentPatrolIndex = Random.Range(0, patrolPoints.Length);
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    public void ChasePlayer(float distanceToPlayer)
    {
        if (agent.isOnNavMesh && player != null && agent.enabled)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(player.position, path);

            if (path.status != NavMeshPathStatus.PathComplete)
            {
                Debug.LogWarning("��ȿ�� ��� ����! ��ġ: " + player.position);
            }
            else
            {
                agent.SetDestination(player.position);
                Debug.Log("���� ��: " + player.position);
            }
        }

        if (distanceToPlayer <= chaseRange)
        {
            loseTimer = 0f;
            lastSeenPosition = player.transform.position;
        }
        else
        {
            loseTimer += Time.deltaTime;
            if (loseTimer >= loseSightTime)
            {
                currentState = State.Return;

                if (agent.isOnNavMesh)
                    agent.SetDestination(lastSeenPosition);

                Debug.Log("�÷��̾� ��ħ! ���� ��...");
            }
        }
    }

    public virtual void ReturnToPatrol()
    {
        if (!canMove)
        {
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Patrol;
            GoToNextPatrolPoint();
            Debug.Log("���� ����");
        }
    }

    public void RetryDestination()
    {
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    bool IsPlayerVisible()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Ray ray = new Ray(transform.position + Vector3.up, directionToPlayer);
        RaycastHit hit;

        Debug.DrawRay(transform.position + Vector3.up, directionToPlayer * chaseRange, Color.red);

        if (Physics.Raycast(ray, out hit, chaseRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true; // �þ߿� �÷��̾� ����
            }
        }

        return false; // ��ֹ��� ������ ����
    }
}
