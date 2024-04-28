using CptLost.ObjectPool;
using System.Collections;
using UnityEngine;
using Zenject;

public class SquaresLineDisplay : MonoBehaviour
{
    [SerializeField]
    private LineRenderer m_lineRendererTemplate;

    [Inject]
    private MarchingSquares m_marchingSquares;

    private ComponentPool<LineRenderer> m_linePool;
    private Coroutine m_refreshCoroutine;
    private float[,] m_squareWeightsCache;

    private void Start()
    {
        m_linePool = new ComponentPool<LineRenderer>(m_lineRendererTemplate, transform);

        m_squareWeightsCache = new float[2, 2];
    }

    private void OnEnable()
    {
        m_marchingSquares.Data.OnWeightUpdate += OnWeightUpdate;
    }

    private void OnDisable()
    {
        m_marchingSquares.Data.OnWeightUpdate -= OnWeightUpdate;
    }

    public void Generate()
    {
        m_linePool.Reset();

        m_marchingSquares.LoopThroughtAllCells(RenderLineForText);
    }

    public void Regenerate()
    {
        if (m_refreshCoroutine != null)
            return;

        m_refreshCoroutine = StartCoroutine(RegenerateAtEndOfFrame());
    }

    private void OnWeightUpdate()
    {
        Regenerate();
    }

    private IEnumerator RegenerateAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        Generate();
        m_refreshCoroutine = null;
    }

    private void RenderLineForText(int cellX, int cellY)
    {
        if (!m_marchingSquares.GetWeightsOfSquare(cellX, cellY, m_squareWeightsCache))
            return;

        LineRenderer lineRenderer = m_linePool.DequeueObject();
        lineRenderer.positionCount = 2;

        int squareIndex = m_marchingSquares.CalculateSquareIndex(m_squareWeightsCache);
        lineRenderer.name = squareIndex.ToString();

        for (int i = 0; i < 2; i++)
        {
            int lineA = MarchingSquaresLookupTable.LineConnections[squareIndex, 2 * i];
            int lineB = MarchingSquaresLookupTable.LineConnections[squareIndex, 2 * i + 1];

            if (lineA == MarchingSquaresLookupTable.InvalidIndex || lineB == MarchingSquaresLookupTable.InvalidIndex)
                return;

            Vector3 vertA = MarchingSquaresLookupTable.SquareVerticles[lineA];
            Vector3 vertB = MarchingSquaresLookupTable.SquareVerticles[lineB];

            Vector3 cellWorld = m_marchingSquares.CalculateCellWorldPosition(cellX, cellY);

            lineRenderer.SetPosition(0, vertA + cellWorld);
            lineRenderer.SetPosition(1, vertB + cellWorld);
        }
    }
}
