using System;
using UnityEngine;
using Zenject;

public class ChunkFacade : MonoBehaviour, IPoolable<ChunkData, IMemoryPool>, IDisposable
{
    public Action OnChunkUpdate;

    public ChunkData ChunkData;

    private IMemoryPool m_pool;

    public void OnSpawned(ChunkData chunkData, IMemoryPool pool)
    {
        ChunkData = chunkData;
        m_pool = pool;

        transform.position = chunkData.WorldPosition;

        OnChunkUpdate?.Invoke();
    }

    public void OnDespawned()
    {
        m_pool = null;
    }

    public void Dispose()
    {
        m_pool.Despawn(this);
    }

    public class Factory : PlaceholderFactory<ChunkData, ChunkFacade> { }
    public class Pool : MonoPoolableMemoryPool<ChunkData, IMemoryPool, ChunkFacade> { }
}
