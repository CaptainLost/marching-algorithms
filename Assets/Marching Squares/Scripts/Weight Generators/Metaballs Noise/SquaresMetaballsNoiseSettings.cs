using System;
using UnityEngine;

[Serializable]
public class SquaresMetaballsNoiseSettings
{
    [field: SerializeField]
    public int AmountOfBalls { get; private set; }
    [field: SerializeField]
    [field: MinMax(-20f, 20f)]
    public Vector2 VelocityXRange { get; private set; }
    [field: SerializeField]
    [field: MinMax(-20f, 20f)]
    public Vector2 VelocityYRange { get; private set; }
    [field: SerializeField]
    [field: MinMax(1f, 20f)]
    public Vector2 RadiusRange { get; private set; }
}
