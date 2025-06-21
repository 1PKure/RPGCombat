using System.Collections.Generic;
using UnityEngine;

public static class MapBuilder
{
    public static List<List<TerrainType>> GenerateMap(int gridWidth, int gridHeight, float obstacleProbability, Vector2Int startPosition)
    {
        var map = new List<List<TerrainType>>();

        for (int row = 0; row < gridHeight; row++)
        {
            var mapRow = new List<TerrainType>();
            for (int col = 0; col < gridWidth; col++)
            {
                mapRow.Add(TerrainType.GRASS);
            }
            map.Add(mapRow);
        }

        return map;
    }
}
