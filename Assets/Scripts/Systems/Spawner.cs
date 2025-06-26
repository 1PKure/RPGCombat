using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs Jugadores")]
    [SerializeField] private GameObject playerFighter;
    [SerializeField] private GameObject playerHealer;
    [SerializeField] private GameObject playerRanger;

    [Header("Prefabs Enemigos")]
    [SerializeField] private GameObject enemy1;
    [SerializeField] private GameObject enemy2;

    [SerializeField] private MapView mapView;

    private List<Vector2Int> availablePositions;

    private void Start()
    {
        availablePositions = mapView.GetAvailablePositions();
    }

    public void SpawnCharacters()
    {
        availablePositions = new List<Vector2Int>(mapView.GetWalkablePositions());

        Spawn(playerFighter, "Fighter", 20, 2);
        Spawn(playerHealer, "Healer", 15, 2);
        Spawn(playerRanger, "Ranger", 15, 4);

        Spawn(enemy1, "Enemy1", 10, 1);
        Spawn(enemy2, "Enemy2", 10, 1);
    }

    private void Spawn(GameObject prefab, string name, int hp, int speed)
    {
        if (availablePositions.Count == 0) return;

        int index = Random.Range(0, availablePositions.Count);
        Vector2Int pos = availablePositions[index];
        availablePositions.RemoveAt(index);

        Vector3 worldPos = mapView.GetWorldPosition(pos);
        GameObject go = Instantiate(prefab, worldPos, Quaternion.identity);

        if (go.TryGetComponent(out CharacterBase character))
            character.Initialize(name, hp, speed, pos);
    }
}

