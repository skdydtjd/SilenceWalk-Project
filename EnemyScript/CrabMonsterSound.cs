using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class CrabMonsterSound : BazicEnemyAI
{
    public Animator crab;
    public AudioSource crabSound;
    public List<AudioClip> crabSounds = new List<AudioClip>();

    bool SearchSound =false;

    int direction = 1;

    public void AttackSound()
    {
        crabSound.Stop();
        crabSound.clip = crabSounds[2];
        crabSound.PlayOneShot(crabSound.clip);
    }

    public void IdleSound()
    {
        crabSound.Stop();
        crabSound.clip = crabSounds[0];
        crabSound.Play();
    }

    public override void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
        {
            return;
        }

        currentPatrolIndex = currentPatrolIndex +direction;

        if(currentPatrolIndex >= (patrolPoints.Length -1))
        {
            direction = -1;
        }
        else if(currentPatrolIndex == 0)
        {
            direction = 1;
        }

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    public override void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isWaiting)
        {
            // 도착 시 대기 시작
            isWaiting = true;
            waitTimeAtPatrolPoint = Random.Range(3f, 5f);
            waitTimer = waitTimeAtPatrolPoint;

            agent.ResetPath(); // 이동 멈추기

            crab.SetBool("Rest1", true);
        }

        // 대기 중이면 타이머 감소
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                crab.SetBool("Rest1", false);
                isWaiting = false;
                GoToNextPatrolPoint(); // 다음 순찰 지점으로 이동
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        crab = GetComponent<Animator>();
        crabSound = GetComponent<AudioSource>();

        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        float distanceToPlayerOfcrab = Vector3.Distance(transform.position, player.transform.position);

        base.Update();

        if (currentState == State.Chase)
        {
            agent.speed = 4.5f;
            crab.SetBool("Rest1", false);

            if (!SearchSound)
            {
                SearchSound = true;
                crabSound.Stop();
                crabSound.clip = crabSounds[1];
                crabSound.PlayOneShot(crabSound.clip);
            }
        }
        else if(currentState == State.Patrol || currentState == State.Return)
        {
            SearchSound = false;
            agent.speed = 2.5f;
        }

        if (currentState == State.Chase && distanceToPlayerOfcrab <= 1.6)
        {
            crab.SetBool("Attack_1", true);
        }
        else
        {
            crab.SetBool("Attack_1", false);
        }
    }
}
