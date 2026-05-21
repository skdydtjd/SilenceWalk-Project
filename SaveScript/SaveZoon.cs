using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine.Analytics;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveZoon : MonoBehaviour
{
    public Button[] loadButtons;

    string savePath;

    public static SaveZoon instance;

    [System.Serializable]
    public class GameSaveData
    {
        public Vector3 playerPosition;

        public bool puzzle1Cleared;
        public bool puzzle2Cleared;

        public int puzzle3KeyCount;
        public int puzzle4KeyCount;

        public bool savedPuzzle2;
    }

    public void SaveGame()
    {
        GameSaveData data = new GameSaveData
        {
            playerPosition = PlayerMove.Instance.transform.position,

            puzzle1Cleared = KeyRotate.gatheredByPlayer,
            puzzle2Cleared = Puzzle2Manager.SolvePipe,
            puzzle3KeyCount = Puzzle3UIforPlayer.NumberOfKey,
            puzzle4KeyCount = AppearKeyPuzzle4.KeyGatherCount,

            // 스테이지2 퍼즐을 해결한 후 문이 열리는 카메라 연출을 1회성으로 번복되지않게 하기 위함
            savedPuzzle2 = DoorOpen.saved
        };

        string json = JsonUtility.ToJson(data,true);
        File.WriteAllText(savePath, json);

        Debug.Log("저장 완료");

        SetLoadButtonState(true);
    }

    public void LoadGameFunction()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("저장된 데이터가 없습니다.");
            return;
        }

        string json = File.ReadAllText(savePath);
        GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

        PlayerMove.Instance.transform.position = data.playerPosition;

        KeyRotate.gatheredByPlayer = data.puzzle1Cleared;
        Puzzle2Manager.SolvePipe = data.puzzle2Cleared;
        Puzzle3UIforPlayer.NumberOfKey = data.puzzle3KeyCount;
        AppearKeyPuzzle4.KeyGatherCount = data.puzzle4KeyCount;

        DoorOpen.saved = data.savedPuzzle2;

        Debug.Log("불러오기 완료");
    }

    IEnumerator Load()
    {
        yield return StartCoroutine(GameManager.Instance.FadeOut());

        // 씬이 완전히 로드 된 후 불러오기
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        yield return new WaitUntil(() => async.isDone);

        LoadGameFunction();

        yield return new WaitForSeconds(1f);

        GameManager.Instance.StartMenu.SetActive(false);
        GameManager.Instance.EscMenu.SetActive(false);

        yield return new WaitForSeconds(1f);

        PlayerMove.Instance.hp = 100;
        PlayerMove.Instance.rigid.isKinematic = false;

        yield return new WaitForSeconds(0.3f);

        PlayerVoice.Instance.die = false;
        GameManager.Instance.gameoverSet = false;
        GameManager.Instance.escSet = false;
        BackGroundMusic.Instance.gameoversound = false;

        PlayerMove.Instance.playermove.ResetTrigger("Death");

        yield return new WaitForSeconds(1f);

        PlayerMove.Instance.playermove.SetTrigger("ReLoad");

        GameManager.Instance.GetUp();

        Transform penal = GameManager.Instance.GameOver.transform.Find("GameOverPanel");
        penal.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        var Stage3door = FindAnyObjectByType<Puzzle3GateDoor>();
        var Stage4door = FindAnyObjectByType<Puzzle4GateDoor>();

        // 클리어 여부 확인
        Stage3door.Refresh();
        Stage4door.Refresh();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        yield return StartCoroutine(GameManager.Instance.FadeIn());
    }

    public void LoadGame()
    {
        StartCoroutine(Load());
    }

    void SetLoadButtonState(bool state)
    {
        foreach (Button btn in loadButtons)
        {
            if (btn != null)
            {
                btn.interactable = state;
                var colors = btn.colors;

                colors.normalColor = new Color(1f, 1f, 1f, state ? 1f : 0.3f);

                btn.colors = colors;
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        savePath = Application.persistentDataPath + "/save.json";

        SetLoadButtonState(File.Exists(savePath));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
