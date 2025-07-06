using System.Collections;
using UnityEngine;

public class PushWithAnother : ImpuseBox
{
    public Rigidbody Another;

    protected override IEnumerator ResetKinematicAfterDelay()
    {
        yield return StartCoroutine(base.ResetKinematicAfterDelay());

        if (Another != null)
        {
            Another.linearVelocity = Vector3.zero;
            Another.isKinematic = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && nearbyPlayer && !isPushed)
        {
            Vector3 forceDir = PlayerMove.Instance.transform.forward;

            rb.isKinematic = false;
            PlayerMove.Instance.playermove.SetBool("Push", true);
            SFXMusic.Instance.Play("PushImpulse");

            rb.AddForce(forceDir * forcePower, ForceMode.Impulse);

            if (Another != null)
            {
                Another.isKinematic = false;
                Another.AddForce(forceDir * forcePower, ForceMode.Impulse);
            }

            isPushed = true;

            StartCoroutine(ResetKinematicAfterDelay());
        }
    }
}
