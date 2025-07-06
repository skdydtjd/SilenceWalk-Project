using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class MiddleBoss : MonoBehaviour
{
    public static MiddleBoss Instance;

    public Rigidbody rb;
    public Collider BossCollider;
    public Animator BossAnim;

    public AudioSource bossvoice;
    public List<AudioClip> bossSounds = new List<AudioClip>();

    public Transform Player;
    public float movespeed = 3.5f;
    public float pushForce = 5f;

    [SerializeField]
    bool isChase = false;
    bool isdead = false;

    bool attackbegin = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "fence")
        {
            StopAllCoroutines();
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = false;
            BossCollider.enabled = false;

            bossvoice.clip = bossSounds[1];
            bossvoice.PlayOneShot(bossvoice.clip);

            BossAnim.SetTrigger("Death");
            isdead = true;

            BackGroundMusic.Instance.ChangeToInGameMusic();

            return;
        }
        else if(collision.gameObject.tag == "Player")
        {
            rb.isKinematic = true;
            return;
        }

        if (!isChase || isdead) 
            return;

        Rigidbody otherRb = collision.rigidbody;
        if (otherRb != null && !collision.collider.isTrigger)
        {
            // 만약 Kinematic이면 잠시 꺼줌
            if (otherRb.isKinematic)
            {
                otherRb.isKinematic = false;
            }

            SFXMusic.Instance.Play("ThrowObjects");
            Vector3 pushDir = (collision.transform.position - transform.position).normalized;

            float pushUpForce = 0.5f;

            // y축 힘 추가
            Vector3 pushForceVector = new Vector3(pushDir.x, pushUpForce, pushDir.z).normalized * pushForce;

            otherRb.AddForce(pushForceVector, ForceMode.Impulse);

            Destroy(collision.gameObject, 1.5f); 
        }
    }

    public void RunSound()
    {
        bossvoice.clip = bossSounds[0];
        bossvoice.PlayOneShot(bossvoice.clip);
    }

    public void StartChase()
    {
        if(!isdead)
        {
            isChase = true;
        }
        BossAnim.SetTrigger("Chase");
    }

    IEnumerator ChooseAttack()
    {
        yield return new WaitForSeconds(3f);

        int RandomNumber = Random.Range(0, 8);

        switch (RandomNumber)
        {
            case 0:
            case 1:
                yield return StartCoroutine(Attack1());
                break;
            case 2:
            case 3:
                yield return StartCoroutine(Attack2());
                break;
            case 4:
            case 5:
                yield return StartCoroutine(Attack3());
                break;
            case 6:
            case 7:
                yield return StartCoroutine(Attack4());
                break;
        }
    }

    IEnumerator Attack1()
    {
        BossAnim.SetBool("Attack1", true);

        bossvoice.clip = bossSounds[2];
        bossvoice.PlayOneShot(bossvoice.clip);

        yield return new WaitForSeconds(1.3f);

        BossAnim.SetBool("Attack1",false);

        yield return StartCoroutine(ChooseAttack());
    }

    IEnumerator Attack2()
    {
        BossAnim.SetBool("Attack2", true);

        bossvoice.clip = bossSounds[4];
        bossvoice.PlayOneShot(bossvoice.clip);

        yield return new WaitForSeconds(1f);

        BossAnim.SetBool("Attack2", false);

        yield return StartCoroutine(ChooseAttack());
    }

    IEnumerator Attack3()
    {
        BossAnim.SetBool("Attack3", true);

        bossvoice.clip = bossSounds[5];
        bossvoice.PlayOneShot(bossvoice.clip);

        yield return new WaitForSeconds(1f);

        BossAnim.SetBool("Attack3", false);

        yield return StartCoroutine(ChooseAttack());
    }

    IEnumerator Attack4()
    {
       BossAnim.SetBool("Attack4", true);

        bossvoice.clip = bossSounds[3];
        bossvoice.PlayOneShot(bossvoice.clip);

        yield return new WaitForSeconds(0.6f);

        BossAnim.SetBool("Attack4", false);

        yield return StartCoroutine(ChooseAttack());
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerMove.Instance != null)
        {
            Player = PlayerMove.Instance.transform;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isChase && Player != null && !isdead)
        {
            if (!attackbegin)
            {
                StartCoroutine(ChooseAttack());
                attackbegin = true;
            }

            Vector3 direction = (Player.position - transform.position);
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Vector3 moveDir = direction.normalized;

                // 보스 이동
                rb.MovePosition(transform.position + moveDir * movespeed * Time.fixedDeltaTime);

                // 보스 회전
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
            }
        }
    }
}
