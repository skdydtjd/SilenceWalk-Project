using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera maincamera;

    public Camera[] Puzzlecamera;

    bool isSwitching = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !isSwitching)
        {
            PlayerMove.Instance.useFixedDirection = true;
            StartCoroutine(swictchCamera());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isSwitching)
        {
            PlayerMove.Instance.useFixedDirection = false;
            StartCoroutine(ReturnMainCamera());
        }
    }

    IEnumerator swictchCamera()
    {
        isSwitching = true;
        yield return StartCoroutine(GameManager.Instance.FadeOut());

        maincamera.enabled = false;
        foreach (var camera in Puzzlecamera)
        {
            if (camera != null)
            {
                camera.enabled = true;
            }
        }

        yield return StartCoroutine(GameManager.Instance.FadeIn());
        isSwitching = false;
    }

    IEnumerator ReturnMainCamera()
    {
        isSwitching = true;
        yield return StartCoroutine(GameManager.Instance.FadeOut());

        maincamera.enabled = true;
        foreach (var camera in Puzzlecamera)
        {
            if (camera != null)
            {
                camera.enabled = false;
            }
        }

        yield return StartCoroutine(GameManager.Instance.FadeIn());
        isSwitching = false;
    }

    private void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (CameraFollow.Instance.cam != null)
        {
            maincamera = CameraFollow.Instance.cam;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
