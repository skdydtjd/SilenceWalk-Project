using System.Collections;
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

    [Header("A: Path Setting")]
    public float pathUpdateInterval = 0.2f; // 경로 갱신 간격
    private float pathUpdateTimer = 0f;

    [Header("B: Link Setting")]
    protected bool isTraversingLink= false; // 링크 이동 중인지 체크

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

    IEnumerator TraverseMeshLink()
    {
        isTraversingLink = true;

        // 현재 링크 정보 가져오기
        OffMeshLinkData data = agent.currentOffMeshLinkData;

        // 시작점과 도착점 설정
        Vector3 startPos = transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

        float duration = 0.5f; // 이동 시간
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Lerp를 이용해 부드럽게 위치 이동
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        agent.CompleteOffMeshLink(); // 에이전트에게 이동 완료를 알림

        yield return new WaitForFixedUpdate();

        isTraversingLink = false;
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

        // 에이전트가 링크를 자동으로 타지 않게 설정
        agent.autoTraverseOffMeshLink = false;

        if (PlayerMove.Instance != null)
        {
            player = PlayerMove.Instance.transform;
        }

        EnemyBackGroundMusic.Instance.RegisterEnemy(this);

        GoToNextPatrolPoint();
    }

    public virtual void Update()
    {
        // 링크 위에 있고 이동 중이 아니라면 코루틴 실행
        if (agent.isOnOffMeshLink && !isTraversingLink)
        {
            StartCoroutine(TraverseMeshLink());
        }

        // 링크 이동 중에는 아래 로직(Update)을 실행하지 않음
        if (isTraversingLink)
        {
            return;
        }

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
