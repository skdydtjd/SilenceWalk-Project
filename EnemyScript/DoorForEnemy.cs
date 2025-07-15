using System.Security.Cryptography;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class DoorForEnemy : AE_Door
{
    public NavMeshObstacle obstacle;
    public AudioSource enemyopen; 

    bool enemyopenSound = false;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.CompareTag("Enemy"))
        {
            open = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        if (obstacle != null)
        {
            obstacle.enabled = !open; // 초기 설정
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (open)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotQ, Time.deltaTime * smooth);

            if (!enemyopenSound)
            {
                enemyopen.PlayOneShot(enemyopen.clip);
                enemyopenSound = true;
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, defaultRotQ, Time.deltaTime * smooth);
            enemyopenSound = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && trig)
        {
            open = !open;
        }

        if (trig)
        {
            if (open)
            {
                txt.text = "Close E";
            }
            else
            {
                txt.text = "Open E";
            }
        }

        if (obstacle != null)
        {
            obstacle.enabled = !open;
        }
    }
}
