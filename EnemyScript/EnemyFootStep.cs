using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class EnemyFootStep : MonoBehaviour
{
    public Animator EnemyAnim;

    public List<AudioClip> EnemyWalkSounds = new List<AudioClip>();
    public List<AudioClip> EnemyRunSounds = new List<AudioClip>();

    public enum Behavior
    {
        Walk,
        Run
    }

    Behavior behavior;

    List<AudioClip> enemyFootSounds;

    AudioSource EnemyFootSound;
     
    public void EnemyStep()
    {
        if (EnemyAnim.GetCurrentAnimatorStateInfo(0).IsName("run1"))
        {
            behavior = Behavior.Run;
        }
        else
        {
            behavior = Behavior.Walk;
        }

        selectStepList();

        AudioClip clip = enemyFootSounds[Random.Range(0, enemyFootSounds.Count)];
        EnemyFootSound.PlayOneShot(clip);
    }

    private void selectStepList()
    {
        switch (behavior)
        {
            case Behavior.Walk:
                enemyFootSounds = EnemyWalkSounds;
                break;
            case Behavior.Run:
                enemyFootSounds = EnemyRunSounds;
                break;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyAnim = GetComponent<Animator>();
        EnemyFootSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
