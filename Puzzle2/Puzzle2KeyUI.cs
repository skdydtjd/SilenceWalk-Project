using UnityEngine;
using DG.Tweening;

public class Puzzle2KeyUI : MonoBehaviour
{
    public GameObject puzzle2Keypenal;

    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float moveDistance = 40f;

    private CanvasGroup puzzle2CanvasGroup;
    private RectTransform[] animatedRectTransforms;
    private Vector2[] originalAnchoredPositions;
    private Tween fadeTween;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ShowPuzzle2Key();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HidePuzzle2Key();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!PreparePuzzle2KeyPanel())
        {
            return;
        }

        puzzle2CanvasGroup.alpha = 0f;
        SetAnimatedPositions(Vector2.zero);
        puzzle2Keypenal.SetActive(false);
    }

    private void OnDestroy()
    {
        fadeTween?.Kill();
    }

    private void ShowPuzzle2Key()
    {
        if (!PreparePuzzle2KeyPanel())
        {
            return;
        }

        fadeTween?.Kill();
        puzzle2Keypenal.SetActive(true);
        puzzle2CanvasGroup.alpha = 0f;
        SetAnimatedPositions(Vector2.down * moveDistance);

        fadeTween = DOTween.Sequence()
            .Append(puzzle2CanvasGroup.DOFade(1f, fadeInDuration))
            .Join(CreateMoveTween(Vector2.zero, fadeInDuration));
    }

    private void HidePuzzle2Key()
    {
        if (!PreparePuzzle2KeyPanel())
        {
            return;
        }

        fadeTween?.Kill();
        fadeTween = DOTween.Sequence()
            .Append(puzzle2CanvasGroup.DOFade(0f, fadeOutDuration))
            .Join(CreateMoveTween(Vector2.down * moveDistance, fadeOutDuration))
            .OnComplete(() =>
            {
                if (puzzle2Keypenal != null)
                {
                    puzzle2Keypenal.SetActive(false);
                    SetAnimatedPositions(Vector2.zero);
                }
            });
    }

    private bool PreparePuzzle2KeyPanel()
    {
        if (puzzle2Keypenal == null)
        {
            return false;
        }

        if (puzzle2CanvasGroup == null)
        {
            puzzle2CanvasGroup = puzzle2Keypenal.GetComponent<CanvasGroup>();

            if (puzzle2CanvasGroup == null)
            {
                puzzle2CanvasGroup = puzzle2Keypenal.AddComponent<CanvasGroup>();
            }
        }

        if (animatedRectTransforms == null || animatedRectTransforms.Length == 0)
        {
            int childCount = puzzle2Keypenal.transform.childCount;

            if (childCount > 0)
            {
                animatedRectTransforms = new RectTransform[childCount];
                originalAnchoredPositions = new Vector2[childCount];

                for (int i = 0; i < childCount; i++)
                {
                    animatedRectTransforms[i] = puzzle2Keypenal.transform.GetChild(i).GetComponent<RectTransform>();

                    if (animatedRectTransforms[i] == null)
                    {
                        return false;
                    }

                    originalAnchoredPositions[i] = animatedRectTransforms[i].anchoredPosition;
                }
            }
            else
            {
                RectTransform puzzle2RectTransform = puzzle2Keypenal.GetComponent<RectTransform>();

                if (puzzle2RectTransform == null)
                {
                    return false;
                }

                animatedRectTransforms = new[] { puzzle2RectTransform };
                originalAnchoredPositions = new[] { puzzle2RectTransform.anchoredPosition };
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
