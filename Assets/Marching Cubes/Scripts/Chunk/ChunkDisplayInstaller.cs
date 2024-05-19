using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChunkInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.Bind<string>().FromInstance("Test");
    }
}

public class ChunkDisplayInstaller : MonoInstaller
{
    [SerializeField]
    private ChunkFacade m_chunkPrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<ChunkData, ChunkFacade, ChunkFacade.Factory>()
            .FromPoolableMemoryPool<ChunkData, ChunkFacade, ChunkFacade.Pool>(poolBinder => poolBinder
                //.FromComponentInNewPrefab(m_chunkPrefab));
                .FromSubContainerResolve()
                //.ByNewPrefabInstaller<ChunkInstaller>(m_chunkPrefab));
                .ByNewContextPrefab(m_chunkPrefab)
                .UnderTransformGroup("Chunks"));
    }
}