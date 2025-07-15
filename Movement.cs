using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MimicSpace
{
    /// <summary>
    /// This is a very basic movement script, if you want to replace it
    /// Just don't forget to update the Mimic's velocity vector with a Vector3(x, 0, z)
    /// </summary>
    public class Movement : MonoBehaviour
    {
        [Header("Controls")]
        [Tooltip("Body Height from ground")]
        [Range(0.5f, 5f)]
        public float height = 0.8f;
        public float speed = 5f;
        Vector3 velocity = Vector3.zero;
        public float velocityLerpCoef = 4f;
        Mimic myMimic;

        public Vector3 moveDirection = Vector3.zero;

        private void Start()
        {
            myMimic = GetComponent<Mimic>();
        }

        void Update()
        {
            velocity = Vector3.Lerp(velocity, moveDirection.normalized * speed, velocityLerpCoef * Time.deltaTime);
            myMimic.velocity = velocity;

            Vector3 nextPosition = transform.position + velocity * Time.deltaTime;

            RaycastHit hit;

            if (Physics.Raycast(nextPosition + Vector3.up * 5f, -Vector3.up, out hit))
            {
                nextPosition.y = Mathf.Lerp(nextPosition.y, hit.point.y + height, velocityLerpCoef * Time.deltaTime);
            }

            transform.position = nextPosition;

        }
    }
}