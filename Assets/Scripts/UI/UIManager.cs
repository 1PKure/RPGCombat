using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.TextCore.Text;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject combatPanel;
    [SerializeField] private GameObject endCanvas;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private TextMeshProUGUI fighterHPText;
    [SerializeField] private TextMeshProUGUI enemy1HPText;
    [SerializeField] private TextMeshProUGUI healerHPText;
    [SerializeField] private TextMeshProUGUI rangerHPText;
    [SerializeField] private TextMeshProUGUI enemy2HPText;
    [SerializeField] private Button enemy1Button;
    [SerializeField] private Button enemy2Button;
    [SerializeField] private Button fighterButton;
    [SerializeField] private Button healerButton;
    [SerializeField] private Button rangerButton;
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
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject joystickGameObject;
    [SerializeField] private GameObject mapView;

    private float messageDuration = 3.5f;
    private PlayerCharacter currentPlayer;
    private enum PendingAction { None, Attack, Heal }
    private PendingAction pendingAction = PendingAction.None;


    private void Start()
    {
        creditsPanel.SetActive(false);
    }
    public void SetupUI()
    {
        combatPanel.SetActive(true);


        endCanvas.SetActive(false);
        endPanel.SetActive(true);

        creditsPanel.SetActive(false);
        messageText.gameObject.SetActive(false);

        joystickGameObject.SetActive(true);
        mapView.SetActive(true);

        ClearHighlightsAndCallbacks();
    }
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }
    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
    public void ShowCombatUI(bool show)
    {
        combatPanel.SetActive(show);
    }

    public void ShowPlayerHealth(int value)
    {
        fighterHPText.text = $"Player HP: {value}";
    }

    public void ShowEnemyHealth(int value)
    {
        enemy1HPText.text = $"Enemy HP: {value}";
    }

    public void ShowEndPanel(bool win)
    {
        // Oculta todo menos el endCanvas
        ShowCombatUI(false);
        creditsPanel.SetActive(false);
        joystickGameObject.SetActive(false);
        mapView.SetActive(false);

        endCanvas.SetActive(true);
        endPanel.GetComponent<Image>().color = win ? Color.green : Color.red;
        endText.text = win ? "¡You Won!" : "You Lost :(";
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

        attackButton.interactable = true;
        healButton.interactable = true;
        escapeButton.interactable = true;
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
        pendingAction = PendingAction.Attack;
        foreach (var enemy in targets)
        {
            if (enemy.characterName == "Enemy 1")
            {
                enemy1Button.onClick.RemoveAllListeners();
                enemy1Button.onClick.AddListener(() => OnTargetSelected(enemy));
            }
            else if (enemy.characterName == "Enemy 2")
            {
                enemy2Button.onClick.RemoveAllListeners();
                enemy2Button.onClick.AddListener(() => OnTargetSelected(enemy));
            }
        }

        ShowMessage("Choose a target to attack");
    }



    public void DoHeal()
    {
        ClearHighlightsAndCallbacks();

        if (currentPlayer.characterName != "Healer")
        {
            ExecuteHeal(currentPlayer);
            return;
        }

        pendingAction = PendingAction.Heal;
        List<CharacterBase> targets = currentPlayer.GetAlliesInHealRange();
        if (targets.Count == 0)
        {
            ShowMessage("No allies in range!");
            return;
        }

        foreach (var ally in targets)
        {
            switch (ally.characterName)
            {
                case "Fighter":
                    fighterButton.onClick.RemoveAllListeners();
                    fighterButton.onClick.AddListener(() => OnTargetSelected(ally));
                    break;
                case "Healer":
                    healerButton.onClick.RemoveAllListeners();
                    healerButton.onClick.AddListener(() => OnTargetSelected(ally));
                    break;
                case "Ranger":
                    rangerButton.onClick.RemoveAllListeners();
                    rangerButton.onClick.AddListener(() => OnTargetSelected(ally));
                    break;
            }
        }

        ShowMessage("Choose an ally to heal");
    }


    private void ExecuteAttack(CharacterBase enemy)
    {
        GameManager.Instance.combatManager.ExecuteAttack(currentPlayer, enemy);
        UpdateHealthDisplays();
        ClearHighlightsAndCallbacks();
    }
    private void ExecuteHeal(CharacterBase ally)
    {
        GameManager.Instance.combatManager.ExecuteHeal(currentPlayer, ally);
        UpdateHealthDisplays();
        ClearHighlightsAndCallbacks();
    }

    private void OnTargetSelected(CharacterBase target)
    {
        if (pendingAction == PendingAction.Attack)
        {
            ExecuteAttack(target);
        }
        else if (pendingAction == PendingAction.Heal)
        {
            ExecuteHeal(target);
        }

        pendingAction = PendingAction.None;
    }



    private void EndTurn()
    {
        ClearHighlightsAndCallbacks();
        currentPlayer.EndPlayerTurn();
        GameManager.Instance.turnManager.EndCurrentPlayerTurn();
    }
    public void UpdateHealthDisplays()
    {
        var players = FindObjectsOfType<PlayerCharacter>();
        foreach (var p in players)
        {
            if (p.characterName == "Fighter") fighterHPText.text = $"HP: {p.currentHealth}";
            else if (p.characterName == "Healer") healerHPText.text = $"HP: {p.currentHealth}";
            else if (p.characterName == "Ranger") rangerHPText.text = $"HP: {p.currentHealth}";
        }

        var enemies = FindObjectsOfType<EnemyCharacter>();
        foreach (var e in enemies)
        {
            if (e.characterName == "Enemy 1") enemy1HPText.text = $"HP: {e.currentHealth}";
            else if (e.characterName == "Enemy 2") enemy2HPText.text = $"HP: {e.currentHealth}";
        }
    }

    private void ClearHighlightsAndCallbacks()
    {
        pendingAction = PendingAction.None;
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
