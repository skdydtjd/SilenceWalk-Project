using System.Collections.Generic;
using UnityEngine;

public class CameraSeeBehindWall : MonoBehaviour
{
    public Transform player;
    public Transform cameraTransform;
    public LayerMask obstacleMask;

    [Header("투명처리에 사용될 Material")]
    public Material transparentMaterial;

    private List<Renderer> transparentRenderers = new List<Renderer>();

    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();

    void Update()
    {
        RestoreMaterials();

        if (player == null || cameraTransform == null) 
            return;

        Vector3 direction = player.position - cameraTransform.position;
        float distance = direction.magnitude;

        Ray ray = new Ray(cameraTransform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, distance, obstacleMask);

        foreach (RaycastHit hit in hits)
        {
            LODGroup lodGroup = hit.collider.GetComponentInParent<LODGroup>();
            if (lodGroup != null)
            {
                foreach (LOD lod in lodGroup.GetLODs())
                {
                    foreach (Renderer rend in lod.renderers)
                    {
                        if (rend != null)
                            MakeTransparent(rend);
                    }
                }
            }
            else
            {
                Renderer rend = hit.collider.GetComponent<Renderer>();
                if (rend != null)
                    MakeTransparent(rend);
            }
        }
    }

    void MakeTransparent(Renderer rend)
    {
        if (!originalMaterials.ContainsKey(rend))
            originalMaterials[rend] = rend.sharedMaterials;

        Material[] newMats = new Material[rend.sharedMaterials.Length];

        for (int i = 0; i < newMats.Length; i++)
        {
            // 개별 복제하여 적용
            newMats[i] = new Material(transparentMaterial);
        }

        rend.materials = newMats;
        transparentRenderers.Add(rend);
    }

    void RestoreMaterials()
    {
        foreach (Renderer rend in transparentRenderers)
        {
            if (rend != null && originalMaterials.ContainsKey(rend))
            {
                rend.materials = originalMaterials[rend];
            }
        }

        transparentRenderers.Clear();
        originalMaterials.Clear();
    }
}
