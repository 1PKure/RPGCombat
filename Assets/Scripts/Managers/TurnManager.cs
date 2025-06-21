using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    private List<IUnit> units = new();
    private int currentIndex = 0;
    private bool isCombatActive = false;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void RegisterUnit(IUnit unit)
    {
        if (!units.Contains(unit))
            units.Add(unit);
    }

    public void StartCombatTurnCycle()
    {
        if (units.Count == 0) return;

        isCombatActive = true;
        currentIndex = 0;
        units.Sort((a, b) => b.Speed.CompareTo(a.Speed)); // Más rápido va primero
        StartCurrentUnitTurn();
    }

    private void StartCurrentUnitTurn()
    {
        if (CheckWinOrLose()) return;

        if (currentIndex >= units.Count) currentIndex = 0;

        if (units[currentIndex].IsDead)
        {
            currentIndex++;
            StartCurrentUnitTurn();
        }
        else
        {
            units[currentIndex].StartTurn();
        }
    }

    public void EndCurrentTurn()
    {
        units[currentIndex].EndTurn();
        currentIndex++;
        StartCurrentUnitTurn();
    }

    private bool CheckWinOrLose()
    {
        bool playersAlive = units.Exists(u => u.IsPlayer && !u.IsDead);
        bool enemiesAlive = units.Exists(u => !u.IsPlayer && !u.IsDead);

        if (!playersAlive)
        {
            UIManager.Instance.ShowEndPanel(false);
            isCombatActive = false;
            return true;
        }
        if (!enemiesAlive)
        {
            UIManager.Instance.ShowEndPanel(true);
            isCombatActive = false;
            return true;
        }

        return false;
    }
}
