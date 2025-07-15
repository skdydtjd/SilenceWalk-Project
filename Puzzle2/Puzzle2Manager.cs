using UnityEngine;

public class Puzzle2Manager : MonoBehaviour
{
    public static Puzzle2Manager Instance;

    public AppearPlace[] allPlace;

    public static bool SolvePipe = false;

    public void CheckPuzzle()
    {
        if (SolvePipe)
        {
            return;
        }

        bool allCorrect = true;

        foreach (var place in allPlace)
        {
            if (place.installItem == null)
            {
                allCorrect = false;
                continue;
            }

            bool correct = place.isCorrect();

            if (correct && !place.isLock)
            {
                place.isLock = true;
                SFXMusic.Instance.Play("SolvePuzzle2");
            }

            if(!correct)
            {
                allCorrect = false;
            }
        }

        // 모든 슬롯이 isCorrect()로 정답이어야만 클리어 처리
        if (allCorrect)
        {
            SolvePipe = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
