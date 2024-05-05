using System;

public class MarchingSquaresData
{
    public Action OnWeightUpdate;

    public float[] CellWeights { get; private set; }

    private int m_gridSizeX, m_gridSizeY;

    public MarchingSquaresData(int gridSizeX, int gridSizeY)
    {
        m_gridSizeX = gridSizeX;
        m_gridSizeY = gridSizeY;

        CellWeights = new float[gridSizeX * gridSizeY];
    }

    public float GetWeight(int cellX, int cellY)
    {
        int index = SquaresGridMetrics.CalculateIndex(cellX, cellY, m_gridSizeX, m_gridSizeY);

        return CellWeights[index];
    }

    public void SetWeight(int cellX, int cellY, float weight)
    {
        int index = SquaresGridMetrics.CalculateIndex(cellX, cellY, m_gridSizeX, m_gridSizeY);

        CellWeights[index] = weight;

        OnWeightUpdate?.Invoke();
    }

    public void ClearWeights()
    {
        Array.Clear(CellWeights, 0, CellWeights.Length);

        OnWeightUpdate?.Invoke();
    }
}
