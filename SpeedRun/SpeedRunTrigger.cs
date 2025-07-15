using System.Collections;
using UnityEngine;

public class SpeedRunTrigger : MonoBehaviour
{
    public Camera  mainCamera;
    public Camera LookBoss;
    public Transform bossPoisition;

    public bool cameraTriggger = false;
    public bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            cameraTriggger = true;
        }
    }

    IEnumerator SwitchtoBossCamera()
    {
        yield return StartCoroutine(GameManager.Instance.FadeOut());

        mainCamera.enabled = false;
        LookBoss.enabled = true;

        var lookScript = LookBoss.GetComponent<LookBoss>();

        if (lookScript != null)
        {
            lookScript.enabled = true;
        }

        yield return StartCoroutine(GameManager.Instance.FadeIn());

        MiddleBoss.Instance.StartChase();


        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(GameManager.Instance.FadeOut());

        LookBoss.enabled = false;
        mainCamera.enabled = true;

        if (lookScript != null)
        {
            lookScript.enabled = false;
        }

        yield return StartCoroutine(GameManager.Instance.FadeIn());

        BackGroundMusic.Instance.BossMusic();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraTriggger = false;

        if (CameraFollow.Instance.cam != null)
        {
            mainCamera = CameraFollow.Instance.cam;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraTriggger && !hasTriggered)
        {
            StartCoroutine(SwitchtoBossCamera());
            hasTriggered = true;
        }
    }
}
