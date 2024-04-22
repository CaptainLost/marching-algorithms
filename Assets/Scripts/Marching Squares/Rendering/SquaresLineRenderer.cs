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
                if (cellY == 49)
                {
                    float x = 2f;
                }

                float[,] squareWeights = m_marchingSquares.GetWeightsOfSquareVerticles(cellX, cellY);

                int squareIndex = m_marchingSquares.CalculateSquareIndex(squareWeights);

                if (squareIndex == 0)
                    return;

                for (int i = 0; i < 2; i++)
                {
                    int lineA = SquaresLookupTable.LinesTable[squareIndex, 2 * i];
                    int lineB = SquaresLookupTable.LinesTable[squareIndex, 2 * i + 1];

                    if (lineA == -1 || lineB == -1)
                        continue;

                    LineRenderer line = Instantiate(m_lineRenderer, transform);
                    line.gameObject.SetActive(true);
                    line.name = $" {cellX}|{cellY} [{squareIndex}]";

                    line.positionCount = 2;

                    line.SetPosition(0, SquaresLookupTable.VerticlesTable[squareIndex, lineA] + new Vector2(cellX, cellY));
                    line.SetPosition(1, SquaresLookupTable.VerticlesTable[squareIndex, lineB] + new Vector2(cellX, cellY));
                }
            });
    }
}
