using UnityEngine;

public class Puzzle4GateDoor : AE_Door
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (AppearKeyPuzzle4.KeyGatherCount == 2)
        {
            base.Update();
        }
        else if (AppearKeyPuzzle4.KeyGatherCount != 2 && Input.GetKeyDown(KeyCode.E) && trig)
        {
            SFXMusic.Instance.Play("DoorLocked");
        }
    }
}
