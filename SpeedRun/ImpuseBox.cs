using System.Collections;
using UnityEngine;

public class ImpuseBox : MonoBehaviour
{
    public Rigidbody rb;

    [SerializeField]
    protected float forcePower = 10f;

    [SerializeField]
    protected bool nearbyPlayer = false;

    [SerializeField]
    protected bool isPushed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            nearbyPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            nearbyPlayer = false;
        }
    }

    protected virtual IEnumerator ResetKinematicAfterDelay()
    {
        yield return new WaitForSeconds(0.7f);

        rb.isKinematic = true;

        PlayerMove.Instance.playermove.SetBool("Push", false);

        isPushed = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && nearbyPlayer && !isPushed)
        {
            Vector3 forceDir = PlayerMove.Instance.transform.forward;

            rb.isKinematic = false;

            PlayerMove.Instance.playermove.SetBool("Push", true);

            SFXMusic.Instance.Play("PushImpulse");
            rb.AddForce(forceDir * forcePower, ForceMode.Impulse);

            isPushed = true;

            StartCoroutine(ResetKinematicAfterDelay());
        }
    }
}
