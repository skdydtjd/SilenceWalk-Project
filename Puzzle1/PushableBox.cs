using System.Collections.Generic;
using UnityEngine;

public class PushableBox : MonoBehaviour
{
    public Rigidbody rb;

    [SerializeField]
    bool sendObject = false;

    [SerializeField]
    bool move = false;

    [SerializeField]
    bool phycise = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sendObject = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sendObject = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sendObject && Input.GetKeyDown(KeyCode.F))
        {
            move = !move;
            phycise = !phycise;

            SFXMusic.Instance.Play("Grab");

            if (rb != null)
            {
                rb.isKinematic = phycise;
            }

            if (PlayerMove.Instance != null)
            {
                PlayerMove.Instance.playermove.SetBool("Push", move);
                PlayerMove.Instance.WalkSpeed = move ? 1.5f : 2.8f;
                PlayerMove.Instance.RunSpeed = move ? 1.5f : 4f;
            }
        }

        // 박스 이동 처리
        if (move && PlayerMove.Instance.moveVec != Vector3.zero)
        {
            Vector3 movebox = PlayerMove.Instance.transform.position + PlayerMove.Instance.transform.forward * 1.2f;
            movebox.y = this.gameObject.transform.position.y;

            transform.position = Vector3.Lerp(transform.position, movebox, Time.deltaTime * 2f);
            transform.rotation = Quaternion.Slerp(transform.rotation, PlayerMove.Instance.transform.rotation, Time.deltaTime * 2f);

            float facingAngle = Vector3.Angle(PlayerMove.Instance.transform.forward, transform.position - PlayerMove.Instance.transform.position);

            if (facingAngle > 120f)
            {
                move = false;
                phycise= true;
                rb.isKinematic = phycise;
                PlayerMove.Instance.playermove.SetBool("Push", false);

                PlayerMove.Instance.WalkSpeed = 2.8f;
                PlayerMove.Instance.RunSpeed = 4f;
            }

        }

        if (move && Vector3.Distance(transform.position, PlayerMove.Instance.transform.position) > 1.5f)
        {
            move = false;
            rb.isKinematic = true;
            PlayerMove.Instance.playermove.SetBool("Push", false);
            PlayerMove.Instance.WalkSpeed = 2.8f;
            PlayerMove.Instance.RunSpeed = 4f;
        }
    }
}
