using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<CharacterBase> turnOrder = new List<CharacterBase>();
    private int currentTurnIndex = 0;
    private bool isCombatActive = false;
    private System.Action currentPlayerEndCallback;
    public static TurnManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        StartCombat();
    }

    public void StartCombat()
    {
        CharacterBase[] characters = FindObjectsOfType<CharacterBase>();

        turnOrder = characters.OrderByDescending(c => c.speed).ToList();

        Debug.Log("Turn order:");
        foreach (var c in turnOrder)
            //Debug.Log($"{c.characterName} - Speed: {c.speed}");

        isCombatActive = true;
        currentTurnIndex = 0;
        StartCoroutine(ExecuteTurn());
    }

    private IEnumerator ExecuteTurn()
    {
        while (isCombatActive)
        {
            if (turnOrder.Count == 0)
            {
                Debug.LogWarning("Jugadores no encontrados.");
                yield break;
            }

            CharacterBase currentCharacter = turnOrder[currentTurnIndex];

            if (currentCharacter.IsAlive())
            {
                Debug.Log($"Es el turno de {currentCharacter.characterName}.");
                bool actionCompleted = false;

                currentCharacter.PerformAction(() => { actionCompleted = true; });

                yield return new WaitUntil(() => actionCompleted);
            }
            else
            {
                Debug.Log($"{currentCharacter.characterName} Muerto. Salteando turno.");
            }

            CheckVictoryConditions();

            currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
        }
    }

    private void CheckVictoryConditions()
    {
        bool anyPlayerAlive = turnOrder.Any(c => c is PlayerCharacter && c.IsAlive());
        bool anyEnemyAlive = turnOrder.Any(c => c is EnemyCharacter && c.IsAlive());

        if (!anyEnemyAlive)
        {
            Debug.Log("¡Victoria! Todos los enemigos fueron derrotados.");
            isCombatActive = false;
        }
        else if (!anyPlayerAlive)
        {
            Debug.Log("Derrota... Todos los jugadores han caído.");
            isCombatActive = false;
        }
    }

    public void RegisterPlayerCallback(CharacterBase player, System.Action onActionComplete)
    {
        currentPlayerEndCallback = onActionComplete;
    }

    public void EndCurrentPlayerTurn()
    {
        currentPlayerEndCallback?.Invoke();
        currentPlayerEndCallback = null;
    }
}
