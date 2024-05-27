using System.Diagnostics;
using UnityEngine;
using Zenject;

public class ChunkDebug : MonoBehaviour
{
    [SerializeField]
    private bool m_drawBoundary;
    [SerializeField]
    private Color m_boundaryColor;

    [Inject]
    private ChunkFacade m_chunkFacade;

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            return;

        ChunkData chunkData = m_chunkFacade.ChunkData;

        DrawBoundary(chunkData);
        DrawWeightsGizmos(chunkData);
    }

    private void DrawWeightsGizmos(ChunkData chunkData)
    {
        for (int x = 0; x < chunkData.ChunkSize.x; x++)
        {
            for (int y = 0; y < chunkData.ChunkSize.y; y++)
            {
                for (int z = 0; z < chunkData.ChunkSize.z; z++)
                {
                    Vector3Int localPosition = new Vector3Int(x, y, z);

                    int index = CubesGridMetrics.CalculateIndex(x, y, z, chunkData.ChunkSize.x, chunkData.ChunkSize.y);

                    if (index == 1)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireCube(transform.position + localPosition + Vector3.one * 0.5f, Vector3.one);
                    }

                    float noise = chunkData.NoiseWeights[index];

                    Gizmos.color = noise >= 0.5f ? Color.white : Color.black;
                    Gizmos.DrawCube(transform.position + localPosition, Vector3.one * 0.1f);
                }
            }
        }
    }

    private void DrawBoundary(ChunkData chunkData)
    {
        Vector3 boxSize = (Vector3)chunkData.ChunkSize - Vector3.one;

        Gizmos.color = m_boundaryColor;
        Gizmos.DrawWireCube(transform.position + boxSize * 0.5f, boxSize);
    }
}
