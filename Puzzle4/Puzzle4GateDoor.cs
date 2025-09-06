using UnityEngine;

public class Puzzle4GateDoor : AE_Door
{
    // Ŭ���� ���� ����
    bool canUseDoor = false;

    // �����ϰ� �ҷ��� �� Ŭ���� ���� ��Ȯ��
    public void Refresh()
    {
        if (AppearKeyPuzzle4.KeyGatherCount >= 2)
        {
            canUseDoor = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        
        // Ȯ��
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
