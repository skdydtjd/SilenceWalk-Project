using UnityEngine;

public class StartKeyUI : MonoBehaviour
{
    public GameObject keypenal;

    public void showpenal()
    {
        keypenal.SetActive(true);

        Invoke("hidepenal", 10f);
    }

    public void hidepenal()
    {
        keypenal.SetActive(false);
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
