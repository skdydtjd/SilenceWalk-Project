using System.Collections;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    private static DoorOpen instance;

    public static DoorOpen Instance
    {
        get { return instance; }
    }

    public Camera Maincamera;
    public Camera Doorcamera;

    public float switchDuration = 3f;
    public float zoomInFOV = 40f;

    public bool coroutineTrigger;
    public static bool saved = false;

    public RectTransform Puzzle2KeyUI;

    IEnumerator switchCameraForDoor()
    {
        yield return StartCoroutine(GameManager.Instance.FadeOut());

        Puzzle2KeyUI.anchoredPosition = new Vector2(0, -300f);

        Maincamera.enabled = false;
        Doorcamera.enabled = true;

        yield return StartCoroutine(GameManager.Instance.FadeIn());

        yield return new WaitForSeconds(1f);

        SFXMusic.Instance.Play("PuzzelDoorOpen");

        yield return StartCoroutine(zoomInCamera(Doorcamera,zoomInFOV,switchDuration));

        yield return StartCoroutine(GameManager.Instance.FadeOut());

        Maincamera.enabled = true;
        Doorcamera.enabled = false;

        Puzzle2KeyUI.anchoredPosition = new Vector2(0, 0);

        yield return StartCoroutine(GameManager.Instance.FadeIn());
    }

    IEnumerator zoomInCamera(Camera cam, float targetFOV, float duration)
    {
        float time = 0;

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
            instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coroutineTrigger = false;

        if (CameraFollow.Instance.cam != null)
        {
            Maincamera = CameraFollow.Instance.cam;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Puzzle2Manager.SolvePipe && !coroutineTrigger && !saved)
        {
            StartCoroutine(switchCameraForDoor());
            coroutineTrigger = true;
        }
    }
}
