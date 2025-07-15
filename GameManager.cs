using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.InputSystem.HID.HID;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject StartMenu;
    public GameObject EscMenu;
    public GameObject GameOver;
    public GameObject EditMusic;

    public Camera Camera;

    public GameObject StartMove;

    public Image FadeImage;
    public float FadeDuration = 1.5f;

    public bool escSet = false;
    public bool gameoverSet = false;

    public AudioSource  Buttonsound;
    public AudioClip buttoncilp;

    public void ButtonSound()
    {
        Buttonsound.clip = buttoncilp;
        Buttonsound.Play();
    }

    public void StartButton()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if(StartMenu != null)
        {
            StartMenu.SetActive(false);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void OnEditMusicScreen()
    {
        Transform musicPenal = EditMusic.transform.Find("Panel");

        musicPenal.gameObject.SetActive(true);
    }

    public void OffEditMusicScreen()
    {
        Transform musicPenal = EditMusic.transform.Find("Panel");

        musicPenal.gameObject.SetActive(false);
    }

    public void BackToGame()
    {
        EscMenu.SetActive(false);
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        escSet = false;
    }

    IEnumerator ReStartScreen()
    {
        yield return StartCoroutine(FadeOut());

        Transform penal = GameOver.transform.Find("GameOverPanel");

        yield return new WaitForSeconds(1f);

        KeyRotate.gatheredByPlayer = false; // ����
        Puzzle2Manager.SolvePipe = false;
        Puzzle3UIforPlayer.NumberOfKey = 0;
        AppearKeyPuzzle4.KeyGatherCount = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // �� ���ε� �� �ٷ� �״� ���� ����
        PlayerMove.Instance.playermove.ResetTrigger("Death");

        PlayerMove.Instance.playermove.ResetTrigger("ReLoad");
        PlayerMove.Instance.playermove.ResetTrigger("GetUp");

        PlayerMove.Instance.ResetPlayerState();
        CameraFollow.Instance.ResetCameraState();

        CameraFollow.Instance.currentY = 180;

        yield return new WaitForSeconds(1f);

        penal.gameObject.SetActive(false);

        StartMenu.SetActive(true);

        PlayerVoice.Instance.die = false;
        gameoverSet = false;

        BackGroundMusic.Instance.gameoversound = false;
        BackGroundMusic.Instance.PlayGameStartBGM();

        yield return StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        float alpha = 1f;

        while (alpha > 0)
        {
            alpha = alpha - Time.deltaTime / FadeDuration;

            FadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        FadeImage.gameObject.SetActive(false);
    }

    public IEnumerator FadeOut()
    {
        FadeImage.gameObject.SetActive(true);

        float alpha = 0f;

        while (alpha < 1)
        {
            alpha = alpha + Time.deltaTime / FadeDuration;

            FadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    public void ReStart()
    {
        StartCoroutine(ReStartScreen());
    }

    public void GetUp()
    {
        if (StartMove != null)
        {
            StartMove.GetComponent<PlayerMove>().enabled = true;
        }

        PlayerMove.Instance.playermove.SetTrigger("GetUp");
    }

    public void AfterDeath()
    {
        StartMove.GetComponent<PlayerMove>().enabled = false;

        gameoverSet = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Transform penal = GameOver.transform.Find("GameOverPanel");

        penal.gameObject.SetActive(true);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(StartMenu);
            DontDestroyOnLoad(EscMenu);
            DontDestroyOnLoad(GameOver);
            DontDestroyOnLoad(EditMusic);
        }
        else
        {
            Destroy(gameObject);
            Destroy(StartMenu);
            Destroy(EscMenu);
            Destroy(GameOver);
            Destroy(EditMusic);
        }

        EscMenu.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartMove.GetComponent<PlayerMove>().enabled = false;
        Time.timeScale = 1.0f;

        StartCoroutine(FadeIn());
    }

    // Update is called once per frame  
    void Update()   
    {
        Transform penal = GameOver.transform.Find("GameOverPanel");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escSet = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (!StartMenu.activeSelf && !penal.gameObject.activeSelf)
            {
                EscMenu.SetActive(true);
                Time.timeScale = 0;

                // ���콺 Ŀ���� �ø��� �͸����� ���õǴ� �� ����
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        if (PlayerMove.Instance.hp <= 0 && !gameoverSet)
        {
            PlayerMove.Instance.playermove.SetTrigger("Death");
            PlayerMove.Instance.rigid.isKinematic = true;

            Invoke("AfterDeath", 0.7f);
        }
    }
}
