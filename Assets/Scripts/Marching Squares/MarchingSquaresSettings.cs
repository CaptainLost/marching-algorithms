using System;
using UnityEngine;

[Serializable]
public class MarchingSquaresSettings
{
    [field: SerializeField]
    public Grid SquaresGrid { get; private set; }
    [field: SerializeField]
    public Vector2Int GridSize { get; private set; }
    [field: SerializeField]
    public float IsoLevel { get; private set; }
}
