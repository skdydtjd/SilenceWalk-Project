using System.Collections.Generic;
using UnityEngine;

public class Room10 : MonoBehaviour
{
    private static Room10 instance;

    public static Room10 Instance
    {
        get { return instance; }
    }

    public bool Checked = false;
    public Collider[] room10;
    public GameObject box10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        room10 = GetComponents<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        bool insideAllColliders = true;
        Vector3 boxpos = box10.transform.position;

        foreach (var col in room10)
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
