namespace ArionDigital
{
    using UnityEngine;

    public class CrashCrate : MonoBehaviour
    {
        [Header("Whole Create")]
        public MeshRenderer wholeCrate;
        public BoxCollider boxCollider;
        [Header("Fractured Create")]
        public GameObject fracturedCrate;

        bool playsound = false;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "prison" || other.gameObject.tag == "Player"
                || other.gameObject.tag == "outside" || other.gameObject.tag == "Untagged" 
                || other.gameObject.tag == "Enemy" || other.gameObject.tag == "WoodBox")
            {
                return;
            }

            wholeCrate.enabled = false;
            boxCollider.enabled = false;
            fracturedCrate.SetActive(true);
            if (!playsound)
            {
                SFXMusic.Instance.Play("BoxCrash");
                playsound = true;
            }
        }

        [ContextMenu("Test")]
        public void Test()
        {
            wholeCrate.enabled = false;
            boxCollider.enabled = false;
            fracturedCrate.SetActive(true);
        }
    }
}