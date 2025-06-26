using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] public MapView mapView;
    [SerializeField] private Spawner spawner;
    [SerializeField] public TurnManager turnManager;
    public UIManager UIManager;
    public GameState CurrentState { get; private set; } = GameState.Exploring;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        spawner.SpawnCharacters();
        turnManager.StartCombat();
    }
    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
    }

    public bool CanMove() => CurrentState == GameState.Exploring;
}
