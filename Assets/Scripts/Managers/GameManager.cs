// Assets/Scripts/GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public MapView mapView { get; private set; }
    public Spawner spawner { get; private set; }
    public TurnManager turnManager { get; private set; }
    public CombatManager combatManager { get; private set; }
    public UIManager UIManager { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        CacheSceneReferences();
        InitGame();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
 
        CacheSceneReferences();
        InitGame();
    }

    private void CacheSceneReferences()
    {
 
        mapView = FindObjectOfType<MapView>();
        spawner = FindObjectOfType<Spawner>();
        turnManager = FindObjectOfType<TurnManager>();
        combatManager = FindObjectOfType<CombatManager>();
        UIManager = FindObjectOfType<UIManager>();
    }

    private void InitGame()
    {
        Time.timeScale = 1f;
        UIManager.SetupUI();

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
        Application.Quit();
#endif
    }
}
