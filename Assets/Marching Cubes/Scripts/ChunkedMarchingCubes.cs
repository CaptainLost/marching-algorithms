using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface INoiseGenerator
{
    float[] Generate(Vector3Int startPosition, Vector3Int cellToGenerate);
}

public class PerlinNoiseGenerator : INoiseGenerator
{
    private int m_totalGridHeight;

    public PerlinNoiseGenerator(int totalGridHeight)
    {
        m_totalGridHeight = totalGridHeight;
    }

    public float[] Generate(Vector3Int startCellWorldIndex, Vector3Int cellToGenerate)
    {
        float[] generatedNoise = new float[cellToGenerate.x * cellToGenerate.y * cellToGenerate.z];

        for (int x = 0; x < cellToGenerate.x; x++)
        {
            for (int z = 0; z < cellToGenerate.z; z++)
            {
                float noiseValue = Mathf.PerlinNoise((startCellWorldIndex.x + x) * 0.05f, (startCellWorldIndex.z + z) * 0.05f);
                int groundPosition = Mathf.RoundToInt(noiseValue * m_totalGridHeight);

                for (int y = 0; y < cellToGenerate.y; y++)
                {
                    int worldY = startCellWorldIndex.y + y;

                    int index = CubesGridMetrics.CalculateIndex(x, y, z, cellToGenerate.x, cellToGenerate.y);

                    generatedNoise[index] = worldY >= groundPosition ? 0f : 1f;
                }
            }
        }

        return generatedNoise;
    }
}

public class ChunkData
{
    public Vector3Int ChunkIndex { get; private set; }
    public Vector3Int ChunkSize { get; private set; }
    public ChunkStorage OwningStorage {  get; private set; }

    public float[] NoiseWeights { get; private set; }

    public Vector3Int WorldPosition => ChunkIndex * ChunkSize;

    public ChunkData(Vector3Int chunkIndex, Vector3Int chunkSize, ChunkStorage owningStorage)
    {
        ChunkIndex = chunkIndex;
        ChunkSize = chunkSize;
        OwningStorage = owningStorage;
    }

    public void SetWeights(float[] weights)
    {
        NoiseWeights = weights;
    }

    public float GetLocalWeight(int indexX, int indexY, int indexZ)
    {
        int weightIndex = CubesGridMetrics.CalculateIndex(indexX, indexY, indexZ, ChunkSize.x, ChunkSize.y);

        return NoiseWeights[weightIndex];
    }

    public bool TryGetLocalWeight(int indexX, int indexY, int indexZ, out float weight)
    {
        int weightIndex = CubesGridMetrics.CalculateIndex(indexX, indexY, indexZ, ChunkSize.x, ChunkSize.y);

        if (weightIndex >= NoiseWeights.Length)
        {
            weight = 0f;

            return false;
        }

        weight = NoiseWeights[weightIndex];

        return true;
    }
}

public class ChunkStorage
{
    public Action<ChunkData> OnChunkAdded;
    public Action<ChunkData> OnChunkRemoved;

    private Dictionary<Vector3Int, ChunkData> m_chunksData = new();
    private Vector3Int m_chunkSize;
    private INoiseGenerator m_noiseGenerator;

    public ChunkStorage(Vector3Int chunkSize, INoiseGenerator noiseGenerator)
    {
        m_chunkSize = chunkSize;
        m_noiseGenerator = noiseGenerator;
    }

    public bool AddChunk(Vector3Int chunkIndex)
    {
        if (m_chunksData.ContainsKey(chunkIndex))
            return false;

        ChunkData chunk = new ChunkData(chunkIndex, m_chunkSize, this);
        m_chunksData.Add(chunkIndex, chunk);

        float[] generatedWeights = m_noiseGenerator.Generate(chunkIndex * m_chunkSize, m_chunkSize);
        chunk.SetWeights(generatedWeights);

        OnChunkAdded?.Invoke(chunk);

        return true;
    }

    public Vector3Int GetChunkIndexFromWorldCell(Vector3Int worldCellPosition)
    {
        return new Vector3Int(
            Mathf.CeilToInt((worldCellPosition.x + 1) / (float)m_chunkSize.x) - 1,
            Mathf.CeilToInt((worldCellPosition.y + 1) / (float)m_chunkSize.y) - 1,
            Mathf.CeilToInt((worldCellPosition.z + 1) / (float)m_chunkSize.z) - 1);
    }

    public ChunkData GetChunkFromWorldCell(Vector3Int worldCellPosition)
    {
        Vector3Int owningChunkIndex = GetChunkIndexFromWorldCell(worldCellPosition);

        return m_chunksData[owningChunkIndex];
    }

    public bool TryGetChunkFromWorldCell(Vector3Int worldCellPosition, out ChunkData chunkData)
    {
        Vector3Int owningChunkIndex = GetChunkIndexFromWorldCell(worldCellPosition);

        return m_chunksData.TryGetValue(owningChunkIndex, out chunkData);
    }

    public float GetWeightFromWorldCell(Vector3Int worldCellPosition)
    {
        Vector3Int owningChunkIndex = GetChunkIndexFromWorldCell(worldCellPosition);

        ChunkData owningChunk = m_chunksData[owningChunkIndex];

        // Local space
        worldCellPosition -= owningChunkIndex * m_chunkSize;

        int weightIndex = CubesGridMetrics.CalculateIndex(worldCellPosition, m_chunkSize.x, m_chunkSize.y);

        return owningChunk.NoiseWeights[weightIndex];
    }

    public float GetWeightFromWorldCell(int worldCellX, int worldCellY, int worldCellZ)
    {
        return GetWeightFromWorldCell(new Vector3Int(worldCellX, worldCellY, worldCellZ));
    }

    public bool TryGetWeightFromWorldCell(Vector3Int worldCellPosition, out float weight)
    {
        Vector3Int owningChunkIndex = GetChunkIndexFromWorldCell(worldCellPosition);

        if (!m_chunksData.TryGetValue(owningChunkIndex, out ChunkData owningChunk))
        {
            weight = 0f;

            return false;
        }

        // Local space
        worldCellPosition -= owningChunkIndex * m_chunkSize;

        int weightIndex = CubesGridMetrics.CalculateIndex(worldCellPosition, m_chunkSize.x, m_chunkSize.y);

        weight = owningChunk.NoiseWeights[weightIndex];

        return true;
    }

    public bool TryGetWeightFromWorldCell(int worldCellX, int worldCellY, int worldCellZ, out float weight)
    {
        return TryGetWeightFromWorldCell(new Vector3Int(worldCellX, worldCellY, worldCellZ), out weight);
    }
}

public class ChunkedMarchingCubes : IMarchingCubes, IInitializable
{
    public ChunkStorage ChunkStorage { get; private set; }

    private MarchingCubesSettings m_settings;

    [Inject]
    public ChunkedMarchingCubes(MarchingCubesSettings settings)
    {
        m_settings = settings;

        ChunkStorage = new ChunkStorage(m_settings.ChunkSize, new PerlinNoiseGenerator(m_settings.WorldSize.y * m_settings.ChunkSize.y)); // TODO: Move noise gen to settings
    }

    public void Initialize()
    {
        for (int x = 0; x < m_settings.WorldSize.x; x++)
        {
            for (int y = 0; y < m_settings.WorldSize.y; y++)
            {
                for (int z = 0; z < m_settings.WorldSize.z; z++)
                {
                    Vector3Int chunkPosition = new Vector3Int(x, y, z);

                    ChunkStorage.AddChunk(chunkPosition);
                }
            }
        }
    }

    public bool IsValidCell(Vector3Int cell)
    {
        return true;
    }

    public void SetWeight(Vector3Int cell, float weight)
    {
        
    }

    public float GetGlobalWeight(Vector3Int cell)
    {
        return 0f;
    }

    public bool TryGetWeight(Vector3Int cell, out float weight)
    {
        weight = 1.0f;

        return false;
    }
}
