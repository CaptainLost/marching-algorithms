using System;
using UnityEngine;
using Zenject;

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
    private float[] m_weightHolder;

    private void Awake()
    {
        m_chunkFacade.OnChunkUpdate += Generate;

        CreateBuffers();
    }

    private void OnDestroy()
    {
        m_chunkFacade.OnChunkUpdate -= Generate;

        DestoryBuffers();
    }

    private void Generate()
    {
        GatherWeights();
    }

    private void GatherWeights()
    {
        for (int i = 0; i < m_weightHolder.Length; i++)
        {
            m_weightHolder[i] = -1;
        }

        Array.Copy(m_chunkFacade.ChunkData.NoiseWeights, m_weightHolder, m_chunkFacade.ChunkData.NoiseWeights.Length);

        for (int i = 0;i < m_weightHolder.Length; i++)
        {

        }

        foreach (var item in m_weightHolder)
        {
            Debug.Log(item);
        }
    }

    private void CreateBuffers()
    {
        int bufferLenght = (m_settings.ChunkSize.x + 1) * (m_settings.ChunkSize.y + 1) * (m_settings.ChunkSize.z + 1);

        m_weightBuffer = new ComputeBuffer(bufferLenght, sizeof(float));
        m_weightHolder = new float[bufferLenght];
    }

    private void DestoryBuffers()
    {
        m_weightBuffer.Release();
    }
}
