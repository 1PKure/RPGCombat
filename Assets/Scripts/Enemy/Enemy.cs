using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int attackPower = 3;

    public int Health { get; private set; }

    private void Awake()
    {
        Health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
    }

    public bool IsDead() => Health <= 0;

    public int GetAttackPower() => attackPower;
}


