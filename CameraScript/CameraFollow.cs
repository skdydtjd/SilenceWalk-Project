using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private static CameraFollow instance;

    public static CameraFollow Instance
    {
        get
        {
            return instance;
        }
    }

    public Transform target;

    public Vector3 startposition;
    public Vector3 targetPosition;

    public Quaternion startRotation;
    public Quaternion targetRotation;

    public float movespeed = 2f;
    public float rotatespeed = 2f;

    public bool isFollowing = false;

    public Camera cam;

    public float normalFOV = 60;
    public float nearWallFOV = 40;

    public float changeFOVspeed = 5f;

    // 마우스 감도
    public float mouseSensitivity = 5f;

    public float currentY = 180;

    public void ResetCameraState()
    {
        transform.position = startposition;
        transform.rotation = startRotation;
        isFollowing = false;
    }

    public void StartCameraMove()
    {
        StartCoroutine(MoveToPosition());
    }

    public IEnumerator MoveToPosition()
    {
        float time = 0;
        Vector3 initialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;

        while(time <1)
        {
            time += Time.deltaTime*movespeed;

            transform.position = Vector3.Lerp(initialPosition, (target.position + targetPosition), time);
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation,time);

            yield return null;
        }
        transform.position = target.position + targetPosition;

        isFollowing = true;
    }


    private void Awake()
    {
        isFollowing=false;

        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startposition = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerMove.Instance.NearWall())
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,nearWallFOV, Time.deltaTime*changeFOVspeed);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFOV, Time.deltaTime * changeFOVspeed);
        }

        if (isFollowing && target != null)
        { 

            if (!GameManager.Instance.escSet && !GameManager.Instance.gameoverSet)
            {
                float mouseX = Input.GetAxis("Mouse X");
                currentY += mouseX * mouseSensitivity;

                currentY %= 360f;
            }

            Vector3 offset = Quaternion.Euler(0, currentY, 0) * new Vector3(0f,2f,-2.5f);

            transform.position = target.position + offset;

            transform.rotation = Quaternion.Euler(15f, currentY, 0);
        }
    }
}
