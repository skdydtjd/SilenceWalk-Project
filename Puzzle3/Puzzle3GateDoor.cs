using UnityEngine;

public class Puzzle3GateDoor : AE_Door
{
    // Ŭ����� ����
    bool canUseDoor = false;

    public override void DoorSound()
    {
        SFXMusic.Instance.Play("PrisonDoor");
    }

    // �����ϰ� �ҷ��� �� Ŭ���� ���� ��Ȯ��
    public void Refresh()
    {
        if (Puzzle3UIforPlayer.NumberOfKey >= 3)
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
        if (Puzzle3UIforPlayer.NumberOfKey >= 3 || canUseDoor)
        {
            base.Update();
        }
        else if (Puzzle3UIforPlayer.NumberOfKey < 3 && Input.GetKeyDown(KeyCode.E) && trig)
        {
            SFXMusic.Instance.Play("DoorLocked");
        }
    }
}
