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

            GameManager.Instance.UIManager.ShowTurnMessage($"It's  {current.characterName} turn");
            GameManager.Instance.UIManager.UpdateActiveMarker(current);
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
            GameManager.Instance.UIManager.ShowTurnMessage("You Lose!");
            GameManager.Instance.UIManager.ShowEndPanel(false);
            if (InterstitialManager.Instance != null)
                InterstitialManager.Instance.ShowInterstitialAd();
            return;
        }
        if (aliveEnemies.Count == 0 && alivePlayers.Count > 0)
        {
            gameEnded = true;
            GameManager.Instance.UIManager.ShowTurnMessage($"You Win! {alivePlayers[0].characterName} is the last player standing!");
            GameManager.Instance.UIManager.ShowEndPanel(true);
            if (InterstitialManager.Instance != null)
                InterstitialManager.Instance.ShowInterstitialAd();
            return;
        }
    }

}
