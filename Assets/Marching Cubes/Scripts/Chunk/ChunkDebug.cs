using UnityEngine;
using Zenject;

public class ChunkDebug : MonoBehaviour
{
    [Inject]
    private ChunkFacade m_chunkFacade;

    private void OnDrawGizmosSelected()
    {
        ChunkData chunkData = m_chunkFacade.ChunkData;

        for (int x = 0; x < chunkData.ChunkSize.x; x++)
        {
            for (int y = 0; y < chunkData.ChunkSize.y; y++)
            {
                for (int z = 0; z < chunkData.ChunkSize.z; z++)
                {
                    Vector3Int localPosition = new Vector3Int(x, y, z);

                    int index = CubesGridMetrics.CalculateIndex(x, y, z, chunkData.ChunkSize.x, chunkData.ChunkSize.y);

                    float noise = chunkData.NoiseWeights[index];

                    Gizmos.color = noise >= 0.5f ? Color.white : Color.black;
                    Gizmos.DrawCube(transform.position + localPosition, Vector3.one * 0.1f);
                }
            }
        }
    }
}
