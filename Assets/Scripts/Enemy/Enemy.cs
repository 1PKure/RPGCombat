using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IUnit
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int attackPower = 3;
    private int currentHealth;
    public string UnitName => gameObject.name;
    public bool IsPlayer => false;
    public int Speed => 2;
    public int Health => currentHealth;

    public bool IsDead => Health <= 0;
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
    }


    public void StartTurn()
    {
        Debug.Log(UnitName + " (enemy) comienza su turno.");
    }
    public void EndTurn() { }
    public int GetAttackPower() => attackPower;
}


