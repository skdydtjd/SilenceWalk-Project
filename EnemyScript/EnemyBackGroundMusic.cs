using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyBackGroundMusic : MonoBehaviour
{
    public static EnemyBackGroundMusic Instance;

    [SerializeField]
    private List<BazicEnemyAI> enemies = new List<BazicEnemyAI>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void RegisterEnemy(BazicEnemyAI enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void CheckEnemyStates()
    {
        bool anyChasing = enemies.Any(e => e.currentState == BazicEnemyAI.State.Chase);

        if (anyChasing)
            BackGroundMusic.Instance.MonsterMusic();
        else
            BackGroundMusic.Instance.ChangeToInGameMusic();
    }
}
