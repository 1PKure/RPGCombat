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

    public virtual void Initialize(string name, int health, int speed, Vector2Int position)
    {
        this.characterName = name;
        this.maxHealth = health;
        this.currentHealth = health;
        this.speed = speed;
        this.gridPosition = position;
    }

    public abstract void PerformAction(System.Action onActionComplete);

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log($"{characterName} took {amount} damage. Current HP: {currentHealth}");
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }
}
