public static class SquaresGridMetrics
{
    public static void CalculateCellPos(int index, int gridSizeX, int gridSizeY, out int cellX, out int cellY)
    {
        cellX = index % gridSizeX;
        cellY = index / gridSizeX;
    }

    public static int CalculateIndex(int cellX, int cellY, int gridSizeX, int gridSizeY)
    {
        return cellX + (cellY * gridSizeX);
    }
}
