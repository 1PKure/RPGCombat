using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    private List<List<TerrainType>> map;
    public List<List<GameObject>> Grid { get; private set; }
    [SerializeField] public MapConfigsSO mapConfigs;
    [SerializeField] private Vector2 gridOrigin = new Vector2(-6f, 0f);

    private void Awake()
    {

        map = MapBuilder.GenerateMap(mapConfigs.GridWidth, mapConfigs.GridHeight, mapConfigs.ObstacleProbability, mapConfigs.StartPosition);
        InitializeMap(map);
    }

    public List<Vector2Int> GetWalkablePositions()
    {
        var walkable = new List<Vector2Int>();
        for (int row = 0; row < map.Count; row++)
            for (int col = 0; col < map[row].Count; col++)
                if (map[row][col] == TerrainType.GRASS)
                    walkable.Add(new Vector2Int(col, row));
        return walkable;
    }

    public void InitializeMap(List<List<TerrainType>> map)
    {
        Grid = new List<List<GameObject>>();

        for (var row = 0; row < map.Count; row++)
        {
            var gridRow = new List<GameObject>();
            for (var col = 0; col < map[row].Count; col++)
            {
                var terrainType = map[row][col];

                var gridCell = Instantiate(Resources.Load<GameObject>("Prefabs/" + terrainType), transform);
                gridCell.transform.position = new Vector3(
    gridOrigin.x + col * mapConfigs.GridCellSize,
    gridOrigin.y + row * mapConfigs.GridCellSize,
    1
);
                gridRow.Add(gridCell);
            }
            Grid.Add(gridRow);
        }

        AdjustCameraToMap();
    }

    public bool IsAValidPosition(Vector2Int posibleNewPosition)
    {
        if (!ThePositionExists(posibleNewPosition))
            return false;

        if (!PositionIsNotBlocked(posibleNewPosition))
            return false;

        if (IsPositionOccupied(posibleNewPosition))
            return false;

        return true;
    }

    private bool IsPositionOccupied(Vector2Int position)
    {
        foreach (var obj in Grid)
        {
            foreach (var cell in obj)
            {
                if (cell != null && cell.transform.position == GetWorldPosition(position))
                {
                    return true;
                }
            }
        }
        return false;
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
    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(
            gridOrigin.x + gridPosition.x * mapConfigs.GridCellSize,
            gridOrigin.y + gridPosition.y * mapConfigs.GridCellSize,
            0f
        );
    }

    public List<Vector2Int> GetAvailablePositions()
    {
        return GetWalkablePositions();
    }

    public void AdjustCameraToMap()
    {
        int cols = map[0].Count;
        int rows = map.Count;

        float aspect = (float)Screen.width / Screen.height;
        float targetSize = Mathf.Max(rows, cols / aspect) * mapConfigs.GridCellSize * 0.6f;

        Camera.main.orthographicSize = targetSize + 0.6f;
    }

}
