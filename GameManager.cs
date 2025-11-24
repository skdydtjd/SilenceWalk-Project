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

    // 게임 시작 버튼
    public void StartButton()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if(StartMenu != null)
        {
            StartMenu.SetActive(false);
        }
    }

    // 게임 종료 버튼
    public void ExitButton()
    {
        Application.Quit();
    }

    // 설정 버튼
    public void OnEditMusicScreen()
    {
        Transform musicPenal = EditMusic.transform.Find("Panel");

        musicPenal.gameObject.SetActive(true);
    }

    // 설정 창 닫기 버튼
    public void OffEditMusicScreen()
    {
        Transform musicPenal = EditMusic.transform.Find("Panel");

        musicPenal.gameObject.SetActive(false);
    }

    // 게임으로 돌아가기 버튼
    public void BackToGame()
    {
        EscMenu.SetActive(false);
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        escSet = false;
    }

    // 처음부터 재시작 버튼 (씬 초기화)
    IEnumerator ReStartScreen()
    {
        yield return StartCoroutine(FadeOut());

        Transform penal = GameOver.transform.Find("GameOverPanel");

        yield return new WaitForSeconds(1f);

        KeyRotate.gatheredByPlayer = false; // 수정
        Puzzle2Manager.SolvePipe = false;
        Puzzle3UIforPlayer.NumberOfKey = 0;
        AppearKeyPuzzle4.KeyGatherCount = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // 씬 리로드 후 바로 죽는 현상 방지
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

    // (게임 화면 -> 검은 화면)
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

    // (검은 화면 -> 게임       EscMenu.SetActive(false);
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        escSet = false;
    }

    // 처음부터 재시작 버튼, 세부 내용 함수 (씬 초기화)
    IEnumerator ReStartScreen()
    {
        yield return StartCoroutine(FadeOut());

        Transform penal = GameOver.transform.Find("GameOverPanel");

        yield return new WaitForSeconds(1f);

        KeyRotate.gatheredByPlayer = false; // 수정
        Puzzle2Manager.SolvePipe = false;
        Puzzle3UIforPlayer.NumberOfKey = 0;
        AppearKeyPuzzle4.KeyGatherCount = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // 씬 리로드 후 바로 죽는 현상 방지
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

    // (검은 화면 -> 게임 화면)
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

    // (게임 화면 -> 검은 화면)
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

    // 처음부터 재시작 버튼
    public void ReStart()
    {
        StartCoroutine(ReStartScreen());
    }

    // 게임 시작 시 플레이어 조작이 가능하게 변경하는 함수
    public void GetUp()
    {
        if (StartMove != null)
        {
            StartMove.GetComponent<PlayerMove>().enabled = true;
        }

        PlayerMove.Instance.playermove.SetTrigger("GetUp");
    }

    // 게임 오버 화면, 플레이어가 사망 후 조작을 못하게 잠그는 함수
    public void AfterDeath()
    {
        StartMove.GetComponent<PlayerMove>().enabled = false;

        gameoverSet = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Transform penal = GameOver.transform.Find("GameOverPanel");

        penal.gameObject.SetActive(true);
    }

    // 씬이 리로드되도 남아있어야하는 오브젝트들 관리
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

    // 게임 시작 시
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartMove.GetComponent<PlayerMove>().enabled = false;
        Time.timeScale = 1.0f;

        StartCoroutine(FadeIn());
    }

    // 일시정지 창, 플레이어 사망 시 게임오버 창 불러오기
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

                // 마우스 커서를 올리는 것만으로 선택되는 것 방지
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
