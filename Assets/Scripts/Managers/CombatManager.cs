using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    [SerializeField] private int playerMaxHealth = 15;
    private int playerHealth;

    private Enemy currentEnemy;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartCombat(Enemy enemy)
    {
        currentEnemy = enemy;
        playerHealth = playerMaxHealth;

        GameManager.Instance.ChangeState(GameState.Combat);
        UIManager.Instance.ShowCombatUI(true);
    }

    public void PlayerAttack(int damage)
    {
        currentEnemy.TakeDamage(damage);
        UIManager.Instance.ShowEnemyHealth(currentEnemy.Health);

        if (currentEnemy.IsDead())
        {
            EndCombat(true);
            Destroy(currentEnemy.gameObject);
            return;
        }

        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        playerHealth -= currentEnemy.GetAttackPower();
        UIManager.Instance.ShowPlayerHealth(playerHealth);

        if (playerHealth <= 0)
        {
            EndCombat(false);
        }
    }

    private void EndCombat(bool playerWon)
    {
        UIManager.Instance.ShowCombatUI(false);

        GameManager.Instance.ChangeState(playerWon ? GameState.Victory : GameState.Defeat);
        UIManager.Instance.ShowEndPanel(playerWon);
    }
}

