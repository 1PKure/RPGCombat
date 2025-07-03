using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    [SerializeField] private int playerMaxHealth = 15;

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

    public void ExecuteAttack(CharacterBase attacker, CharacterBase target)
    {
        int damage = attacker.stats.isRanged ? attacker.stats.rangedDamage : attacker.stats.meleeDamage;
        target.TakeDamage(damage);

        Debug.Log($"{attacker.characterName} attacked {target.characterName} for {damage} HP.");

        if (!target.IsAlive())
        {
            Destroy(target.gameObject);
            Debug.Log($"{target.characterName} has died.");
        }

        GameManager.Instance.UIManager.HideActionPanel();
        GameManager.Instance.turnManager.CheckEndConditions();

        attacker.EndPlayerTurn();
        GameManager.Instance.turnManager.EndCurrentPlayerTurn();
    }

    public void ExecuteHeal(PlayerCharacter healer, CharacterBase target)
    {
        int amount = healer.stats.healAmount;
        target.Heal(amount);

        Debug.Log($"{healer.characterName} healed {target.characterName} for {amount} HP.");

        GameManager.Instance.UIManager.HideActionPanel();
        GameManager.Instance.turnManager.CheckEndConditions();

        healer.EndPlayerTurn();
        GameManager.Instance.turnManager.EndCurrentPlayerTurn();
    }
}

