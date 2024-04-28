using UnityEngine;

public static class MarchingSquaresLookupTable
{
    public static int InvalidIndex = -1;

    public static Vector2[] SquareVerticles = new Vector2[8]
    {
        new Vector2(0f, 0f),    // 0
        new Vector2(0.5f, 0f),  // 1
        new Vector2(1f, 0f),    // 2
        new Vector2(1f, 0.5f),  // 3
        new Vector2(1f, 1f),    // 4
        new Vector2(0.5f, 1f),  // 5
        new Vector2(0f, 1f),    // 6
        new Vector2(0f, 0.5f)   // 7
    };

    public static int[,] LineConnections = new int[16, 4]
    {
        {-1, -1, -1, -1},   // 0
        {5, 7, -1, -1},     // 1
        {3, 5, -1, -1},     // 2
        {3, 7, -1, -1},     // 3
        {1, 3, -1, -1},     // 4
        {5, 7, 1, 3},       // 5
        {1, 5, -1, -1},     // 6
        {1, 7, -1, -1},     // 7
        {1, 7, -1, -1},     // 8
        {1, 5, -1, -1},     // 9
        {5, 7, 1, 3},       // 10
        {1, 3, -1, -1},     // 11
        {3, 7, -1, -1},     // 12
        {3, 5, -1, -1},     // 13
        {5, 7, -1, -1},     // 14
        {-1, -1, -1, -1}    // 15
    };
}
