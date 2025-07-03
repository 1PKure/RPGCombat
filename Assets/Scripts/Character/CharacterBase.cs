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

    public CharacterStats stats;
    public bool isMyTurn = false;

    public virtual void Initialize(string name, int hp, int speed, Vector2Int pos)
    {
        this.characterName = name;
        this.maxHealth = stats.maxHP;
        this.currentHealth = stats.maxHP;
        this.speed = stats.speed;
        this.gridPosition = pos;
    }

    public abstract void PerformAction(System.Action onActionComplete);

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        GetComponentInChildren<HPDisplay>()?.UpdateHP();
        Debug.Log($"{characterName} took {amount} damage. Current HP: {currentHealth}");
    }


    public virtual void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        GetComponentInChildren<HPDisplay>()?.UpdateHP();
        Debug.Log($"{characterName} healed to {currentHealth}/{maxHealth} HP");
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
