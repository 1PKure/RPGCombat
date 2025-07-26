using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject combatPanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private TextMeshProUGUI fighterHealthText;
    [SerializeField] private TextMeshProUGUI enemy1HealthText;
    [SerializeField] private TextMeshProUGUI healerHealthText;
    [SerializeField] private TextMeshProUGUI rangerHealthText;
    [SerializeField] private TextMeshProUGUI enemy2HealthText;
    [SerializeField] private Button enemy1Button;
    [SerializeField] private Button enemy2Button;
    [SerializeField] private Button fighterButton;
    [SerializeField] private Button healerButton;
    [SerializeField] private Button rangerButton;
    [SerializeField] private Transform actionMarker;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button healButton;
    [SerializeField] private Button escapeButton;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject fighterMarker;
    [SerializeField] private GameObject healerMarker;
    [SerializeField] private GameObject rangerMarker;
    [SerializeField] private GameObject enemy1Marker;
    [SerializeField] private GameObject enemy2Marker;

    private float messageDuration = 2f;
    private PlayerCharacter currentPlayer;
    public RectTransform GetActionPanelIcon(string characterName)
    {
        Transform iconTransform = actionMarker.Find(characterName);
        return iconTransform?.GetComponent<RectTransform>();
    }
    public void ShowCombatUI(bool show)
    {
        combatPanel.SetActive(show);
    }

    public void ShowPlayerHealth(int value)
    {
        fighterHealthText.text = $"Player HP: {value}";
    }

    public void ShowEnemyHealth(int value)
    {
        enemy1HealthText.text = $"Enemy HP: {value}";
    }

    public void ShowEndPanel(bool win)
    {
        endPanel.SetActive(true);
        endText.text = win ? "¡You Won!" : "You Lost.";
        Time.timeScale = 0f;
    }
    public void ShowTurnMessage(string message)
    {
        turnText.text = message;
    }

    public void ShowMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessageCoroutine(message));
    }

    private IEnumerator ShowMessageCoroutine(string msg)
    {
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(messageDuration);
        messageText.gameObject.SetActive(false);
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
        actionMarker.gameObject.SetActive(true);

        attackButton.interactable = true;
        healButton.interactable = true;
        escapeButton.interactable = true;
        attackButton.onClick.RemoveAllListeners();
        healButton.onClick.RemoveAllListeners();
        escapeButton.onClick.RemoveAllListeners();

        attackButton.onClick.AddListener(() => DoAttack());
        healButton.onClick.AddListener(() => DoHeal());
        escapeButton.onClick.AddListener(() => EndTurn());
    }

    public void DoAttack()
    {
        ClearHighlightsAndCallbacks();

        List<EnemyCharacter> targets = currentPlayer.GetEnemiesInRange();
        if (targets.Count == 0)
        {
            ShowMessage("No enemies in range!");
            return;
        }

        foreach (var enemy in targets)
        {
            if (enemy.characterName == "Enemy 1")
            {
                enemy1Button.onClick.RemoveAllListeners();
                enemy1Button.onClick.AddListener(() => OnEnemyClickedToAttack(enemy));
            }
            else if (enemy.characterName == "Enemy 2")
            {
                enemy2Button.onClick.RemoveAllListeners();
                enemy2Button.onClick.AddListener(() => OnEnemyClickedToAttack(enemy));
            }
        }

        ShowMessage("Choose a target to attack");
    }


    private void OnEnemyClickedToAttack(CharacterBase target)
    {
        GameManager.Instance.combatManager.ExecuteAttack(currentPlayer, target);
        ClearHighlightsAndCallbacks();
    }


    public void DoHeal()
    {
        ClearHighlightsAndCallbacks();

        List<CharacterBase> targets = currentPlayer.GetAlliesInHealRange();
        if (targets.Count == 0)
        {
            ShowMessage("No allies in range!");
            return;
        }

        foreach (var ally in targets)
        {
            if (ally.characterName == "Fighter")
            {
                fighterButton.onClick.RemoveAllListeners();
                fighterButton.onClick.AddListener(() => OnAllyClickedToHeal(ally));
            }
            else if (ally.characterName == "Healer")
            {
                healerButton.onClick.RemoveAllListeners();
                healerButton.onClick.AddListener(() => OnAllyClickedToHeal(ally));
            }
            else if (ally.characterName == "Ranger")
            {
                rangerButton.onClick.RemoveAllListeners();
                rangerButton.onClick.AddListener(() => OnAllyClickedToHeal(ally));
            }
        }

        ShowMessage("Choose an ally to heal");
    }


    private void OnAllyClickedToHeal(CharacterBase target)
    {
        GameManager.Instance.combatManager.ExecuteHeal(currentPlayer, target);
        ClearHighlightsAndCallbacks();
    }


    private void EndTurn()
    {
        HideActionPanel();
        ClearHighlightsAndCallbacks();
        currentPlayer.EndPlayerTurn();
        GameManager.Instance.turnManager.EndCurrentPlayerTurn();
    }

    public void HideActionPanel()
    {
        actionMarker.gameObject.SetActive(false);
    }
    public void UpdateHealthDisplays()
    {
        var players = FindObjectsOfType<PlayerCharacter>();
        foreach (var p in players)
        {
            if (p.characterName == "Fighter")
                fighterHealthText.text = $"Fighter HP: {p.currentHealth}";
            else if (p.characterName == "Healer")
                healerHealthText.text = $"Healer HP: {p.currentHealth}";
            else if (p.characterName == "Ranger")
                rangerHealthText.text = $"Ranger HP: {p.currentHealth}";
        }

        var enemies = FindObjectsOfType<EnemyCharacter>();
        if (enemies.Length > 0)
            enemy1HealthText.text = $"Enemy 1 HP: {enemies[0].currentHealth}";
        if (enemies.Length > 1)
            enemy2HealthText.text = $"Enemy 2 HP: {enemies[1].currentHealth}";
    }

    private void ClearHighlightsAndCallbacks()
    {
        foreach (var character in FindObjectsOfType<CharacterBase>())
        {
            Highlight(character, false);
            character.OnClickedToReceiveAction(null);
        }

        enemy1Button.onClick.RemoveAllListeners();
        enemy2Button.onClick.RemoveAllListeners();
        fighterButton.onClick.RemoveAllListeners();
        healerButton.onClick.RemoveAllListeners();
        rangerButton.onClick.RemoveAllListeners();
    }

    public void UpdateActiveMarker(CharacterBase current)
    {
        fighterMarker.SetActive(false);
        healerMarker.SetActive(false);
        rangerMarker.SetActive(false);
        enemy1Marker.SetActive(false);
        enemy2Marker.SetActive(false);

        if (current.characterName == "Fighter")
            fighterMarker.SetActive(true);
        else if (current.characterName == "Healer")
            healerMarker.SetActive(true);
        else if (current.characterName == "Ranger")
            rangerMarker.SetActive(true);
        else if (current.characterName == "Enemy 1")
            enemy1Marker.SetActive(true);
        else if (current.characterName == "Enemy 2")
            enemy2Marker.SetActive(true);
    }
}
