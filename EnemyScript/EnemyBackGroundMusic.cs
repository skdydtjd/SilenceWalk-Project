using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyBackGroundMusic : MonoBehaviour
{
    public static EnemyBackGroundMusic Instance;

    [SerializeField]
    List<BazicEnemyAI> enemies = new List<BazicEnemyAI>();

    public void RegisterEnemy(BazicEnemyAI enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void CheckEnemyStates()
    {
        bool anyChasing = enemies.Any(e => e.currentState == BazicEnemyAI.State.Chase);

        if (anyChasing)
        {
            BackGroundMusic.Instance.MonsterMusic();
        }
        else
        {
            BackGroundMusic.Instance.ChangeToInGameMusic();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
