using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    public GameObject Telephone;
    public GameObject SavePenal;

    bool saveTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            saveTrigger = true;
            SavePenal.SetActive(true);

            if (Puzzle2Manager.SolvePipe)
            {
                DoorOpen.saved = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            saveTrigger = false;
            SavePenal.SetActive(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(saveTrigger && Input.GetKeyUp(KeyCode.E))
        {
            SaveZoon.instance.SaveGame();
            SFXMusic.Instance.Play("Save");

            if (Telephone != null)
            {
                Telephone.GetComponent<Light>().enabled = true;
            }
        }
    }
}
