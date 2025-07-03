using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject combatPanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    [SerializeField] private GameObject actionPanel;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button healButton;
    [SerializeField] private Button escapeButton;


    private PlayerCharacter currentPlayer;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ShowCombatUI(bool show)
    {
        combatPanel.SetActive(show);
    }

    public void ShowPlayerHealth(int value)
    {
        playerHealthText.text = $"Player HP: {value}";
    }

    public void ShowEnemyHealth(int value)
    {
        enemyHealthText.text = $"Enemy HP: {value}";
    }

    public void ShowEndPanel(bool win)
    {
        endPanel.SetActive(true);
        endText.text = win ? "¡Ganaste!" : "Perdiste";
    }

    public void HideEndPanel()
    {
        endPanel.SetActive(false);
    }
    private void Highlight(CharacterBase character, bool enable)
    {
        var sprite = character.GetComponent<SpriteRenderer>();
        if (sprite != null)
            sprite.color = enable ? Color.red : Color.white;
    }

    public void ShowActionsFor(PlayerCharacter player)
    {
        currentPlayer = player;
        actionPanel.SetActive(true);

        if (player.characterName == "Fighter")
        {
            attackButton.interactable = true;
            healButton.interactable = false;
        }
        else if (player.characterName == "Healer")
        {
            attackButton.interactable = false;
            healButton.interactable = true;
        }
        else if (player.characterName == "Ranger")
        {
            attackButton.interactable = true;
            healButton.interactable = true;
        }

        attackButton.onClick.RemoveAllListeners();
        healButton.onClick.RemoveAllListeners();
        escapeButton.onClick.RemoveAllListeners();

        attackButton.onClick.AddListener(() => DoAttack());
        healButton.onClick.AddListener(() => DoHeal());
        escapeButton.onClick.AddListener(() => EndTurn());
    }

    private void DoAttack()
    {
        var enemiesInRange = currentPlayer.GetEnemiesInRange();

        if (enemiesInRange.Count == 0)
        {
            Debug.Log("No hay enemigos en rango.");
            return;
        }

        foreach (var enemy in enemiesInRange)
        {
            Highlight(enemy, true);
            enemy.OnClickedToReceiveAction(() =>
            {
                Highlight(enemy, false);
                CombatManager.Instance.ExecuteAttack(currentPlayer, enemy);
            });
        }
    }

    private void DoHeal()
    {
        List<CharacterBase> allies = currentPlayer.GetAlliesInHealRange();

        if (allies.Count == 0)
        {
            Debug.Log("No hay aliados para curar.");
            return;
        }

        foreach (var ally in allies)
        {
            Highlight(ally, true);
            ally.OnClickedToReceiveAction(() =>
            {
                Highlight(ally, false);
                CombatManager.Instance.ExecuteHeal(currentPlayer, ally);
            });
        }
    }

    private void EndTurn()
    {
        actionPanel.SetActive(false);
        currentPlayer.EndPlayerTurn();
        GameManager.Instance.turnManager.EndCurrentPlayerTurn();
    }

    public void HideActionPanel()
    {
        actionPanel.SetActive(false);
    }
}
