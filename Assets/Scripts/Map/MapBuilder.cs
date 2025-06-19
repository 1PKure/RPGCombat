using System.Collections.Generic;
using UnityEngine;

public static class MapBuilder
{
    public static List<List<TerrainType>> GenerateMap(int gridWidth, int gridHeight, float obstacleProbability, Vector2Int startPosition)
    {
        var map = new List<List<TerrainType>>();

        for (var width = 0; width < gridWidth; width++)
        {
            var row = new List<TerrainType>();
            for (var height = 0; height < gridHeight; height++)
            {
                if(Random.Range(0f, 1f) <= obstacleProbability)
                    row.Add(TerrainType.TREE);
                else
                    row.Add(TerrainType.GRASS);
            }
            map.Add(row);
        }
        map[startPosition.x][startPosition.y] = TerrainType.START;
        map[gridWidth-1][gridHeight-1] = TerrainType.FINISH;
            
        return map;
    }
}
