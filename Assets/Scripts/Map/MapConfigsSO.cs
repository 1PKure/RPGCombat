using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfigs", menuName = "MapConfigs")]
public class MapConfigsSO : ScriptableObject
{
    [SerializeField] public float GridCellSize = 0.48f;
    [SerializeField] public int GridWidth = 3;
    [SerializeField] public int GridHeight = 4;
    [SerializeField] public float ObstacleProbability = .2f;
    [SerializeField] public Vector2Int StartPosition = Vector2Int.zero;
}
