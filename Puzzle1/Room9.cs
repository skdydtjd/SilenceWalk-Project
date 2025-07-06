using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Room9 : MonoBehaviour
{
    private static Room9 instance;

    public static Room9 Instance
    {
        get { return instance; }
    }

    public bool Checked = false;
    public Collider[] room9;
    public GameObject box9;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        room9 = GetComponents<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        bool insideAllColliders = true;
        Vector3 boxpos = box9.transform.position;

        foreach (var col in room9)
        {
            Bounds bounds = col.bounds;

            if (boxpos.x > bounds.max.x || boxpos.x < bounds.min.x || boxpos.z > bounds.max.z || boxpos.z < bounds.min.z)
            {
                insideAllColliders = false;
                break;
            }
        }

        if (insideAllColliders)
        {
            if (!Checked)
            {
                Checked = true;
                SFXMusic.Instance.Play("BoxCheck");
            }
        }
        else
        {
            if (Checked)
            {
                Checked = false;
            }
        }
    }
}
