using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<CharacterBase> turnOrder = new List<CharacterBase>();
    private int turnIndex = 0;

    private System.Action currentPlayerEndCallback;

    public void StartCombat()
    {
        CharacterBase[] allCharacters = FindObjectsOfType<CharacterBase>();
        turnOrder = allCharacters.OrderByDescending(c => c.stats.speed).ToList();

        Debug.Log("Turn order:");
        foreach (var c in turnOrder)
            //Debug.Log($"{c.characterName} - Speed: {c.stats.speed}");

        StartCoroutine(CombatCycle());
    }

    private IEnumerator CombatCycle()
    {
        while (true)
        {
            CharacterBase current = turnOrder[turnIndex];

            if (!current.IsAlive())
            {
                NextTurn();
                continue;
            }

            //Debug.Log($"Es el turno de {current.characterName}");

            current.PerformAction(() =>
            {
                CheckEndConditions();
                NextTurn();
            });

            yield return new WaitUntil(() => currentPlayerEndCallback == null);
        }
    }

    public void RegisterPlayerCallback(CharacterBase character, System.Action onEnd)
    {
        currentPlayerEndCallback = onEnd;
    }

    public void EndCurrentPlayerTurn()
    {
        currentPlayerEndCallback?.Invoke();
        currentPlayerEndCallback = null;
        ActiveMarker.Instance.Hide();
    }

    private void NextTurn()
    {
        turnIndex = (turnIndex + 1) % turnOrder.Count;
    }

    public void CheckEndConditions()
    {
        List<CharacterBase> alivePlayers = turnOrder
            .Where(c => c is PlayerCharacter && c.IsAlive()).ToList();

        List<CharacterBase> aliveEnemies = turnOrder
            .Where(c => c is EnemyCharacter && c.IsAlive()).ToList();

        bool anyPlayerDead = turnOrder
            .Any(c => c is PlayerCharacter && !c.IsAlive());

        if (aliveEnemies.Count == 0)
        {
            Debug.Log(" Victoria: todos los enemigos han sido derrotados.");
            GameManager.Instance.UIManager.ShowEndPanel(true);
            StopAllCoroutines();
        }
        else if (anyPlayerDead && aliveEnemies.Count > 0)
        {
            Debug.Log(" Derrota: un jugador murió mientras quedan enemigos.");
            GameManager.Instance.UIManager.ShowEndPanel(false);
            StopAllCoroutines();
        }
    }

}
