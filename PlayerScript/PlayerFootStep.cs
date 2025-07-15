using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerFootStep : MonoBehaviour
{
    public Animator PlayerAni;

    public List<AudioClip> Prisonwalks = new List<AudioClip>();
    public List<AudioClip> Outsidewalks = new List<AudioClip>();

    public List<AudioClip> PrisonJumps = new List<AudioClip>();
    public List<AudioClip> OutsideJumps = new List<AudioClip>();

    public List<AudioClip> PrisonRuns = new List<AudioClip>();
    public List<AudioClip> OutsideRuns = new List<AudioClip>();

    enum Surface
    {
        prison,
        outside,
        prisonjump,
        outsidejump,
        prinsonRun,
        outsideRun
    }

    Surface surface;

    List<AudioClip> currentclipList;

    AudioSource source;

    public void PlayStep()
    {
        AudioClip clip = currentclipList[Random.Range(0, currentclipList.Count)];
        source.PlayOneShot(clip);
    }

    private void selectStepList()
    {
        switch (surface)
        {
            case Surface.prison:
                currentclipList = Prisonwalks;
                break;
            case Surface.outside:
                currentclipList = Outsidewalks;
                break;
            case Surface.prisonjump:
                currentclipList = PrisonJumps;
                break;
            case Surface.outsidejump:
                currentclipList = OutsideJumps;
                break;
            case Surface.prinsonRun:
                currentclipList = PrisonRuns;
                break;
            case Surface.outsideRun:
                currentclipList = OutsideRuns;
                break;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "prison")
        {
            surface = Surface.prison;

            if (PlayerAni.GetCurrentAnimatorStateInfo(0).IsName("HumanF@Run01_Forward"))
            {
                surface = Surface.prinsonRun;
            }
        }

        if (collision.gameObject.tag == "outside")
        {
            surface = Surface.outside;

            if (PlayerAni.GetCurrentAnimatorStateInfo(0).IsName("HumanF@Run01_Forward"))
            {
                surface = Surface.outsideRun;
            }

            BackGroundMusic.Instance.ChangeToInGameMusic();
        }

        selectStepList();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "prison")
        {
            surface = Surface.prisonjump;
        }

        if (collision.gameObject.tag == "outside")
        {
            surface = Surface.outsidejump;
        }

        selectStepList();
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        PlayerAni = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
