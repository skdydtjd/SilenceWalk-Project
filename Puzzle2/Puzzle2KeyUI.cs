using UnityEngine;
using DG.Tweening;

public class Puzzle2KeyUI : MonoBehaviour
{
    public GameObject puzzle2Keypenal;

    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private float fadeOutDuration = 1f;

    private CanvasGroup puzzle2CanvasGroup;
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
        fadeTween = puzzle2CanvasGroup.DOFade(1f, fadeInDuration);
    }

    private void HidePuzzle2Key()
    {
        if (!PreparePuzzle2KeyPanel())
        {
            return;
        }

        fadeTween?.Kill();
        fadeTween = puzzle2CanvasGroup.DOFade(0f, fadeOutDuration)
            .OnComplete(() =>
            {
                if (puzzle2Keypenal != null)
                {
                    puzzle2Keypenal.SetActive(false);
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

        return true;
    }
}
