using UnityEngine;

public class AppearPlace : MonoBehaviour
{
    public Pipe.ItemType requiredType;

    public Quaternion correctRotation;

    public GameObject installItem;
    public bool isLock = false;

    Puzzle2Manager manager;

    public Renderer placeRenderer;
    Color originalColor;

    public int placeIndex;

    public bool isCorrect()
    {
        if (installItem == null)
            return false;

        var itemType = installItem.GetComponent<Pipe>().Type;
        if (itemType != requiredType)
            return false;

        // 오일러 각도로 변환해서 90도 단위 오차만 허용
        Vector3 currentEuler = installItem.transform.rotation.eulerAngles;
        Vector3 targetEuler = correctRotation.eulerAngles;

        // 각각 X, Y, Z 축 비교 (오차범위 ±5도 허용)
        bool xCorrect = Mathf.Abs(Mathf.DeltaAngle(currentEuler.x, targetEuler.x)) < 5f;
        bool yCorrect = Mathf.Abs(Mathf.DeltaAngle(currentEuler.y, targetEuler.y)) < 5f;
        bool zCorrect = Mathf.Abs(Mathf.DeltaAngle(currentEuler.z, targetEuler.z)) < 5f;

        return xCorrect && yCorrect && zCorrect;
    }

    public void SetItem(GameObject item)
    {
        installItem = item;
        SetHitghlight(false);
        manager.CheckPuzzle();
    }

    public void ClearSlot()
    {
        if (installItem != null)
        {
            Destroy(installItem);
            installItem = null;
        }

        isLock = false;

        SetHitghlight(true);
        manager.CheckPuzzle();
    }

    void SetHitghlight(bool isActive)
    {
        Color color = originalColor;
        color.a = isActive ? 0.3f : 0f;
        placeRenderer.material.color = color;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = FindAnyObjectByType<Puzzle2Manager>();
        originalColor = placeRenderer.material.color;

        SetHitghlight(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
