using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;

public class SFXMusic : MonoBehaviour
{
    public static SFXMusic Instance;

    public AudioSource SFXSource;

    [Serializable]
    public class sound
    {
        public string name;
        public List<AudioClip> SFXs;
    }

    public List<sound> sounds = new List<sound>();
    Dictionary<string, List<AudioClip>> soundDict;

    public void Play(string soundName)
    {
        if (soundDict.ContainsKey(soundName))
        {
            List<AudioClip> clips = soundDict[soundName];

            if (clips.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, clips.Count);
                SFXSource.PlayOneShot(clips[randomIndex]);
            }
        }
        else
        {
            Debug.LogWarning($"���� '{soundName}'�� ã�� �� �����ϴ�!");
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        soundDict = new Dictionary<string, List<AudioClip>>();
        foreach (var s in sounds)
        {
            soundDict[s.name] = s.SFXs;
        }

        if (SFXSource == null)
            SFXSource = GetComponent<AudioSource>();
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
