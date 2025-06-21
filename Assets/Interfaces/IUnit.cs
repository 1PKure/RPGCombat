using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    string UnitName { get; }
    bool IsPlayer { get; }
    int Speed { get; }
    int Health { get; }
    bool IsDead { get; }
    void StartTurn();
    void EndTurn();
}
