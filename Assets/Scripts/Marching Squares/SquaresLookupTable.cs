using UnityEngine;

public static class SquaresLookupTable
{
    public static readonly int IndiciesInvalidIndex = -1;

    public static readonly Vector2[,] VerticlesTable = new Vector2[16, 6]
    {
        { new Vector2(-1f, -1f), new Vector2(-1f, -1f), new Vector2(-1f, -1f), new Vector2(-1f, -1f), new Vector2(-1f, -1f), new Vector2(-1f, -1f) },
        { new Vector2(0.5f, 1f), new Vector2(0f, 1f), new Vector2(0f, 0.5f), new Vector2(-1f, -1f), new Vector2(-1f, -1f), new Vector2(-1f, -1f) },
        { new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(0.5f, 1f), new Vector2(-1f, -1f), new Vector2(-1f, -1f), new Vector2(-1f, -1f) },
        { new Vector2(0f, 0.5f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(-1f, -1f), new Vector2(-1f, -1f) },
        { new Vector2(1f, 0f), new Vector2(0.5f, 0f), new Vector2(1f, 0.5f), new Vector2(-1f, -1f), new Vector2(-1f, -1f), new Vector2(-1f, -1f) },
        { new Vector2(0f, 0.5f), new Vector2(0f, 1f), new Vector2(0.5f, 1f), new Vector2(1f, 0f), new Vector2(0.5f, 0f), new Vector2(1f, 0.5f) },
        { new Vector2(0.5f, 0f), new Vector2(0.5f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f), new Vector2(-1f, -1f), new Vector2(-1f, -1f) },
        
        { new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 0.5f), new Vector2(-1f, -1f) },
        { new Vector2(0f, 0.5f), new Vector2(0f, 0f), new Vector2(0.5f, 0f), new Vector2(-1f, -1f), new Vector2(-1f, -1f), new Vector2(-1f, -1f) },
        { new Vector2(0f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 1f), new Vector2(0f, 1f), new Vector2(-1f, -1f), new Vector2(-1f, -1f) },
        { new Vector2(0f, 0f), new Vector2(0f, 0.5f), new Vector2(0.5f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(1f, 0.5f) },
        { new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(0.5f, 0f), new Vector2(-1f, -1f) },
        { new Vector2(0f, 0f), new Vector2(1f, 0), new Vector2(1f, 0.5f), new Vector2(0f, 0.5f), new Vector2(-1f, -1f), new Vector2(-1f, -1f) },
        { new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(0.5f, 1f), new Vector2(1f, 0.5f), new Vector2(1f, 0f), new Vector2(-1f, -1f) },
        { new Vector2(1f, 1f), new Vector2(1f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0.5f), new Vector2(0.5f, 1f), new Vector2(-1f, -1f) },
        { new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f), new Vector2(-1f, -1f), new Vector2(-1f, -1f) }
    };

    public static readonly int[,] LinesTable = new int[16, 4]
    {
        { -1, -1, -1, -1 },
        { 0, 2, -1, -1 },
        { 2, 3, -1, -1 },
        { 0, 3, -1, -1 },
        { 1, 2, -1, -1 },
        { 0, 2, 4, 5 },
        { 0, 1, -1, -1 },
        { 3, 4, -1, -1 },
        { -1, -1, -1, -1 },
        { -1, -1, -1, -1 },
        { -1, -1, -1, -1 },
        { -1, -1, -1, -1 },
        { -1, -1, -1, -1 },
        { -1, -1, -1, -1 },
        { -1, -1, -1, -1 },
        { -1, -1, -1, -1 }
    };

    public static readonly int[,] IndiciesTable = new int[16, 9]
    {
        { -1, -1, -1, -1, -1, -1, -1, -1, -1 },
        { 0, 1, 2, -1, -1, -1, -1, -1, -1 },
        { 0, 1, 2, -1, -1, -1, -1, -1, -1 },
        { 0, 1, 2, 0, 2, 3, -1, -1, -1 },
        { 0, 1, 2, -1, -1, -1, -1, -1, -1 },
        { 0, 1, 2, 3, 4, 5, -1, -1, -1 },
        { 0, 1, 2, 0, 2, 3, -1, -1, -1 },
        { 2, 3, 1, 3, 4, 1, 4, 0, 1 },
        { 2, 1, 0, -1, -1, -1, -1, -1, -1 },
        { 1, 0, 2, 0, 3, 2, -1, -1, -1 },
        { 0, 1, 2, 5, 4, 3, -1, -1, -1 },
        { 0, 1, 2, 0, 2, 3, 4, 0, 3 },
        { 0, 3, 2, 0, 2, 1, -1, -1, -1 },
        { 0, 1, 2, 0, 2, 3, 0, 3, 4 },
        { 0, 1, 4, 1, 3, 4, 1, 2, 3 },
        { 0, 1, 2, 0, 2, 3, -1, -1, -1 }
    };
}
