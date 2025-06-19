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
}
