using MimicSpace;
using UnityEngine;

public class MimicSound : BazicEnemyAI
{
    public AudioSource mimicsound;

    public AudioClip sound;
    bool playingsound = false;

    Movement movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (player != null && movement != null)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            movement.moveDirection = dir;
        }

        if (agent.hasPath)
        {
            Vector3 nextDir = agent.steeringTarget - transform.position;
            movement.moveDirection = nextDir.normalized;
        }
        else
        {
            movement.moveDirection = Vector3.zero;
        }

        if (currentState == State.Chase && !playingsound)
        {
            playingsound = true;
            mimicsound.PlayOneShot(sound);
        }
        else if(currentState == State.Patrol || currentState == State.Return)
        {
            playingsound=false;
        }

    }
}
