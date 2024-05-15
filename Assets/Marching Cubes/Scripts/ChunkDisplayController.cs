using CptLost.ObjectPool;
using UnityEngine;
using Zenject;

public class ChunkDisplayController : MonoBehaviour
{
    [SerializeField]
    private ChunkDisplay m_chunkRendererTemplate;

    [Inject]
    private ChunkedMarchingCubes m_marchingCubes;
    private ComponentPool<ChunkDisplay> m_rendererPool;

    private void Awake()
    {
        m_rendererPool = new ComponentPool<ChunkDisplay>(m_chunkRendererTemplate, transform);
    }

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
        ChunkDisplay chunkRenderer = m_rendererPool.DequeueObject();
        chunkRenderer.SetData(chunkData);

        chunkRenderer.transform.position = chunkData.ChunkIndex * chunkData.ChunkSize;
    }

    private void OnChunkRemoved(ChunkData chunkData)
    {

    }
}
