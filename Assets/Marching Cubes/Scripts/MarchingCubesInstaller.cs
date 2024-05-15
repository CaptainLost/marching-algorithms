using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public class MarchingCubesSettings
{
    public Vector3Int InitialAmountOfChunks;
    public Vector3Int ChunkSize;
}

public class MarchingCubesInstaller : MonoInstaller
{
    [SerializeField]
    private MarchingCubesSettings m_settings;

    public override void InstallBindings()
    {
        Container.Bind(typeof(ChunkedMarchingCubes), typeof(IInitializable))
            .To<ChunkedMarchingCubes>()
            .AsSingle()
            .WithArguments(m_settings);
    }
}
