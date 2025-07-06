using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackGroundMusic : MonoBehaviour
{
    private static BackGroundMusic instance;

    public static BackGroundMusic Instance
    {
        get 
        { 
            return instance; 
        }
    }

    public List<AudioClip> BackGroundSounds = new List<AudioClip>();
    public AudioSource ScreenSound;

    public bool gameoversound = false;

    public void ChangeToInGameMusic()
    {
        ScreenSound.Stop();
        ScreenSound.clip = BackGroundSounds[1];
        ScreenSound.loop = true;
        ScreenSound.Play();
    }

    public void MonsterMusic()
    {
        ScreenSound.Stop();
        ScreenSound.clip = BackGroundSounds[3];
        ScreenSound.loop = true;
        ScreenSound.Play();
    }

    public void BossMusic()
    {
        ScreenSound.Stop();
        ScreenSound.clip = BackGroundSounds[4];
        ScreenSound.loop = true;
        ScreenSound.Play();
    }

    public void PlayGameStartBGM()
    {
        if (ScreenSound.clip != BackGroundSounds[0])
        {
            ScreenSound.clip = BackGroundSounds[0];
            ScreenSound.loop = true;
            ScreenSound.Play();
        }
    }

    void GameOverSound()
    {

        ScreenSound.Stop();
        ScreenSound.clip = BackGroundSounds[2];
        ScreenSound.loop = false;
        ScreenSound.Play();

    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayGameStartBGM();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerMove.Instance.hp <=0 && !gameoversound)
        {
            GameOverSound();
            gameoversound = true;
        }
    }
}
