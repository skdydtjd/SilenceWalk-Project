using UnityEngine;
using DG.Tweening;

public class StartKeyUI : MonoBehaviour
{
    public GameObject keypenal;

    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float visibleDuration = 10f;
    [SerializeField] private float moveDistance = 40f;

    private CanvasGroup keyCanvasGroup;
    private RectTransform[] animatedRectTransforms;
    private Vector2[] originalAnchoredPositions;
    private Tween fadeTween;

    public void showpenal()
    {
        if (!PrepareKeyPanel())
        {
            return;
        }

        fadeTween?.Kill();
        keypenal.SetActive(true);
        keyCanvasGroup.alpha = 0f;
        SetAnimatedPositions(Vector2.down * moveDistance);

        fadeTween = DOTween.Sequence()
            .Append(keyCanvasGroup.DOFade(1f, fadeInDuration))
            .Join(CreateMoveTween(Vector2.zero, fadeInDuration))
            .AppendInterval(visibleDuration)
            .Append(keyCanvasGroup.DOFade(0f, fadeOutDuration))
            .Join(CreateMoveTween(Vector2.down * moveDistance, fadeOutDuration))
            .OnComplete(() =>
            {
                if (keypenal != null)
                {
                    keypenal.SetActive(false);
                    SetAnimatedPositions(Vector2.zero);
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
        fadeTween = DOTween.Sequence()
            .Append(keyCanvasGroup.DOFade(0f, fadeOutDuration))
            .Join(CreateMoveTween(Vector2.down * moveDistance, fadeOutDuration))
            .OnComplete(() =>
            {
                if (keypenal != null)
                {
                    keypenal.SetActive(false);
                    SetAnimatedPositions(Vector2.zero);
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
        SetAnimatedPositions(Vector2.zero);
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

        if (animatedRectTransforms == null || animatedRectTransforms.Length == 0)
        {
            int childCount = keypenal.transform.childCount;

            if (childCount > 0)
            {
                animatedRectTransforms = new RectTransform[childCount];
                originalAnchoredPositions = new Vector2[childCount];

                for (int i = 0; i < childCount; i++)
                {
                    animatedRectTransforms[i] = keypenal.transform.GetChild(i).GetComponent<RectTransform>();

                    if (animatedRectTransforms[i] == null)
                    {
                        return false;
                    }

                    originalAnchoredPositions[i] = animatedRectTransforms[i].anchoredPosition;
                }
            }
            else
            {
                RectTransform keyRectTransform = keypenal.GetComponent<RectTransform>();

                if (keyRectTransform == null)
                {
                    return false;
                }

                animatedRectTransforms = new[] { keyRectTransform };
                originalAnchoredPositions = new[] { keyRectTransform.anchoredPosition };
            }
        }

        return true;
    }

    private Tween CreateMoveTween(Vector2 offset, float duration)
    {
        Sequence moveSequence = DOTween.Sequence();

        for (int i = 0; i < animatedRectTransforms.Length; i++)
        {
            Tween moveTween = animatedRectTransforms[i].DOAnchorPos(originalAnchoredPositions[i] + offset, duration);

            if (i == 0)
            {
                moveSequence.Append(moveTween);
            }
            else
            {
                moveSequence.Join(moveTween);
            }
        }

        return moveSequence;
    }

    private void SetAnimatedPositions(Vector2 offset)
    {
        for (int i = 0; i < animatedRectTransforms.Length; i++)
        {
            animatedRectTransforms[i].anchoredPosition = originalAnchoredPositions[i] + offset;
        }
    }
}
