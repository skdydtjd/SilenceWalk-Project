using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeyCount : KeyRotate
{
    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base .OnTriggerExit(other);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        Vector3 eulerAngle = transform.eulerAngles;
        eulerAngle.y += Time.deltaTime * spinDuration;

        transform.rotation = Quaternion.Euler(eulerAngle);

        if (trigger && Input.GetKeyDown(KeyCode.Q))
        {
            Puzzle3UIforPlayer.NumberOfKey++;
            Destroy(gameObject);
        }
    }
}
