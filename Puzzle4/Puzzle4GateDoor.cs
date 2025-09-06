using UnityEngine;

public class Puzzle4GateDoor : AE_Door
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        // 저장 후 불러올 시 클리어 여부 다시 체크
        if (AppearKeyPuzzle4.KeyGatherCount >= 2)
        {
            open = false;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (AppearKeyPuzzle4.KeyGatherCount >= 2)
        {
            base.Update();
        }
        else if (AppearKeyPuzzle4.KeyGatherCount < 2 && Input.GetKeyDown(KeyCode.E) && trig)
        {
            SFXMusic.Instance.Play("DoorLocked");
        }
    }
}
