using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSound : BazicEnemyAI
{
    public Animator Ghoul;
    public AudioSource ghoulSound;
    public List<AudioClip> ghoulSounds = new List<AudioClip>();

    int currentSound;

    float RandomIdleMotion;
    bool FindPlayer = false;
    bool BeforeReturnFlag = false;

    public float visibleTime = 5f;
    public float invisibleTime = 3f;

    [SerializeField]
    float timer;
    bool isVisible = true;
    bool isFading = false;

    SkinnedMeshRenderer meshRenderer;
    Material ghostMaterial;
    float fadeDuration = 1f;

    float stuckCheckDistance = 1.5f;

    public void GhoulIdleSound()
    {
        currentSound = Random.Range(0, 3);

        ghoulSound.Stop();
        ghoulSound.clip = ghoulSounds[currentSound];
        ghoulSound.PlayOneShot(ghoulSound.clip);
    }

    public void Scream()
    {
        ghoulSound.Stop();
        ghoulSound.clip = ghoulSounds[4];
        ghoulSound.PlayOneShot(ghoulSound.clip);
    }

    public void GhoulAttackSound()
    {
        ghoulSound.Stop();
        ghoulSound.clip = ghoulSounds[5];
        ghoulSound.PlayOneShot(ghoulSound.clip);
    }

    public override void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isWaiting)
        {
            // 도착 시 대기 시작
            isWaiting = true;
            waitTimeAtPatrolPoint = Random.Range(5f, 6f);
            waitTimer = waitTimeAtPatrolPoint;

            RandomIdleMotion = Random.value;

            if (RandomIdleMotion <= 0.5f)
            {
                Ghoul.SetBool("Idle1", true);
            }
            else
            {
                Ghoul.SetBool("Idle2", true);
            }
        }

        // 대기 중이면 타이머 감소
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            agent.ResetPath(); // 이동 멈추기

            if (waitTimer <= 0f)
            {
                Ghoul.SetBool("Idle1",false);
                Ghoul.SetBool("Idle2",false );
                isWaiting = false;
                GoToNextPatrolPoint(); // 다음 순찰 지점으로 이동
            }
        }
    }

    public override void ReturnToPatrol()
    {
        if(!BeforeReturnFlag)
        {
            StartCoroutine(BeforeReturn());
        }
    }

    IEnumerator BeforeReturn()
    {
        BeforeReturnFlag = true;
        canMove = false;

        Ghoul.SetBool("Find", false);
        yield return new WaitForSeconds(2.5f);

        canMove = true;
        base.ReturnToPatrol() ;

        BeforeReturnFlag= false;
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = ghostMaterial.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            ghostMaterial.color = new Color(color.r, color.g, color.b, alpha);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        ghostMaterial.color = new Color(color.r, color.g, color.b, 0f);
        meshRenderer.enabled = false;

        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
    }

    IEnumerator FadeIn()
    {
        meshRenderer.enabled = true;

        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
 
        float elapsedTime = 0f;
        Color color = ghostMaterial.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            ghostMaterial.color = new Color(color.r, color.g, color.b, alpha);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        ghostMaterial.color = new Color(color.r, color.g, color.b, 1f);
    }

    private IEnumerator FadeInWrapper()
    {
        isFading = true;

        yield return StartCoroutine(FadeIn());

        isFading = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        Ghoul = GetComponent<Animator>();
        ghoulSound = GetComponent<AudioSource>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        if (meshRenderer != null)
        {
            ghostMaterial = meshRenderer.material;
        }

        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base .Update();

        float distanceToPlayerOfGhoul = Vector3.Distance(transform.position, player.transform.position);

        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance &&
            distanceToPlayerOfGhoul > stuckCheckDistance)
        {
            Debug.Log("코앞에서 멈춤 감지! 목적지 다시 설정");
            agent.SetDestination(player.position);
        }

        if (currentState == State.Chase)
        {
            agent.speed = 4.5f;
            Ghoul.SetBool("Idle1", false);
            Ghoul.SetBool("Idle2", false);

            Ghoul.SetBool("Find", true);

            if (!isVisible && !isFading)
            {
                StopAllCoroutines();
                StartCoroutine(FadeInWrapper());
                isVisible = true;
                timer = visibleTime;

                if (!FindPlayer)
                {
                    FindPlayer = true;
                    ghoulSound.Stop();
                    ghoulSound.clip = ghoulSounds[3];
                    ghoulSound.PlayOneShot(ghoulSound.clip);
                }

            }

            if (distanceToPlayerOfGhoul <= 1.4)
            {
                Ghoul.SetBool("Attack", true);
            }
            else
            {
                Ghoul.SetBool("Attack", false);
            }
        }
        else if (currentState == State.Patrol || currentState == State.Return)
        {
            FindPlayer = false;
            agent.speed = 2;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isVisible = !isVisible;

            if (isVisible)
                StartCoroutine(FadeIn());
            else
                StartCoroutine(FadeOut());

            timer = isVisible ? visibleTime : invisibleTime;
        }
    }
}
