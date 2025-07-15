using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class PlayerVoice : MonoBehaviour
{
    private static PlayerVoice instance;

    public static PlayerVoice Instance
    {
        get 
        {
            return instance; 
        }
    }

    public Animator PlayerAni;

    public List<AudioClip> RunAndPushPlayerSound = new List<AudioClip>();
    public List<AudioClip> JumpSound = new List<AudioClip>();
    public List<AudioClip> GatherSound = new List<AudioClip>();
    public List<AudioClip> hitSound = new List<AudioClip>();
    public List<AudioClip> DeathSound = new List<AudioClip>();

    public enum situation
    {
        hit,
        death
    }

    public situation situ;

    [SerializeField]
    List<AudioClip> currentclipList;

    public AudioSource source;

    public bool die = false;

    public void ExerciseVoice(string name)
    {
        if (source == null)
        {
            return;
        }

        switch (name)
        {
            case "Exercise":
                source.clip = RunAndPushPlayerSound[Random.Range(0, RunAndPushPlayerSound.Count)];
                source.PlayOneShot(source.clip);
                break;

            case "Jump":
                source.clip = JumpSound[Random.Range(0, JumpSound.Count)];
                source.PlayOneShot(source.clip);
                break;

            case "Gather":
                source.clip = GatherSound[Random.Range(0, GatherSound.Count)];
                source.PlayOneShot(source.clip);
                break;
        }
    }

    public void PlayVoice()
    {
        AudioClip Selectclip = currentclipList[Random.Range(0, currentclipList.Count)];
        source.PlayOneShot(Selectclip);
    }

    private void selectStepList()
    {
        switch (situ)
        {
            case situation.hit:
                currentclipList = hitSound;
                break;

            case situation.death:
                currentclipList = DeathSound;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (PlayerMove.Instance.hp > 0)
            {
                situ = situation.hit;
            }
        }

        selectStepList();
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        PlayerAni = GetComponent<Animator>();

        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        die = false;

        if (source.isPlaying)
        {
            source.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMove.Instance != null)
        {
            if (PlayerMove.Instance.hp <= 0 && !die)
            {
                situ = situation.death;

                die = true;
            }
        }
    }
}
