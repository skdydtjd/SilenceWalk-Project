using UnityEngine;
using DG.Tweening;

public class StartKeyUI : MonoBehaviour
{
    public GameObject keypenal;

    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float visibleDuration = 10f;

    private CanvasGroup keyCanvasGroup;
    private Tween fadeTween;

    public void showpenal()
    {
        if (!PrepareKeyPanel())
        {
            return;
        }

        fadeTween?.Kill();
        keypenal.SetActive(true);

        fadeTween = DOTween.Sequence()
            .Append(keyCanvasGroup.DOFade(1f, fadeInDuration))
            .AppendInterval(visibleDuration)
            .Append(keyCanvasGroup.DOFade(0f, fadeOutDuration))
            .OnComplete(() =>
            {
                if (keypenal != null)
                {
                    keypenal.SetActive(false);
                }
            });
    }

    public void hidepenal()
    {
        if (!PrepareKeyPanel())
        {
            return;
        }

        fadeTween?.Kill();
        fadeTween = keyCanvasGroup.DOFade(0f, fadeOutDuration)
            .OnComplete(() =>
            {
                if (keypenal != null)
                {
                    keypenal.SetActive(false);
                }
            });
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!PrepareKeyPanel())
        {
            return;
        }

        keyCanvasGroup.alpha = 0f;
        keypenal.SetActive(false);
    }

    private void OnDestroy()
    {
        fadeTween?.Kill();
    }

    private bool PrepareKeyPanel()
    {
        if (keypenal == null)
        {
            return false;
        }

        if (keyCanvasGroup == null)
        {
            keyCanvasGroup = keypenal.GetComponent<CanvasGroup>();

            if (keyCanvasGroup == null)
            {
                keyCanvasGroup = keypenal.AddComponent<CanvasGroup>();
            }
        }

        return true;
    }
}
