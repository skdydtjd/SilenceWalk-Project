using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EditMusic : MonoBehaviour
{
    public static EditMusic Instance;


    public AudioSource bgmSource;
    public AudioSource VoiceSource;
    public AudioSource SFXSource;

    float bgmvolume = 1f;
    float voicevolume = 1f;
    float SFXvolume = 1f;
    float MouseSpeedValue = 5f;

    public void SetBGMVolume(float volume)
    {
        bgmvolume = volume;
        bgmSource.volume = bgmvolume;
        PlayerPrefs.SetFloat("BGMVolume", bgmvolume);
    }

    public void SetVoiceVolume(float volume)
    {
        voicevolume = volume;
        VoiceSource.volume = voicevolume;
        PlayerPrefs.SetFloat("VoiceVolume", voicevolume);
    }

    public void SetSFXVolume(float volume)
    {
        SFXvolume = volume;
        SFXSource.volume = SFXvolume;
        PlayerPrefs.SetFloat("SFXVolume",SFXvolume);
    }

    public void SetMouseSpeed(float speed)
    {
        MouseSpeedValue = speed;
        CameraFollow.Instance.mouseSensitivity = MouseSpeedValue;
        PlayerPrefs.SetFloat("MouseSpeedValue", MouseSpeedValue);
    }

    private void LoadVolumeSettings()
    {
        bgmvolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        voicevolume = PlayerPrefs.GetFloat("VoiceVolume", 1f);
        SFXvolume = PlayerPrefs.GetFloat("SFXVolume",1f);
        MouseSpeedValue = PlayerPrefs.GetFloat("MouseSpeedValue", 5f);

        bgmSource.volume = bgmvolume;
        VoiceSource.volume = voicevolume;
        SFXSource.volume= SFXvolume;
        CameraFollow.Instance.mouseSensitivity= MouseSpeedValue;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadVolumeSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
