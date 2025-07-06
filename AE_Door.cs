using UnityEngine;
using UnityEngine.UI;

public class AE_Door : MonoBehaviour
{
    public bool trig, open = false;//trig-проверка входа выхода в триггер(игрок должен быть с тегом Player) open-закрыть и открыть дверь
    public float smooth = 2.0f;//скорость вращения
    public float DoorOpenAngle = 87.0f;//угол вращения 
    protected Vector3 defaulRot;
    protected Vector3 openRot;

    // Rotation을 Quaternion으로 변경
    protected Quaternion defaultRotQ;
    protected Quaternion openRotQ;
    public Text txt;//text 

    bool openSound = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        // EulerAngle로 바꿈
        defaulRot = transform.eulerAngles;
        openRot = new Vector3(defaulRot.x, defaulRot.y + DoorOpenAngle, defaulRot.z);

        defaultRotQ = transform.rotation;

        // Quaternion으로 바꿈
        openRotQ = Quaternion.Euler(openRot);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (open)//открыть
        {
            // Quaternion으로 바꿈
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotQ, Time.deltaTime * smooth);

            if (!openSound)
            {
                DoorSound();
                openSound = true;
            }
        }
        else//закрыть
        {
            // Quaternion으로 바꿈
            transform.rotation = Quaternion.Slerp(transform.rotation, defaultRotQ, Time.deltaTime * smooth);
            openSound = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && trig)
        {
            open = !open;
        }
        if (trig)
        {
            if (open)
            {
                txt.text = "Close E";
            }
            else
            {
                txt.text = "Open E";
            }
        }
    }

    // 효과음 추가
    public virtual void DoorSound()
    {
        SFXMusic.Instance.Play("SteelDoor");
    }

    protected virtual void OnTriggerEnter(Collider coll)//вход и выход в\из  триггера 
    {
        if (coll.tag == "Player")
        {
            if (!open)
            {
                txt.text = "Close E ";
            }
            else
            {
                txt.text = "Open E";
            }
            trig = true;
        }
    }
    private void OnTriggerExit(Collider coll)//вход и выход в\из  триггера 
    {
        if (coll.tag == "Player")
        {
            txt.text = " ";
            trig = false;
        }
    }
}
