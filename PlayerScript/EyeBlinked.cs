using UnityEngine;

public class EyeBlinked : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    void Blink()
    {
        int time = Random.Range(2, 5);
        _Invoke(time);
    }

    void _Invoke(int time)
    {
        animator.SetTrigger("EyeBlink");
        Invoke("Blink", time);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Blink();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
