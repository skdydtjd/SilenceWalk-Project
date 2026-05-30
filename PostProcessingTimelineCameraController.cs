using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PostProcessingTimelineCameraController : MonoBehaviour
{
    [Header("Timeline")]
    public PlayableDirector playableDirector;

    [Header("Cameras")]
    public GameObject beforeCamera;
    public GameObject afterCamera;

    [Header("World Path")]
    public Vector3 secondPosition = new Vector3(-36.19f, 1.69f, 9.24f);
    public Vector3 secondRotation = Vector3.zero;
    public float xMoveDistance = -12f;
    public float zMoveDistance = 12f;

    [Header("Duration")]
    public float xMoveDuration = 3f;
    public float zMoveDuration = 3f;

    private Coroutine cameraMoveCoroutine;

    private void Reset()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    private void Awake()
    {
        if (playableDirector == null)
        {
            playableDirector = GetComponent<PlayableDirector>();
        }
    }

    private void OnEnable()
    {
        if (playableDirector != null)
        {
            playableDirector.played += OnTimelinePlayed;
        }
    }

    private void OnDisable()
    {
        if (playableDirector != null)
        {
            playableDirector.played -= OnTimelinePlayed;
        }
    }

    private void Start()
    {
        if (playableDirector != null && playableDirector.state == PlayState.Playing)
        {
            StartCameraSequence();
        }
    }

    private void OnTimelinePlayed(PlayableDirector director)
    {
        StartCameraSequence();
    }

    public void StartCameraSequence()
    {
        if (cameraMoveCoroutine != null)
        {
            StopCoroutine(cameraMoveCoroutine);
        }

        cameraMoveCoroutine = StartCoroutine(RunCameraSequence());
    }

    private IEnumerator RunCameraSequence()
    {
        if (beforeCamera == null || afterCamera == null)
        {
            Debug.LogWarning("PostProcessingTimelineCameraController needs both beforeCamera and afterCamera.", this);
            cameraMoveCoroutine = null;
            yield break;
        }

        beforeCamera.SetActive(true);
        afterCamera.SetActive(false);
        yield return MoveCamera(beforeCamera.transform);

        beforeCamera.SetActive(false);
        afterCamera.SetActive(true);
        yield return MoveCamera(afterCamera.transform);

        cameraMoveCoroutine = null;
    }

    private IEnumerator MoveCamera(Transform cameraTransform)
    {
        Vector3 startPosition = cameraTransform.position;
        Quaternion startRotation = cameraTransform.rotation;
        Vector3 xTargetPosition = startPosition + new Vector3(xMoveDistance, 0f, 0f);

        yield return MoveTransform(
            cameraTransform,
            startPosition,
            startRotation,
            xTargetPosition,
            startRotation,
            xMoveDuration);

        cameraTransform.SetPositionAndRotation(secondPosition, Quaternion.Euler(secondRotation));

        Vector3 zTargetPosition = secondPosition + new Vector3(0f, 0f, zMoveDistance);
        yield return MoveTransform(
            cameraTransform,
            secondPosition,
            Quaternion.Euler(secondRotation),
            zTargetPosition,
            Quaternion.Euler(secondRotation),
            zMoveDuration);
    }

    private IEnumerator MoveTransform(
        Transform target,
        Vector3 fromPosition,
        Quaternion fromRotation,
        Vector3 toPosition,
        Quaternion toRotation,
        float duration)
    {
        if (duration <= 0f)
        {
            target.SetPositionAndRotation(toPosition, toRotation);
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            target.SetPositionAndRotation(
                Vector3.Lerp(fromPosition, toPosition, smoothT),
                Quaternion.Slerp(fromRotation, toRotation, smoothT));

            yield return null;
        }

        target.SetPositionAndRotation(toPosition, toRotation);
    }
}
