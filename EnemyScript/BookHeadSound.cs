using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookHeadSound : BazicEnemyAI
{
    public AudioSource BookHeadsound;
    public List<AudioClip> BookHeadsounds = new List<AudioClip>();
    public Animator bookheadAnim;

    public float playerViewAngle = 90f;

    public bool isFrozen = false;
    bool find = false;

    bool lastViewState = false;

    public void BookAttackSound()
    {
        BookHeadsound.Stop();
        BookHeadsound.clip = BookHeadsounds[0];
        BookHeadsound.PlayOneShot(BookHeadsound.clip);
    }

    public override void Patrol()
    {

    }

    public override void ReturnToPatrol()
    {
        // Patrol�� ���ư��� �ʰ� �� �ڸ����� ���
        currentState = State.Patrol;
        agent.isStopped = true;
        Debug.Log("�÷��̾� ��ħ: ���ڸ� ���");
    }

    private bool IsInPlayersView()
    {
        Vector3 dirToEnemy = (transform.position - player.position).normalized;
        float angle = Vector3.Angle(player.forward, dirToEnemy);

        if (angle < playerViewAngle / 2f)
        {
            if (Physics.Raycast(player.position + Vector3.up, dirToEnemy, out RaycastHit hit, 30f))
            {
                Debug.DrawRay(dirToEnemy, hit.point);
                return hit.transform == this.transform;
            }
        }
        return false;
    }

    IEnumerator ResumeAfterFreeze()
    {
        yield return new WaitForSeconds(0.1f);

        agent.velocity = Vector3.zero;
        agent.isStopped = false;
        bookheadAnim.speed = 1f;

        if (player != null && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }

        isFrozen = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        BookHeadsound = GetComponent<AudioSource>();
        bookheadAnim = GetComponent<Animator>();
        base.Start();

        currentState = State.Patrol;
        agent.isStopped = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        float distanceToPlayerOfBookHead = Vector3.Distance(transform.position, player.transform.position);

        base.Update();

        bool currentViewState = IsInPlayersView();

        if (currentState == State.Chase)
        {
            bookheadAnim.SetBool("Catch", true);

            if (!find)
            {
                find = true;
                BookHeadsound.Stop();
                BookHeadsound.clip = BookHeadsounds[1]; // �߰� ����
                BookHeadsound.PlayOneShot(BookHeadsound.clip);
            }

            if (currentViewState && !lastViewState)
            {
                // ���� �þ߿� ����
                agent.isStopped = true;
                bookheadAnim.speed = 0f;
                isFrozen = true;
                Debug.Log("�÷��̾��� �þ߿� ����: ����");
            }
            else if (!currentViewState && lastViewState)
            {
                // �þ߿��� ���

                if(StartCoroutine(ResumeAfterFreeze()) != null)
                {
                    StopCoroutine(ResumeAfterFreeze());
                }

                StartCoroutine(ResumeAfterFreeze());

                Debug.Log("�þ� ���: �ٽ� ����");
            }

            if (distanceToPlayerOfBookHead <= 1f)
            {
                bookheadAnim.SetBool("Attack", true);
            }
            else
            {
                bookheadAnim.SetBool("Attack", false);
            }

            lastViewState = currentViewState;
        }
        else
        {
            bookheadAnim.SetBool("Catch", false);
        }
    }
}
