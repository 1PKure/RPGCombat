using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    private List<List<TerrainType>> map;
    public List<List<GameObject>> Grid { get; private set; }

    [SerializeField] private MapConfigsSO mapConfigs;

    private void Awake()
    {
        map = MapBuilder.GenerateMap(mapConfigs.GridWidth, mapConfigs.GridHeight, mapConfigs.ObstacleProbability, mapConfigs.StartPosition);
        InitializeMap(map);
    }

    public void InitializeMap(List<List<TerrainType>> map)
    {
        Grid = new List<List<GameObject>>();

        for (var row = 0; row < map.Count; row++)
        {
            var gridRow = new List<GameObject>();
            for (var column = 0; column < map[row].Count; column++)
            {
                var terrainType = map[row][column];

                var gridCell = Instantiate(Resources.Load<GameObject>("Prefabs/" + terrainType), transform);
                gridCell.transform.localPosition = new Vector3(column * mapConfigs.GridCellSize, row * mapConfigs.GridCellSize, 1);
                gridRow.Add(gridCell);
            }
            Grid.Add(gridRow);
        }
    }

    public bool IsWinningCell(Vector2Int characterPosition)
    {
        return map[characterPosition.y][characterPosition.x] == TerrainType.FINISH;
    }

    public bool IsAValidPosition(Vector2Int posibleNewPosition)
    {
        return ThePositionExists(posibleNewPosition) && PositionIsNotBlocked(posibleNewPosition);
    }

    private bool PositionIsNotBlocked(Vector2Int posibleNewPosition)
    {
        return map[posibleNewPosition.y][posibleNewPosition.x] != TerrainType.TREE;
    }

    private bool ThePositionExists(Vector2Int posibleNewPosition)
    {
        return posibleNewPosition.x >= 0
            && posibleNewPosition.y >= 0
            && posibleNewPosition.y < map.Count
            && posibleNewPosition.x < map[posibleNewPosition.y].Count;
    }
}
