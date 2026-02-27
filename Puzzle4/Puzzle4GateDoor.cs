using UnityEngine;

public class Puzzle4GateDoor : AE_Door
{
    // 클리어 여부 변수
    bool canUseDoor = false;

    // 저장하고 불러올 때 클리어 여부 재확인
    public void Refresh()
    {
        if (AppearKeyPuzzle4.KeyGatherCount >= 2)
        {
            canUseDoor = true;
        }
        else
        {
            Debug.Log("Puzzle4Door 불러오기 실패");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        
        // 확인
        Refresh();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (AppearKeyPuzzle4.KeyGatherCount >= 2|| canUseDoor)
        {
            base.Update();
        }
        else if (AppearKeyPuzzle4.KeyGatherCount < 2 && Input.GetKeyDown(KeyCode.E) && trig)
        {
            SFXMusic.Instance.Play("DoorLocked");
        }
    }
}
