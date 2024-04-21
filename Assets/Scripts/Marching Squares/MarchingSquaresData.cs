public class MarchingSquaresData
{
    private int m_gridSizeX, m_gridSizeY;
    private float[,] m_gridWeights;

    public MarchingSquaresData(int xGridSize, int yGridSize)
    {
        m_gridSizeX = xGridSize;
        m_gridSizeY = yGridSize;

        m_gridWeights = new float[xGridSize, yGridSize];
    }

    public bool IsValidCell(int cellX, int cellY)
    {
        if (cellX < 0 || cellX >= m_gridSizeX)
            return false;

        if (cellY < 0 || cellY >= m_gridSizeY)
            return false;

        return true;
    }

    public float GetWeight(int cellX, int cellY)
    {
        return m_gridWeights[cellX, cellY];
    }

    public void SetWeight(int cellX, int cellY, float newWeight)
    {
        m_gridWeights[cellX, cellY] = newWeight;
    }
}
