using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject combatPanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    [SerializeField] private Transform actionMarker;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button healButton;
    [SerializeField] private Button escapeButton;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float messageDuration = 2f;
    [SerializeField] private RectTransform fighterSlot;
    [SerializeField] private RectTransform healerSlot;
    [SerializeField] private RectTransform rangerSlot;

    private PlayerCharacter currentPlayer;
    private void Awake()
    {
        if (Instance != null)
        {
            //Destroy(gameObject);
            return;
        }

        Instance = this;
    }

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
        playerHealthText.text = $"Player HP: {value}";
    }

    public void ShowEnemyHealth(int value)
    {
        enemyHealthText.text = $"Enemy HP: {value}";
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
        actionMarker.gameObject.SetActive(true); // Fix: Use gameObject property to access SetActive method  

        attackButton.interactable = true;
        healButton.interactable = true;
        escapeButton.interactable = true;
        attackButton.onClick.RemoveAllListeners();
        healButton.onClick.RemoveAllListeners();
        escapeButton.onClick.RemoveAllListeners();

        attackButton.onClick.AddListener(() => DoAttack());
        healButton.onClick.AddListener(() => DoHeal());
        escapeButton.onClick.AddListener(() => EndTurn());

        if (player.characterName == "Fighter")
            ActiveMarker.Instance.SetUIIndicator(fighterSlot);
        else if (player.characterName == "Healer")
            ActiveMarker.Instance.SetUIIndicator(healerSlot);
        else if (player.characterName == "Ranger")
            ActiveMarker.Instance.SetUIIndicator(rangerSlot);
    }

    public void DoAttack()
    {
        EnemyCharacter[] enemies = FindObjectsOfType<EnemyCharacter>();
        if (enemies.Length == 0) return;

        EnemyCharacter closest = enemies[0];
        float minDist = Vector2Int.Distance(currentPlayer.gridPosition, closest.gridPosition);

        foreach (EnemyCharacter enemy in enemies)
        {
            float dist = Vector2Int.Distance(currentPlayer.gridPosition, enemy.gridPosition);
            if (dist < minDist)
            {
                closest = enemy;
                minDist = dist;
            }
        }

        CombatManager.Instance.ExecuteAttack(currentPlayer, closest);
    }


    public void DoHeal()
    {
        CombatManager.Instance.ExecuteHeal(currentPlayer, currentPlayer);
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
        actionMarker.gameObject.SetActive(false); // Fix: Use gameObject property to access SetActive method  
    }

    private void ClearHighlightsAndCallbacks()
    {
        foreach (var character in FindObjectsOfType<CharacterBase>())
        {
            Highlight(character, false);
            character.OnClickedToReceiveAction(null);
        }
    }
}
