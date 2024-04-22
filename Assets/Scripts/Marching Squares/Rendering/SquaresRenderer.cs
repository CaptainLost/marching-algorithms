using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SquaresRenderer : MonoBehaviour
{
    [SerializeField]
    private MeshFilter m_meshFilter;
    [SerializeField]
    private bool m_applyLerp;

    [Inject]
    private MarchingSquares m_marchingSquares;

    private void Start()
    {
        Generate();
    }

    [ContextMenu("Redraw")]
    private void Generate()
    {
        Mesh mesh = CalucalteMesh();

        m_meshFilter.mesh = mesh;
    }

    private Mesh CalucalteMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> verticiesList = new();
        List<int> indiciesList = new();

        m_marchingSquares.LoopThroughtAllCells(
            (cellX, cellY) =>
            {
                float[,] squareWeights = GetWeightsOfSquareVerticles(cellX, cellY);

                int squareIndex = CalculateSquareIndex(squareWeights);

                if (squareIndex == 0)
                    return;

                int indiciesCount = verticiesList.Count;

                for (int i = 0; i < 6; i++)
                {
                    Vector3 verticle = SquaresLookupTable.VerticlesTable[squareIndex, i];

                    if (verticle.x == -1f || verticle.y == -1f)
                        break;

                    if (m_applyLerp)
                    {
                        ApplyLerpToVericle(ref verticle, squareWeights);
                    }

                    verticle.x += cellX;
                    verticle.y += cellY;

                    verticiesList.Add(verticle);
                }

                for (int i = 0; i < 9; i++)
                {
                    int index = SquaresLookupTable.IndiciesTable[squareIndex, i];

                    if (index == SquaresLookupTable.IndiciesInvalidIndex)
                        break;

                    indiciesList.Add(indiciesCount + index);
                }
            });

        mesh.vertices = verticiesList.ToArray();
        mesh.triangles = indiciesList.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }

    private float[,] GetWeightsOfSquareVerticles(int cellX, int cellY)
    {
        float[,] squareWeights = new float[2, 2];

        for (int xOffset = 0; xOffset < 2; xOffset++)
        {
            for (int yOffset = 0; yOffset < 2; yOffset++)
            {
                int positionX = cellX + xOffset;
                int positionY = cellY + yOffset;

                if (!m_marchingSquares.IsValidCell(positionX, positionY))
                {
                    squareWeights[xOffset, yOffset] = m_marchingSquares.GetWeight(cellX, cellY);

                    continue;
                }

                float weight = m_marchingSquares.GetWeight(positionX, positionY);

                squareWeights[xOffset, yOffset] = weight;
            }
        }

        return squareWeights;
    }

    private int CalculateSquareIndex(float[,] weightMap)
    {
        int index = 0;

        float isoLevel = m_marchingSquares.Settings.IsoLevel;

        if (weightMap[0, 1] >= isoLevel)
        {
            index |= 1;
        }

        if (weightMap[1, 1] >= isoLevel)
        {
            index |= 2;
        }

        if (weightMap[1, 0] >= isoLevel)
        {
            index |= 4;
        }

        if (weightMap[0, 0] >= isoLevel)
        {
            index |= 8;
        }

        return index;
    }

    private void ApplyLerpToVericle(ref Vector3 verticle, float[,] squareWeights)
    {
        // In the lookup table, there is no case where x and y are equal to 0.5
        if (verticle.x == 0.5f)
        {
            int y = (int)verticle.y;

            float weightA = squareWeights[0, y];
            float weightB = squareWeights[1, y];
            float distance = (m_marchingSquares.Settings.IsoLevel - weightA) / (weightB - weightA);

            verticle.x = Mathf.Lerp(0f, 1f, distance);
        }
        else if (verticle.y == 0.5f)
        {
            int x = (int)verticle.x;

            float weightA = squareWeights[x, 0];
            float weightB = squareWeights[x, 1];
            float distance = (m_marchingSquares.Settings.IsoLevel - weightA) / (weightB - weightA);

            verticle.y = Mathf.Lerp(0f, 1f, distance);
        }
    }
}
