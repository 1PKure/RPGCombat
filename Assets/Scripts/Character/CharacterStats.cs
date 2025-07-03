using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "RPG/Character Stats")]
public class CharacterStats : ScriptableObject
{
    public string characterName;

    public int maxHP;
    public int speed;

    public int meleeDamage;
    public int rangedDamage;
    public int rangedRange;

    public int healAmount;
    public int healRange;

    public bool canHeal;
    public bool isRanged;
}
