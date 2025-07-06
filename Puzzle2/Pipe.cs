using UnityEngine;

public class Pipe : MonoBehaviour
{
    public enum ItemType
    {
        Straight,
        Curve,
        LongCurve,
        Slash
    }

    public ItemType Type;

    public void PickUp()
    {
        gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
