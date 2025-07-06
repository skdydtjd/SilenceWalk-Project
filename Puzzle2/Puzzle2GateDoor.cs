using UnityEngine;

public class Puzzle2GateDoor : AE_Door
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Puzzle2Manager.SolvePipe)
        {
            base.Update();
        }
        else if (!Puzzle2Manager.SolvePipe && Input.GetKeyDown(KeyCode.E) && trig)
        {
            SFXMusic.Instance.Play("DoorLocked");
        }
    }
}
