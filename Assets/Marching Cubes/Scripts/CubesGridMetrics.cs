using UnityEngine;

public static class CubesGridMetrics
{
    public static void CalculateCellPos(int index, int chunkSizeX, int chunkSizeY, out int cellX, out int cellY, out int cellZ)
    {
        cellX = index % chunkSizeX;
        cellY = (index / chunkSizeX) % chunkSizeY;
        cellZ = (index / chunkSizeX) / chunkSizeY;
    }

    public static int CalculateIndex(int cellX, int cellY, int cellZ, int chunkSizeX, int chunkSizeY)
    {
        return (cellZ * chunkSizeY + cellY) * chunkSizeX + cellX;
    }

    public static int CalculateIndex(int cellX, int cellY, int cellZ, Vector3Int chunkSize)
    {
        return (cellZ * chunkSize.y + cellY) * chunkSize.x + cellX;
    }

    public static int CalculateIndex(Vector3Int cellPosition, int chunkSizeX, int chunkSizeY)
    {
        return (cellPosition.z * chunkSizeY + cellPosition.y) * chunkSizeX + cellPosition.x;
    }

    public static int CalculateIndex(Vector3Int cellPosition, Vector3Int chunkSize)
    {
        return (cellPosition.z * chunkSize.y + cellPosition.y) * chunkSize.x + cellPosition.x;
    }
}
