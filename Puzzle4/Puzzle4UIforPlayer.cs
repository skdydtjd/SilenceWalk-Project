using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Puzzle4UIforPlayer : MonoBehaviour
{
    public static Puzzle4UIforPlayer Instance;

    public TextMeshProUGUI keycount;
    public Image keyImage;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene newscene, LoadSceneMode mode)
    {
        keycount = GameObject.Find("KeyCountP4")?.GetComponent<TextMeshProUGUI>();
        keyImage = GameObject.Find("KeyImageP4")?.GetComponent<Image>();

        keycount.alpha = 0f;

        Color imageColor = keyImage.color;
        imageColor.a = 0f;
        keyImage.color = imageColor;

        AppearKeyPuzzle4.KeyGatherCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Puzzle4")
        {
            keycount.alpha = 1f;

            Color imageColor = keyImage.color;
            imageColor.a = 1f;
            keyImage.color = imageColor;
        }

        if (other.gameObject.tag == "Puzzle3" || other.gameObject.tag == "BossRace")
        {
            keycount.alpha = 0f;

            Color imageColor = keyImage.color;
            imageColor.a = 0f;
            keyImage.color = imageColor;
        }
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

    }

    // Update is called once per frame
    void Update()
    {
        keycount.text = $"{AppearKeyPuzzle4.KeyGatherCount} / 2";
    }
}
