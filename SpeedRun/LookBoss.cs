using UnityEngine;

public class LookBoss : MonoBehaviour
{
    public Transform boss;
    public float rotationSpeed = 3f;

    private Vector3 lastBossPos;

    void Start()
    {
        if (boss != null)
        {
            lastBossPos = boss.position;
        }
    }

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
