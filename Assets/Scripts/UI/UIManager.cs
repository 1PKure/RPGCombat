using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public void ShowActionsFor(PlayerCharacter player)
    {
        currentPlayer = player;
        actionPanel.SetActive(true);

        // Activar/desactivar botones según el tipo
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
        Debug.Log($"{currentPlayer.characterName} attacks!");
        EndTurn();
    }

    private void DoHeal()
    {
        Debug.Log($"{currentPlayer.characterName} heals!");
        EndTurn();
    }

    private void EndTurn()
    {
        actionPanel.SetActive(false);
        currentPlayer.EndPlayerTurn();
        GameManager.Instance.turnManager.EndCurrentPlayerTurn();
    }
}
