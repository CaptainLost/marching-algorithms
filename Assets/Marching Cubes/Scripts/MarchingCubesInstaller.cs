using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public class MarchingCubesSettings
{
    public Vector3Int WorldSize;
    public Vector3Int ChunkSize;
}

public class MarchingCubesInstaller : MonoInstaller
{
    [SerializeField]
    private MarchingCubesSettings m_settings;

    public override void InstallBindings()
    {
        Container.Bind<MarchingCubesSettings>()
            .FromInstance(m_settings)
            .AsSingle();

        Container.Bind(typeof(ChunkedMarchingCubes), typeof(IInitializable))
            .To<ChunkedMarchingCubes>()
            .AsSingle();
    }
}
