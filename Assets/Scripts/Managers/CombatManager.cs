using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class CombatManager : MonoBehaviour
{
    public void ExecuteAttack(PlayerCharacter attacker, CharacterBase target)
    {
        Vector2Int attackerPos = attacker.gridPosition;
        Vector2Int targetPos = target.gridPosition;
        int distance = Mathf.Abs(attackerPos.x - targetPos.x) + Mathf.Abs(attackerPos.y - targetPos.y);

        bool canAttack = attacker.stats.isRanged
            ? distance <= attacker.stats.rangedRange
            : distance == 1 || (Mathf.Abs(attackerPos.x - targetPos.x) <= 1 && Mathf.Abs(attackerPos.y - targetPos.y) <= 1);

        if (!canAttack)
        {
            GameManager.Instance.UIManager.ShowMessage("Target out of range!");
            return;
        }

        int damage = attacker.stats.isRanged ? attacker.stats.rangedDamage : attacker.stats.meleeDamage;
        target.TakeDamage(damage);

        GameManager.Instance.UIManager.ShowMessage($"{attacker.characterName} attacked {target.characterName} for {damage} HP.");

        if (!target.IsAlive())
        {
            Destroy(target.gameObject);
            GameManager.Instance.UIManager.ShowMessage($"{target.characterName} has died.");
        }

        GameManager.Instance.UIManager.UpdateHealthDisplays();
        GameManager.Instance.turnManager.CheckEndConditions();

        attacker.EndPlayerTurn();
        GameManager.Instance.turnManager.EndCurrentPlayerTurn();
    }


    public void ExecuteHeal(PlayerCharacter healer, CharacterBase target)
    {
        int dist = Mathf.Abs(healer.gridPosition.x - target.gridPosition.x)
             + Mathf.Abs(healer.gridPosition.y - target.gridPosition.y);

        if (target != healer && dist > healer.stats.healRange)
        {
            GameManager.Instance.UIManager.ShowMessage("Target out of heal range!");
            return;
        }


        int amount = healer.stats.healAmount;
        target.Heal(amount);
        GameManager.Instance.UIManager.ShowMessage($"{healer.characterName} healed {target.characterName} for {amount} HP.");

        GameManager.Instance.UIManager.UpdateHealthDisplays();
        GameManager.Instance.turnManager.CheckEndConditions();

        healer.EndPlayerTurn();
        GameManager.Instance.turnManager.EndCurrentPlayerTurn();
    }
}

