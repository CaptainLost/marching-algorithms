#ifndef SQUARES_TABLE
#define SQUARES_TABLE

namespace SquaresLookupTable
{
    static const int InvalidIndex = -1;
    
    static const float2 SquareVerticles[8] =
    {
        float2(0, 0),   // 0
        float2(0.5, 0), // 1
        float2(1, 0),   // 2
        float2(1, 0.5), // 3
        float2(1, 1),   // 4
        float2(0.5, 1), // 5
        float2(0, 1),   // 6
        float2(0, 0.5)   // 7
    };
    
    static const int LineConnections[16][4] =
    {
        { -1, -1, -1, -1 }, // 0
        { 5, 7, -1, -1 },   // 1
        { 3, 5, -1, -1 },   // 2
        { 3, 7, -1, -1 },   // 3
        { 1, 3, -1, -1 },   // 4
        { 5, 7, 1, 3 },     // 5
        { 1, 5, -1, -1 },   // 6
        { 1, 7, -1, -1 },   // 7
        { 1, 7, -1, -1 },   // 8
        { 1, 5, -1, -1 },   // 9
        { 5, 7, 1, 3 },     // 10
        { 1, 3, -1, -1 },   // 11
        { 3, 7, -1, -1 },   // 12
        { 3, 5, -1, -1 },   // 13
        { 5, 7, -1, -1 },   // 14
        { -1, -1, -1, -1 }  // 15
    };
}

#endif