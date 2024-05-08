using CptLost.ObjectPool;
using UnityEngine;

public class SquaresLineDisplay : BaseSquaresDisplay
{
    public struct SquareLineData
    {
        public static readonly int s_LineBufferSize = sizeof(float) * 2 * 4;

        public Vector2 PositionA;
        public Vector2 PositionB;
        public Vector2 PositionC;
        public Vector2 PositionD;
    }

    [SerializeField]
    private ComputeShader m_calculationShader;
    [SerializeField]
    private LineRenderer m_lineRendererTemplate;
    [SerializeField]
    private Gradient m_horizontalColor;
    [SerializeField]
    private bool m_applyInterpolation;

    private ComponentPool<LineRenderer> m_linePool;
    private SquareLineData[] m_lineData;

    private ComputeBuffer m_lineDataBuffer;
    private ComputeBuffer m_cellWeightsBuffer;

    private void Start()
    {
        m_lineData = new SquareLineData[m_marchingSquares.Settings.AmountOfCells.x * m_marchingSquares.Settings.AmountOfCells.y];
        m_linePool = new ComponentPool<LineRenderer>(m_lineRendererTemplate, transform);

        CreateBuffers();
    }

    private void OnDestroy()
    {
        ReleaseBuffers();
    }

    public override void Generate()
    {
        m_linePool.Reset();

        m_cellWeightsBuffer.SetData(m_marchingSquares.Data.CellWeights);

        m_calculationShader.SetBuffer(0, "LinesData", m_lineDataBuffer);
        m_calculationShader.SetBuffer(0, "CellWeightData", m_cellWeightsBuffer);

        m_calculationShader.SetInts("CellAmount", m_marchingSquares.Settings.AmountOfCells.x, m_marchingSquares.Settings.AmountOfCells.y);
        m_calculationShader.SetFloat("IsoLevel", m_marchingSquares.Settings.IsoLevel);
        m_calculationShader.SetBool("HasInterpolation", m_applyInterpolation);

        m_calculationShader.Dispatch(0, m_lineData.Length, 1, 1);

        m_lineDataBuffer.GetData(m_lineData);

        CreateLines();
    }

    private void CreateLines()
    {
        for (int i = 0; i < m_lineData.Length; i++)
        {
            SquareLineData lineData = m_lineData[i];

            int cellX, cellY;

            SquaresGridMetrics.CalculateCellPos(i,
                m_marchingSquares.Settings.AmountOfCells.x, m_marchingSquares.Settings.AmountOfCells.y,
                out cellX, out cellY);

            if (lineData.PositionA.x != -1f)
            {
                LineRenderer lineRenderer = m_linePool.DequeueObject();
                lineRenderer.positionCount = 2;

                Vector3 cellWorld = m_marchingSquares.CalculateCellWorldPosition(cellX, cellY);

                Vector3 positionA = cellWorld + (Vector3)lineData.PositionA;
                Vector3 positionB = cellWorld + (Vector3)lineData.PositionB;

                lineRenderer.SetPosition(0, positionA);
                lineRenderer.SetPosition(1, positionB);

                lineRenderer.startColor = CalculateLineColor(positionA);
                lineRenderer.endColor = CalculateLineColor(positionB);
            }

            if (lineData.PositionC.x != -1f)
            {
                LineRenderer lineRenderer = m_linePool.DequeueObject();
                lineRenderer.positionCount = 2;

                Vector3 cellWorld = m_marchingSquares.CalculateCellWorldPosition(cellX, cellY);

                Vector3 positionA = cellWorld + (Vector3)lineData.PositionC;
                Vector3 positionB = cellWorld + (Vector3)lineData.PositionD;

                lineRenderer.SetPosition(0, positionA);
                lineRenderer.SetPosition(1, positionB);

                lineRenderer.startColor = CalculateLineColor(positionA);
                lineRenderer.endColor = CalculateLineColor(positionB);
            }
        }
    }

    private Color CalculateLineColor(Vector3 position)
    {
        float xMidPoint = position.x / m_marchingSquares.SimulationSize.x;

        Color xColor = m_horizontalColor.Evaluate(xMidPoint);

        return xColor;
    }

    private void CreateBuffers()
    {
        m_lineDataBuffer = new ComputeBuffer(m_lineData.Length, SquareLineData.s_LineBufferSize);
        m_cellWeightsBuffer = new ComputeBuffer(m_marchingSquares.Data.CellWeights.Length, sizeof(float));
    }

    private void ReleaseBuffers()
    {
        m_lineDataBuffer.Release();
        m_cellWeightsBuffer.Release();
    }
}
