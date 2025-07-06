using UnityEngine;

public class LowHPSound : MonoBehaviour
{
    public AudioSource Lowhp;
    public AudioClip lowhpsound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMove.Instance.hp > 0 && PlayerMove.Instance.hp < 50)
        {
            if (!Lowhp.isPlaying)
            {
                Lowhp.clip = lowhpsound;
                Lowhp.loop = true;
                Lowhp.Play();
            }
        }
        else
        {
            if (Lowhp.isPlaying)
            {
                Lowhp.Stop();
            }
        }
    }
}
