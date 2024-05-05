using System;
using UnityEngine;

[Serializable]
public class MarchingSquaresSettings
{
    [field: SerializeField]
    public Vector2Int AmountOfCells { get; private set; }
    [field: SerializeField]
    public float CellSize { get; private set; }
    [field: SerializeField]
    public float IsoLevel { get; private set; }
}
