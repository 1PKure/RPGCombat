using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<CharacterBase> turnOrder = new List<CharacterBase>();
    private int turnIndex = 0;
    private System.Action currentPlayerEndCallback;
    private bool gameEnded = false;
    public bool GameEnded => gameEnded;

    public void StartCombat()
    {
        CharacterBase[] allCharacters = FindObjectsOfType<CharacterBase>();
        turnOrder = allCharacters.OrderByDescending(c => c.speed).ToList();


        foreach (var c in turnOrder)
            UIManager.Instance.ShowMessage($"{c.characterName} - Speed: {c.speed}");

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

            UIManager.Instance.ShowTurnMessage($"It's  {current.characterName} turn");
            RectTransform iconTransform = UIManager.Instance.GetActionPanelIcon(current.characterName);
            if (ActiveMarker.Instance != null && iconTransform != null)
            {
                ActiveMarker.Instance.SetUIIndicator(iconTransform);
            }
            current.PerformAction(() =>
            {
                CheckEndConditions();
                NextTurn();
            });
            current.PerformAction(currentPlayerEndCallback);
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
        if (ActiveMarker.Instance != null)
            ActiveMarker.Instance.Hide();
    }

    private void NextTurn()
    {
        turnIndex = (turnIndex + 1) % turnOrder.Count;
    }

    public void CheckEndConditions()
    {
        if (gameEnded) return;
        List<CharacterBase> alivePlayers = turnOrder
            .Where(c => c is PlayerCharacter && c.IsAlive()).ToList();

        List<CharacterBase> aliveEnemies = turnOrder
            .Where(c => c is EnemyCharacter && c.IsAlive()).ToList();

        bool anyPlayerDead = turnOrder
            .Any(c => c is PlayerCharacter && !c.IsAlive());

        if (alivePlayers.Count < turnOrder.Count(c => c is PlayerCharacter) && aliveEnemies.Count > 0)
        {
            gameEnded = true;
            UIManager.Instance.ShowTurnMessage("You Lose!");
            UIManager.Instance.ShowEndPanel(false);
            return;
        }
        if (aliveEnemies.Count == 0 && alivePlayers.Count > 0)
        {
            gameEnded = true;
            UIManager.Instance.ShowTurnMessage($"You Win! {alivePlayers[0].characterName} is the last player standing!");
            UIManager.Instance.ShowEndPanel(true);
            return;
        }
    }

}
