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

    public float pathUpdateInterval = 0.3f; // 경로 갱신 간격
    private float pathUpdateTimer = 0f;

    float loseTimer = 0f;
    Vector3 lastSeenPosition;

    State lastState = State.Patrol;

    protected bool canMove = true;

    public virtual void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isWaiting)
        {
            // 도착 시 대기 시작
            isWaiting = true;
            waitTimeAtPatrolPoint = Random.Range(3f, 5f);
            waitTimer = waitTimeAtPatrolPoint;
            agent.ResetPath(); // 이동 멈추기
        }

        // 대기 중이면 타이머 감소
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                isWaiting = false;
                GoToNextPatrolPoint(); // 다음 순찰 지점으로 이동
            }
        }
    }

    public virtual void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
        {
            return;
        }

        currentPatrolIndex = Random.Range(0, patrolPoints.Length);
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    public void ChasePlayer(float distanceToPlayer)
    {
        if (agent.isOnNavMesh && player != null && agent.enabled)
        {
            NavMeshPath path = new NavMeshPath();
            
            pathUpdateTimer -= Time.deltaTime;
           
            if (pathUpdateTimer <= 0f)
            {
                agent.SetDestination(player.position);
                pathUpdateTimer = pathUpdateInterval;
               
                // [보완] 경로가 불완전한지 체크 (기존의 CalculatePath 역할을 대신함)
                if (agent.pathStatus == NavMeshPathStatus.PathPartial)
                {
                    Debug.LogWarning("플레이어에게 가는 경로가 끊겨 있습니다!");
                    currentState = State.Return; // 상태 변경
                    return; // 아래 추적 로직 실행 방지
                }
                
                Debug.Log("추적 중: " + player.position);
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

                Debug.Log("플레이어 놓침! 복귀 중...");
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
            Debug.Log("순찰 복귀");
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
                return true; // 시야에 플레이어 있음
            }
        }

        return false; // 장애물에 가려져 있음
    }

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

        if (PlayerMove.Instance != null)
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
                    Debug.Log("플레이어 발견!");
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
                    Debug.Log("플레이어 발견!");
                }
                break;
        }
    }
}
