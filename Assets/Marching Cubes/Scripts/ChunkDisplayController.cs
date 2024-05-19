using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChunkDisplayController : MonoBehaviour
{
    [Inject]
    private ChunkedMarchingCubes m_marchingCubes;
    [Inject]
    private ChunkFacade.Factory m_chunkFactory;

    private Dictionary<ChunkData, ChunkFacade> m_chunkDataBind = new();

    private void OnEnable()
    {
        m_marchingCubes.ChunkStorage.OnChunkAdded += OnChunkAdded;
        m_marchingCubes.ChunkStorage.OnChunkRemoved += OnChunkRemoved;
    }

    private void OnDisable()
    {
        m_marchingCubes.ChunkStorage.OnChunkAdded -= OnChunkAdded;
        m_marchingCubes.ChunkStorage.OnChunkRemoved -= OnChunkRemoved;
    }

    private void OnChunkAdded(ChunkData chunkData)
    {
        if (m_chunkDataBind.ContainsKey(chunkData))
            return;

        ChunkFacade createdChunk = m_chunkFactory.Create(chunkData);
        createdChunk.name = chunkData.ChunkIndex.ToString();

        m_chunkDataBind.Add(chunkData, createdChunk);
    }

    private void OnChunkRemoved(ChunkData chunkData)
    {
        if (m_chunkDataBind.TryGetValue(chunkData, out ChunkFacade chunk))
        {
            chunk.Dispose();
        }
    }
}
