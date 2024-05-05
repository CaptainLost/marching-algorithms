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

                lineRenderer.SetPosition(0, cellWorld + (Vector3)lineData.PositionA);
                lineRenderer.SetPosition(1, cellWorld + (Vector3)lineData.PositionB);
            }

            if (lineData.PositionC.x != -1f)
            {
                LineRenderer lineRenderer = m_linePool.DequeueObject();
                lineRenderer.positionCount = 2;

                Vector3 cellWorld = m_marchingSquares.CalculateCellWorldPosition(cellX, cellY);

                lineRenderer.SetPosition(0, cellWorld + (Vector3)lineData.PositionC);
                lineRenderer.SetPosition(1, cellWorld + (Vector3)lineData.PositionD);
            }
        }
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
