using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public struct ChunkMeshTriangle
{
    public static int Size => sizeof(float) * 3 * 3;

    public Vector3 A, B, C;
}

public class ChunkMesh : MonoBehaviour
{
    [SerializeField]
    private MeshFilter m_meshFilter;
    [SerializeField]
    private ComputeShader m_meshComputerShader;

    [Inject]
    private ChunkFacade m_chunkFacade;
    [Inject]
    private MarchingCubesSettings m_settings;
    private ComputeBuffer m_weightBuffer;
    private ComputeBuffer m_triangleBuffer;
    private ComputeBuffer m_triangleCountBuffer;
    private Coroutine m_refreshCoroutine;

    private float[] m_weightHolder;

    private void Awake()
    {
        m_chunkFacade.OnChunkUpdate += Regenerate;

        CreateBuffers();
    }

    private void OnDestroy()
    {
        m_chunkFacade.OnChunkUpdate -= Regenerate;

        DestoryBuffers();
    }

    public void Regenerate()
    {
        if (m_refreshCoroutine != null)
            return;

        m_refreshCoroutine = StartCoroutine(RegenerateAtEndOfFrame());
    }

    private void GenerateNow()
    {
        GatherWeights();

        m_weightBuffer.SetData(m_weightHolder);
        m_meshComputerShader.SetBuffer(0, "WeightData", m_weightBuffer);
        m_meshComputerShader.SetBuffer(0, "TriangleData", m_triangleBuffer);

        m_triangleBuffer.SetCounterValue(0);

        m_meshComputerShader.Dispatch(0, m_settings.ChunkSize.x / 8, m_settings.ChunkSize.y / 8, m_settings.ChunkSize.z / 8);

        // Triangle buffer count
        int[] triangleCount = { 0 };
        ComputeBuffer.CopyCount(m_triangleBuffer, m_triangleCountBuffer, 0);
        m_triangleCountBuffer.GetData(triangleCount);

        Debug.Log(triangleCount[0]);

        ChunkMeshTriangle[] triangleData = new ChunkMeshTriangle[triangleCount[0]];
        m_triangleBuffer.GetData(triangleData);

        foreach (ChunkMeshTriangle t in triangleData)
        {
            Debug.Log($"{t.A} {t.B} {t.C}");
        }
    }

    private IEnumerator RegenerateAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        GenerateNow();
        m_refreshCoroutine = null;
    }

    private void GatherWeights()
    {
        for (int x = 0; x < m_settings.ChunkSize.x + 1; x++)
        {
            for (int y = 0; y < m_settings.ChunkSize.y + 1; y++)
            {
                for (int z = 0; z < m_settings.ChunkSize.z + 1; z++)
                {
                    Vector3Int localCellIndex = new Vector3Int(x, y, z);
                    Vector3Int worldCellIndex = m_chunkFacade.ChunkData.WorldPosition + localCellIndex;

                    int index = CubesGridMetrics.CalculateIndex(localCellIndex.x, localCellIndex.y, localCellIndex.z, m_settings.ChunkSize + Vector3Int.one);

                    if (m_chunkFacade.ChunkData.IsLocalCellValid(x, y, z))
                    {
                        m_weightHolder[index] = m_chunkFacade.ChunkData.GetLocalWeight(x, y, z);
                        continue;
                    }

                    if (m_chunkFacade.ChunkData.OwningStorage.TryGetWeightFromWorldCell(worldCellIndex, out float weight))
                    {
                        m_weightHolder[index] = weight;
                    }
                    else
                    {
                        m_weightHolder[index] = -1f;
                    }
                }
            }
        }
    }

    private void CreateBuffers()
    {
        int bufferLenght = (m_settings.ChunkSize.x + 1) * (m_settings.ChunkSize.y + 1) * (m_settings.ChunkSize.z + 1);
        int bufferLenght2 = (m_settings.ChunkSize.x) * (m_settings.ChunkSize.y) * (m_settings.ChunkSize.z);

        m_weightBuffer = new ComputeBuffer(bufferLenght, sizeof(float));
        m_weightHolder = new float[bufferLenght];

        m_triangleBuffer = new ComputeBuffer(5 * bufferLenght2, ChunkMeshTriangle.Size, ComputeBufferType.Append);
        m_triangleCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
    }

    private void DestoryBuffers()
    {
        m_weightBuffer.Release();
        m_triangleBuffer.Release();
        m_triangleCountBuffer.Release();
    }
}
