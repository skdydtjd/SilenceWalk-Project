using UnityEngine;
using UnityEngine.UI;

public class SlideOfMusic : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider VoiceSlider;
    public Slider SFXSlider;
    public Slider MouseSpeedSlider;


    private void Start()
    {
        // 기존 설정 불러오기
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        VoiceSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 1f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        MouseSpeedSlider.value = PlayerPrefs.GetFloat("MouseSpeedValue", 5f);

        // 슬라이더 값 변경 시 볼륨 조절
        bgmSlider.onValueChanged.AddListener(EditMusic.Instance.SetBGMVolume);
        VoiceSlider.onValueChanged.AddListener(EditMusic.Instance.SetVoiceVolume);
        SFXSlider.onValueChanged.AddListener(EditMusic.Instance.SetSFXVolume);

        // 마우스 감도 조절
        MouseSpeedSlider.onValueChanged.AddListener(EditMusic.Instance.SetMouseSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
