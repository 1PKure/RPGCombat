using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    public string characterName;
    public int maxHealth;
    public int currentHealth;
    public int speed;
    public Vector2Int gridPosition;
    public bool isMyTurn = false;
    public abstract void PerformAction(System.Action onActionComplete);

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        GetComponentInChildren<HPDisplay>()?.UpdateHP();
        UIManager.Instance.ShowMessage($"{characterName} took {amount} damage. Current HP: {currentHealth}");
        if (!IsAlive())
        {
            UIManager.Instance.ShowMessage($"{characterName} has been defeated.");
            GameManager.Instance.turnManager.CheckEndConditions();
            Destroy(gameObject);
        }
    }


    public virtual void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        GetComponentInChildren<HPDisplay>()?.UpdateHP();
        UIManager.Instance.ShowMessage($"{characterName} healed to {currentHealth}/{maxHealth} HP");
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public virtual void OnClickedToReceiveAction(System.Action callback)
    {
        
    }

    public void EndPlayerTurn()
    {
        isMyTurn = false;
    }
}
