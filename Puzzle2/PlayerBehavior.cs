using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    public static PlayerBehavior Instance;

    public GameObject straightPrefep, CurvePrefeb, LongCurvePrefep, SlashPrefeb;

    public TextMeshProUGUI InventoryText;
    public TextMeshProUGUI keycount;
    public Image keyImage;

    public AppearPlace[] allPlace;
    int selectPlaceIndex = 0;

    List<Pipe.ItemType> haveItem = new();
    int selectedItemIndex = 0;

    void OnEnable()
    {
        // �� �ε� �̺�Ʈ�� ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // �� �ε� �̺�Ʈ���� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ���� �ε�� ������ ȣ���
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �� �ȿ� �ִ� Place�� �ٽ� �Ҵ�
        allPlace = FindObjectsByType<AppearPlace>(FindObjectsSortMode.None).OrderBy(p => p.placeIndex).ToArray();

        // �κ��丮 UI �ٽ� ã�� (Tag�� �̸����� ã�ƾ� ��Ȯ)
        InventoryText = GameObject.Find("InventoryText")?.GetComponent<TextMeshProUGUI>();

        haveItem.Clear();
        selectedItemIndex = 0;
        selectPlaceIndex = 0;

        InventoryText.alpha = 0f;

        // ���� ���� �ʱ�ȭ
        SelectPlace(0);

        // UI ������Ʈ
        UpdateUI();
    }

    void SelectPlace(int index)
    {
        selectPlaceIndex = index;

        for (int i = 0; i < allPlace.Length; i++)
        {
            var rend = allPlace[i].transform.GetChild(0).GetComponent<Renderer>();

            if (rend != null)
            {
                rend.material.color = (i == selectPlaceIndex ? Color.red : Color.white);
            }
        }
    }

    void TryPickUp()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
        foreach (var hit in hits)
        {
            var pipeItem = hit.GetComponent<Pipe>();

            if (pipeItem != null && hit.gameObject.activeSelf)
            {
                haveItem.Add(pipeItem.Type);
                pipeItem.PickUp();
                return;
            }
        }
    }

    void TryPlace()
    {
        var slot = allPlace[selectPlaceIndex];

        if (slot == null)
            return;

        Vector3 playerPos = transform.position;
        Vector3 slotPos = slot.transform.position;

        playerPos.y = 0f;
        slotPos.y = 0f;

        float distance = Vector3.Distance(playerPos, slotPos);
        if (distance > 2f)
            return;

        // ȸ�� ����
        if (slot.installItem != null)
        {
            var item = slot.installItem.GetComponent<Pipe>();
            haveItem.Add(item.Type);
            Destroy(slot.installItem);
            slot.installItem = null;

            slot.isLock = false;

            selectedItemIndex = Mathf.Clamp(selectedItemIndex, 0, haveItem.Count - 1);

            // ���� üũ
            Puzzle2Manager.Instance.CheckPuzzle();

            return;
        }

        // ��ġ ����
        if (haveItem.Count == 0)
            return;

        GameObject prefab = GetPrefab(haveItem[selectedItemIndex]);
        GameObject newPipe = Instantiate(prefab, slot.transform.position, Quaternion.identity);

        float randomX = 90f * Random.Range(0, 4); // 0, 90, 180, 270
        float randomY = 90f * Random.Range(0, 4);
        float randomZ = 90f * Random.Range(0, 4);

        newPipe.transform.eulerAngles = new Vector3(randomX, randomY, randomZ);

        SFXMusic.Instance.Play("DropItem");

        newPipe.GetComponent<Pipe>().Type = haveItem[selectedItemIndex];
        slot.SetItem(newPipe);

        haveItem.RemoveAt(selectedItemIndex);
        selectedItemIndex = Mathf.Clamp(selectedItemIndex, 0, haveItem.Count - 1);

        // ���� üũ
        Puzzle2Manager.Instance.CheckPuzzle();
    }

    void Rotateselect(string axis)
    {
        var place = allPlace[selectPlaceIndex];

        if (place != null && place.installItem != null)
        {
            if(place.isLock)
            {
                return;
            }

            Transform ItemTransform = place.installItem.transform;

            string axisUpper = axis.ToUpper();

            if (axisUpper == "X")
            {
                ItemTransform.localRotation *= Quaternion.Euler(90f, 0, 0);
            }
            else if (axisUpper == "Y")
            {
                ItemTransform.localRotation *= Quaternion.Euler(0, 90f, 0);
            }
            else if (axisUpper == "Z")
            {
                ItemTransform.localRotation *= Quaternion.Euler(0, 0, 90f);
            }

            SFXMusic.Instance.Play("RotatePipe");

            Puzzle2Manager.Instance.CheckPuzzle();
        }
    }

    void cycleItem()
    {
        if (haveItem.Count == 0) 
            return;
        selectedItemIndex = (selectedItemIndex + 1) % haveItem.Count;
    }

    GameObject GetPrefab(Pipe.ItemType type)
    {
        return type switch
        {
            Pipe.ItemType.Straight => straightPrefep,
            Pipe.ItemType.Curve => CurvePrefeb,
            Pipe.ItemType.LongCurve => LongCurvePrefep,
            Pipe.ItemType.Slash => SlashPrefeb,
            _ => null,
        };
    }

    void UpdateUI()
    {
        if (InventoryText == null) 
            return;

        string result = "";

        if (haveItem.Count == 0)
        {
            result = "�������� ��� ����";
        }
        else
        {
            result = "���� ������: ";

            for (int i = 0; i < haveItem.Count; i++)
            {
                if (i == selectedItemIndex)
                    result += "[" + haveItem[i] + "] ";
                else
                    result += haveItem[i] + " ";
            }

            result += $"\n���� ���: {selectPlaceIndex + 1}��";
        }

        InventoryText.text = result;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Puzzle2")
        {
            keycount.alpha = 0f;

            Color imageColor = keyImage.color;
            imageColor.a = 0f;
            keyImage.color = imageColor;

            InventoryText.alpha = 1f;
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SelectPlace(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) 
            SelectPlace(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) 
            SelectPlace(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) 
            SelectPlace(2);
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) 
            SelectPlace(3);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryPickUp();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPlace();
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            cycleItem();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Rotateselect("x");
        }

        if(Input.GetMouseButtonDown(1))
        {
            Rotateselect("y");
        }

        if(Input.GetMouseButtonDown(2))
        {
            Rotateselect("z");
        }

        UpdateUI();
    }
}
