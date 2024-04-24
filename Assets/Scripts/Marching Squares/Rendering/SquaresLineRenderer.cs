using CptLost.ObjectPool;
using System.Collections;
using UnityEngine;
using Zenject;

public class SquaresLineRenderer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer m_lineRenderer;
    [SerializeField]
    private bool m_applyLerp;

    [Inject]
    private MarchingSquares m_marchingSquares;
    private ComponentPool<LineRenderer> m_pool;
    private Coroutine m_refreshCoroutine;

    private void Awake()
    {
        m_pool = new ComponentPool<LineRenderer>(m_lineRenderer, transform);
    }

    private void OnEnable()
    {
        m_marchingSquares.OnAnyWeightUpdate += Regenerate;
    }

    private void OnDisable()
    {
        m_marchingSquares.OnAnyWeightUpdate -= Regenerate;
    }

    public void Generate()
    {
        m_pool.Reset();

        m_marchingSquares.LoopThroughtAllCells(
            (cellX, cellY) =>
            {
                float[,] squareWeights = m_marchingSquares.GetWeightsOfSquareVerticles(cellX, cellY);

                if (squareWeights == null)
                    return;

                int squareIndex = m_marchingSquares.CalculateSquareIndex(squareWeights);

                if (squareIndex == 0)
                    return;

                for (int i = 0; i < 2; i++)
                {
                    int lineA = SquaresLookupTable.LinesTable[squareIndex, 2 * i];
                    int lineB = SquaresLookupTable.LinesTable[squareIndex, 2 * i + 1];

                    if (lineA == -1 || lineB == -1)
                        continue;

                    LineRenderer line = m_pool.DequeueObject();
                    line.name = $" {cellX}|{cellY} [{squareIndex}]";

                    line.positionCount = 2;

                    Vector3 vertA = SquaresLookupTable.VerticlesTable[squareIndex, lineA];
                    Vector3 vertB = SquaresLookupTable.VerticlesTable[squareIndex, lineB];

                    if (m_applyLerp)
                    {
                        m_marchingSquares.ApplyLerpToVericle(ref vertA, squareWeights);
                        m_marchingSquares.ApplyLerpToVericle(ref vertB, squareWeights);
                    }

                    line.SetPosition(0, vertA + new Vector3(cellX, cellY));
                    line.SetPosition(1, vertB + new Vector3(cellX, cellY));
                }
            });
    }

    public void Regenerate()
    {
        if (m_refreshCoroutine != null)
            return;

        m_refreshCoroutine = StartCoroutine(RegenerateAtEndOfFrame());
    }

    private IEnumerator RegenerateAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        Generate();
        m_refreshCoroutine = null;
    }
}
