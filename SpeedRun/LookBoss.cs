using UnityEngine;

public class LookBoss : MonoBehaviour
{
    public Transform boss;
    public float rotationSpeed = 3f;

    Vector3 lastBossPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (boss != null)
        {
            lastBossPos = boss.position;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (boss == null)
        {
            return;
        }

        Vector3 moveDir = boss.position - lastBossPos;

        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(moveDir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        lastBossPos = boss.position;
    }
}
