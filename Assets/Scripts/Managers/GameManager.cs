using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] public MapView mapView;
    [SerializeField] private Spawner spawner;
    [SerializeField] public TurnManager turnManager;
    [SerializeField] public CombatManager combatManager;
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
