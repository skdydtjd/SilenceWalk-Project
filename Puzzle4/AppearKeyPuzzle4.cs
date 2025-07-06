using UnityEngine;

public class AppearKeyPuzzle4 : MonoBehaviour
{
    public static AppearKeyPuzzle4 Instance;

    public GameObject puzzle4Key;

    public static int KeyGatherCount = 0;

    bool keyappear = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "WoodBox")
        {
            puzzle4Key.SetActive(true);

            if (!keyappear)
            {
                SFXMusic.Instance.Play("AppearKey");
                keyappear = true;
            }
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puzzle4Key.SetActive(false);
        KeyGatherCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
