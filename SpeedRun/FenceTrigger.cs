using UnityEngine;

public class FenceTrigger : MonoBehaviour
{
    public GameObject fence;

    [SerializeField]
    bool nearbyPlayer = false;

    [SerializeField]
    bool MoveFence = false;
    bool fenceonPlayed = false;

    public Transform fenceOn;
    public GameObject fenceOnPenal;

    [SerializeField]
    float moveOnSpeed = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            nearbyPlayer = true;
            fenceOnPenal.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            nearbyPlayer = false;
            fenceOnPenal.SetActive(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveFence = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearbyPlayer)
        {
            MoveFence = true;
        }

        if(MoveFence)
        {
            Vector3 currentPos = fence.transform.position;
            Vector3 targetPos = new Vector3(currentPos.x, fenceOn.position.y, currentPos.z); // y만 이동

            if (!fenceonPlayed)
            {
                SFXMusic.Instance.Play("FenceOn");
                fenceonPlayed = true;
            }

            currentPos = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * moveOnSpeed);
            fence.transform.position = currentPos;

            // 목표 위치에 도달하면 정지
            if (Mathf.Abs(currentPos.y - fenceOn.position.y) < 0.01f)
            {
                fence.transform.position = new Vector3(currentPos.x, fenceOn.position.y, currentPos.z);
                MoveFence = false;
            }
        }
    }
}
