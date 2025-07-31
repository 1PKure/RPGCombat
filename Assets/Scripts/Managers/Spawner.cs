using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs Jugadores")]
    [SerializeField] public GameObject playerFighter;
    [SerializeField] public GameObject playerHealer;
    [SerializeField] public GameObject playerRanger;

    [Header("Prefabs Enemigos")]
    [SerializeField] public GameObject enemy1;
    [SerializeField] public GameObject enemy2;

    [SerializeField] private MapView mapView;

    public void Spawn(GameObject prefab, Vector2Int gridPosition)
    {
        Vector3 worldPos = mapView.GetWorldPosition(gridPosition);
        GameObject go = Instantiate(prefab, worldPos, Quaternion.identity);

        if (go.TryGetComponent<PlayerCharacter>(out var player))
            player.Initialize(gridPosition);
        else if (go.TryGetComponent<EnemyCharacter>(out var enemy))
            enemy.Initialize(gridPosition);
    }

    public void SpawnAll()
    {
        List<Vector2Int> allPositions = new List<Vector2Int>();

        for (int x = 0; x < 6; x++) 
        {
            for (int y = 0; y < 4; y++) 
            {
                allPositions.Add(new Vector2Int(x, y));
            }
        }

        for (int i = allPositions.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector2Int temp = allPositions[i];
            allPositions[i] = allPositions[j];
            allPositions[j] = temp;
        }

        Vector2Int pos1 = allPositions[0];
        Vector2Int pos2 = allPositions[1];
        Vector2Int pos3 = allPositions[2];
        Vector2Int pos4 = allPositions[3];
        Vector2Int pos5 = allPositions[4];

        Spawn(playerFighter, pos1);
        Spawn(playerHealer, pos2);
        Spawn(playerRanger, pos3);

        Spawn(enemy1, pos4);
        Spawn(enemy2, pos5);
    }
}

