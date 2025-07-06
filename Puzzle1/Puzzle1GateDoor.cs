using UnityEngine;

public class Puzzle1GateDoor : AE_Door
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (KeyRotate.gatheredByPlayer)
        {
            base.Update();
        }
        else if (!KeyRotate.gatheredByPlayer && Input.GetKeyDown(KeyCode.E) && trig)
        {
            SFXMusic.Instance.Play("DoorLocked");
        }
    }

    public override void DoorSound()
    {
        SFXMusic.Instance.Play("PrisonDoor");
    }
}
