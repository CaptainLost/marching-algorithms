using UnityEngine;
using Zenject;

public class SquaresLineRenderer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer m_lineRenderer;

    [Inject]
    private MarchingSquares m_marchingSquares;

    private void Start()
    {
        m_marchingSquares.LoopThroughtAllCells(
            (cellX, cellY) =>
            {
                float[,] squareWeights = GetWeightsOfSquareVerticles(cellX, cellY);

                int squareIndex = CalculateSquareIndex(squareWeights);

                if (squareIndex == 0)
                    return;

                int lineA = SquaresLookupTable.LinesTable[squareIndex, 0];
                int lineB = SquaresLookupTable.LinesTable[squareIndex, 1];
                int lineC = SquaresLookupTable.LinesTable[squareIndex, 2];
                int lineD = SquaresLookupTable.LinesTable[squareIndex, 3];

                if (lineA != -1)
                {
                    LineRenderer line = Instantiate(m_lineRenderer, transform);
                    line.gameObject.SetActive(true);
                    line.name = $"{squareIndex}";

                    m_lineRenderer.positionCount = 2;

                    m_lineRenderer.SetPosition(0, SquaresLookupTable.VerticlesTable[squareIndex, lineA] + new Vector2(cellX, cellY));
                    m_lineRenderer.SetPosition(1, SquaresLookupTable.VerticlesTable[squareIndex, lineB] + new Vector2(cellX, cellY));
                }

                if (lineC != -1)
                {
                    LineRenderer line = Instantiate(m_lineRenderer, transform);
                    line.gameObject.SetActive(true);
                    line.name = $"{squareIndex}";

                    m_lineRenderer.positionCount = 2;

                    m_lineRenderer.SetPosition(0, SquaresLookupTable.VerticlesTable[squareIndex, lineC] + new Vector2(cellX, cellY));
                    m_lineRenderer.SetPosition(1, SquaresLookupTable.VerticlesTable[squareIndex, lineD] + new Vector2(cellX, cellY));
                }
            });
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
}
