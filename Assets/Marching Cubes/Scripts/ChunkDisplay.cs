using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkDisplay : MonoBehaviour
{
    [SerializeField]
    private ComputeShader m_chunkDisplayShader;

    public ChunkData ChunkData { get; private set; }

    public void SetData(ChunkData chunkData)
    {
        ChunkData = chunkData;
    }

    private void Start()
    {
        for (int x = 0; x < ChunkData.ChunkSize.x - 1; x++)
        {
            for (int y = 0; y < ChunkData.ChunkSize.y - 1; y++)
            {
                for (int z = 0; z < ChunkData.ChunkSize.z - 1; z++)
                {
                    Vector3Int localPosition = new Vector3Int(x, y, z);
                    Vector3Int worldPosition = ChunkData.ChunkIndex * ChunkData.ChunkSize + localPosition;

                    float[] cubeValues = new float[8]
                    {
                       ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x, worldPosition.y, worldPosition.z + 1),
                       ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x + 1, worldPosition.y, worldPosition.z + 1),
                       ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x + 1, worldPosition.y, worldPosition.z),
                       ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x, worldPosition.y, worldPosition.z),
                       ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x, worldPosition.y + 1, worldPosition.z + 1),
                       ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x + 1, worldPosition.y + 1, worldPosition.z + 1),
                       ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x + 1, worldPosition.y + 1, worldPosition.z),
                       ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x, worldPosition.y + 1, worldPosition.z)
                    };

                    int cubeIndex = 0;
                    if (cubeValues[0] < 0.5f) cubeIndex |= 1;
                    if (cubeValues[1] < 0.5f) cubeIndex |= 2;
                    if (cubeValues[2] < 0.5f) cubeIndex |= 4;
                    if (cubeValues[3] < 0.5f) cubeIndex |= 8;
                    if (cubeValues[4] < 0.5f) cubeIndex |= 16;
                    if (cubeValues[5] < 0.5f) cubeIndex |= 32;
                    if (cubeValues[6] < 0.5f) cubeIndex |= 64;
                    if (cubeValues[7] < 0.5f) cubeIndex |= 128;

                    Debug.Log($"{cubeIndex}");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < ChunkData.ChunkSize.x; x++)
        {
            for (int y = 0; y < ChunkData.ChunkSize.y; y++)
            {
                for (int z = 0; z < ChunkData.ChunkSize.z; z++)
                {
                    Vector3Int localPosition = new Vector3Int(x, y, z);

                    int index = CubesGridMetrics.CalculateIndex(x, y, z, ChunkData.ChunkSize.x, ChunkData.ChunkSize.y);

                    float noise = ChunkData.NoiseWeights[index];

                    Gizmos.color = noise >= 0.5f ? Color.white : Color.black;
                    Gizmos.DrawCube(transform.position + localPosition, Vector3.one * 0.1f);
                }
            }
        }
    }
}
