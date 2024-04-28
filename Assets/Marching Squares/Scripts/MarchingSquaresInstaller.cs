using System;
using UnityEngine;
using Zenject;

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

public class MarchingSquaresInstaller : MonoInstaller
{
    [SerializeField]
    private MarchingSquaresSettings m_settings;

    public override void InstallBindings()
    {
        Container.Bind<MarchingSquares>()
            .AsSingle()
            .WithArguments(m_settings);
    }
}
