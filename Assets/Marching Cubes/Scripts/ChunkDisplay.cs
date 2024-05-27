using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChunkDisplay : MonoBehaviour
{
    [Inject]
    private ChunkFacade m_chunkFacade;
    public ChunkData ChunkData { get { return m_chunkFacade.ChunkData; } }

    [SerializeField]
    private MeshFilter m_meshFilter;

    private void Start()
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> inds = new List<int>();

        for (int x = 0; x < ChunkData.ChunkSize.x; x++)
        {
            for (int y = 0; y < ChunkData.ChunkSize.y; y++)
            {
                for (int z = 0; z < ChunkData.ChunkSize.z; z++)
                {
                    Vector3Int localPosition = new Vector3Int(x, y, z);

                    int index = CubesGridMetrics.CalculateIndex(x, y, z, ChunkData.ChunkSize.x + 1, ChunkData.ChunkSize.y + 1);

                    //float[] cubeValues = new float[8]
                    //{
                    //   ChunkData.GetLocalWeight(x, y, z + 1),
                    //   ChunkData.GetLocalWeight(x + 1, y, z + 1),
                    //   ChunkData.GetLocalWeight(x + 1, y, z),
                    //   ChunkData.GetLocalWeight(x, y, z),
                    //   ChunkData.GetLocalWeight(x, y + 1, z + 1),
                    //   ChunkData.GetLocalWeight(x + 1, y + 1, z + 1),
                    //   ChunkData.GetLocalWeight(x + 1, y + 1, z),
                    //   ChunkData.GetLocalWeight(x, y + 1, z)
                    //};

                    //Vector3Int worldPosition = ChunkData.ChunkIndex * ChunkData.ChunkSize + localPosition;

                    //float[] cubeValuesWorld = new float[8]
                    //{
                    //   ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x, worldPosition.y, worldPosition.z + 1),
                    //   ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x + 1, worldPosition.y, worldPosition.z + 1),
                    //   ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x + 1, worldPosition.y, worldPosition.z),
                    //   ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x, worldPosition.y, worldPosition.z),
                    //   ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x, worldPosition.y + 1, worldPosition.z + 1),
                    //   ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x + 1, worldPosition.y + 1, worldPosition.z + 1),
                    //   ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x + 1, worldPosition.y + 1, worldPosition.z),
                    //   ChunkData.OwningStorage.GetWeightFromWorldCell(worldPosition.x, worldPosition.y + 1, worldPosition.z)
                    //};

                    float[,,] weights = new float[2, 2, 2];

                    if (!GetGlobalWeights(localPosition, weights))
                        continue;

                    int cubeIndex = 0;
                    if (weights[0, 0, 1] < 0.5f) cubeIndex |= 1;
                    if (weights[1, 0, 1] < 0.5f) cubeIndex |= 2;
                    if (weights[1, 0, 0] < 0.5f) cubeIndex |= 4;
                    if (weights[0, 0, 0] < 0.5f) cubeIndex |= 8;
                    if (weights[0, 1, 1] < 0.5f) cubeIndex |= 16;
                    if (weights[1, 1, 1] < 0.5f) cubeIndex |= 32;
                    if (weights[1, 1, 0] < 0.5f) cubeIndex |= 64;
                    if (weights[0, 1, 0] < 0.5f) cubeIndex |= 128;

                    int[] edges = MarchingCubesTables.triTable[cubeIndex];

                    for (int i = 0; edges[i] != -1; i++)
                    {
                        int edge1 = MarchingCubesTables.edgeConnections[edges[i]][0];
                        int edge2 = MarchingCubesTables.edgeConnections[edges[i]][1];

                        Vector3 v1 = MarchingCubesTables.cubeCorners[edge1];
                        Vector3 v2 = MarchingCubesTables.cubeCorners[edge2];

                        Vector3 vertexPos = (v1 + v2) * 0.5f;

                        verts.Add(vertexPos + localPosition);
                        inds.Add(verts.Count - 1);
                    }
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        mesh.triangles = inds.ToArray();
        mesh.RecalculateNormals();

        m_meshFilter.sharedMesh = mesh;
    }

    private bool GetGlobalWeights(Vector3Int localCellPosition, float[,,] weights)
    {
        Vector3Int worldCellPosition = ChunkData.WorldPosition + localCellPosition;

        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                for (int z = 0; z < 2; z++)
                {
                    if (!ChunkData.OwningStorage.TryGetWeightFromWorldCell(
                        worldCellPosition.x + x,
                        worldCellPosition.y + y,
                        worldCellPosition.z + z,
                        out weights[x, y, z]))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    //private void OnDrawGizmos()
    //{
    //    for (int x = 0; x < ChunkData.ChunkSize.x; x++)
    //    {
    //        for (int y = 0; y < ChunkData.ChunkSize.y; y++)
    //        {
    //            for (int z = 0; z < ChunkData.ChunkSize.z; z++)
    //            {
    //                Vector3Int localPosition = new Vector3Int(x, y, z);

    //                int index = CubesGridMetrics.CalculateIndex(x, y, z, ChunkData.ChunkSize.x, ChunkData.ChunkSize.y);

    //                float noise = ChunkData.NoiseWeights[index];

    //                Gizmos.color = noise >= 0.5f ? Color.white : Color.black;
    //                Gizmos.DrawCube(transform.position + localPosition, Vector3.one * 0.1f);
    //            }
    //        }
    //    }
    //}
}
