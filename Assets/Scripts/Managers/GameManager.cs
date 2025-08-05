using System.Collections;
using System.Collections.Generic;
using Terresquall;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] public MapView mapView;
    [SerializeField] private Spawner spawner;
    [SerializeField] public TurnManager turnManager;
    [SerializeField] public CombatManager combatManager;
    [SerializeField] private GameObject combatPanel;
    [SerializeField] private GameObject joystickGO;      
    [SerializeField] private GameObject endCanvas;
    public UIManager UIManager;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene s, LoadSceneMode m)
    {

        combatPanel.SetActive(true);
        joystickGO.SetActive(true);


        endCanvas.SetActive(false);
    }

    private void Start()
    {
        var map = MapBuilder.GenerateMap(
            mapView.mapConfigs.GridWidth,
            mapView.mapConfigs.GridHeight,
            mapView.mapConfigs.ObstacleProbability,
            mapView.mapConfigs.StartPosition
        );

        mapView.InitializeMap(map);
        spawner.SpawnAll();
        turnManager.StartCombat();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // Salir del juego en el ejecutable
#endif
    }
}
