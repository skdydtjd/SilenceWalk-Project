using System.Collections;
using UnityEngine;

public class Appearkey : MonoBehaviour
{
    private static Appearkey instance;

    public static Appearkey Instance
    {
        get { return instance; }
    }

    public GameObject key;

    public Camera Maincamera;
    public Camera keycamera;

    public float switchDuration = 3f;
    public float zoomInFOV = 25f;

    public bool coroutineTrigger;

    IEnumerator switchCameraForKey()
    {
        yield return StartCoroutine(GameManager.Instance.FadeOut());

        Maincamera.enabled = false;
        keycamera.enabled = true;

        yield return StartCoroutine(GameManager.Instance.FadeIn());

        yield return new WaitForSeconds(1f);

        key.SetActive(true);

        SFXMusic.Instance.Play("AppearKey");

        yield return StartCoroutine(zoomInCamera(keycamera,zoomInFOV,switchDuration));

        yield return StartCoroutine(GameManager.Instance.FadeOut());

        Maincamera.enabled = true;
        keycamera.enabled = false;

        yield return StartCoroutine(GameManager.Instance.FadeIn());
    }

    IEnumerator zoomInCamera(Camera cam, float targetFOV, float duration)
    {
        float time = 0;

        cam.transform.LookAt(key.transform.position);

        while (time < duration)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        cam.fieldOfView = targetFOV;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coroutineTrigger = false;

        if (CameraFollow.Instance.cam != null)
        {
            Maincamera = CameraFollow.Instance.cam;
        }

        key.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Room8.Instance.Checked && Room9.Instance.Checked && Room10.Instance.Checked && !coroutineTrigger)
        {
            StartCoroutine(switchCameraForKey());
            coroutineTrigger = true;
        }
    }
}
