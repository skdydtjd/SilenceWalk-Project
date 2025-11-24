using UnityEngine;

public class KeyRotate : MonoBehaviour
{
    public float spinDuration = 20f;

    [SerializeField]
    protected bool trigger = false;
    public static bool gatheredByPlayer = false; // 수정

    protected virtual void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            trigger = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trigger = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Vector3 eulerAngle = transform.eulerAngles;
        eulerAngle.y += Time.deltaTime*spinDuration;

        transform.rotation = Quaternion.Euler(eulerAngle);

        if (trigger && Input.GetKeyDown(KeyCode.Q))
        {
            gatheredByPlayer = true;
            Destroy(gameObject);
        }
    }
}
