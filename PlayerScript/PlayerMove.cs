using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private static PlayerMove instance;

    public static PlayerMove Instance 
    {  
        get 
        {
            return instance;
        } 
    }

    public Animator playermove;
    public Rigidbody rigid;

    public float WalkSpeed = 3f;
    public float RunSpeed = 4.5f;
    public float jumpForce = 5f;

    float jumpBufferTime = 0.3f;
    float jumpBufferCounter = 0f;

    float gatherBufferTime = 1f;
    float gatherBufferCounter = 0f;

    public float hp = 100;

    bool Runtrigger;
    bool jumpTrigger = false;
    bool gatherTrigger = false;
    bool hitTrigger = false;

    public float hAix;
    public float vAix;

    public Vector3 moveVec;

    Vector3 spawnPosition;
    Quaternion spawnRotation;

    public float wallcheckDistance = 1.5f;
    public LayerMask mask;

    public bool useFixedDirection = false;

    public void ResetPlayerState()
    {
        WalkSpeed = 2.8f;
        RunSpeed = 4f;

        hp = 100;

        rigid.position = spawnPosition;
        rigid.rotation = spawnRotation;

        rigid.isKinematic = false;
        playermove.SetTrigger("ReLoad");
    }

    void ResetSpeed()
    {
        WalkSpeed = 2.8f;
        RunSpeed = 4f;
    }

    void Jump()
    {
        jumpBufferCounter = jumpBufferTime;

        AnimatorStateInfo stateInfo = playermove.GetCurrentAnimatorStateInfo(0);

        if (jumpBufferCounter > 0 && jumpTrigger)
        {
            if (!stateInfo.IsName("human_male_gathering_01") && !stateInfo.IsName("Push"))
            {
                jumpTrigger = false;
                rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                playermove.SetTrigger("Jump");
                jumpBufferCounter = 0;
            }
        }

        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    public bool NearWall()
    {
        return Physics.Raycast(rigid.position, transform.forward, wallcheckDistance,mask);
    }

    IEnumerator Respawn()
    {
        yield return StartCoroutine(GameManager.Instance.FadeOut());

        yield return new WaitForFixedUpdate();

        rigid.linearVelocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        rigid.position = spawnPosition;

        yield return new WaitForEndOfFrame();

        yield return new WaitForFixedUpdate();

        yield return StartCoroutine(GameManager.Instance.FadeIn());
    }

    // �浹 �Լ�
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "prison" || collision.gameObject.tag == "outside")
        {
            Debug.Log("done");
            jumpTrigger = true;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("enemy");
            hp = hp - 30;

            if(hp < 0) 
                hp = 0;

            hitTrigger = true;
            RunSpeed = WalkSpeed = 0.5f;
            Invoke("ResetSpeed", 1.5f);
        }

        if(collision.gameObject.tag == "Boss")
        {
            hp = 0;
        }
    }

    private void Awake()
    {
        playermove = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        if (instance == null)
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
        hp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        // hp����
        if (0 < hp && hp < 100)
        {
            hp = hp + (2*Time.deltaTime);

            if (100 < hp)
            {
                hp = 100;
            }
        }

        if(GameManager.Instance.escSet)
        {
            return;
        }

        // �޸���
        Runtrigger = Input.GetKey(KeyCode.LeftShift);

        // �ȱ� �ִϸ��̼�
        playermove.SetBool("Walk", moveVec != Vector3.zero);

        // �޸��� �ִϸ��̼�
        playermove.SetBool("Run", Runtrigger);

        //ĳ���� ȸ��
        if (moveVec != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveVec.normalized);
            rigid.MoveRotation(targetRotation); // ��� ȸ��
        }

        AnimatorStateInfo stateInfo = playermove.GetCurrentAnimatorStateInfo(0);

        // ������ �ݱ�
        if (Input.GetKeyDown(KeyCode.Q) && !stateInfo.IsName("HumanF@Gather")&& !playermove.IsInTransition(0))
        {
            gatherBufferCounter = gatherBufferTime;
            gatherTrigger = true;
        }

        if (stateInfo.IsName("HumanF@Gather"))
        {
            gatherTrigger = false;
        }

        if (gatherBufferCounter > 0 && gatherTrigger)
        {
            if (!stateInfo.IsName("HumanF@Jump01 [RM]") && !stateInfo.IsName("HumanF@Run01_Forward"))
            {
                playermove.SetTrigger("Gather");
                gatherBufferCounter = 0;
            }
        }

        if (gatherBufferCounter > 0)
        {
            gatherBufferCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (hitTrigger)
        {
            playermove.SetTrigger("Hit");
            hitTrigger = false;
        }

        // �ȴ� �ӵ� ����
        float speed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;

        // �̵�
        hAix = Input.GetAxisRaw("Horizontal");
        vAix = Input.GetAxisRaw("Vertical");

        if (useFixedDirection)
        {
            moveVec = (-Vector3.forward * vAix) + (-Vector3.right * hAix).normalized;
        }
        else
        {
            Vector3 camForward = CameraFollow.Instance.cam.transform.forward;
            Vector3 camRight = CameraFollow.Instance.cam.transform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            moveVec = (-camForward * vAix + -camRight * hAix).normalized;
        }

        AnimatorStateInfo stateInfo = playermove.GetCurrentAnimatorStateInfo(0);

        // ������ �ݴ� ���� �� �����̰�
        if (!stateInfo.IsName("human_male_gathering_01"))
        {
            rigid.MovePosition(rigid.position + ((moveVec * speed) * Time.deltaTime));

            if(rigid.position.y <= -15)
            {
                StartCoroutine(Respawn());
            }
        }

        // ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
}
