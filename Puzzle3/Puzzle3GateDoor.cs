using UnityEngine;

public class Puzzle3GateDoor : AE_Door
{
    public override void DoorSound()
    {
        SFXMusic.Instance.Play("PrisonDoor");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Puzzle3UIforPlayer.NumberOfKey == 3)
        {
            base.Update();
        }
        else if (Puzzle3UIforPlayer.NumberOfKey != 3 && Input.GetKeyDown(KeyCode.E) && trig)
        {
            SFXMusic.Instance.Play("DoorLocked");
        }
    }
}
