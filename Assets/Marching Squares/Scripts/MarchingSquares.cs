using System;
using UnityEngine;

public class MarchingSquares
{
    public MarchingSquaresSettings Settings { get; private set; }
    public MarchingSquaresData Data { get; private set; }

    public Vector2 SimulationSize { get; private set; }

    public MarchingSquares(MarchingSquaresSettings settings)
    {
        Settings = settings;

        Data = new MarchingSquaresData(Settings.AmountOfCells.x, Settings.AmountOfCells.y);

        SimulationSize = CalculateSimulationSize();
    }

    public bool IsValidCell(int cellX, int cellY)
    {
        if (cellX < 0 || cellX >= Settings.AmountOfCells.x)
            return false;

        if (cellY < 0 || cellY >= Settings.AmountOfCells.y)
            return false;

        return true;
    }

    public Vector2 CalculateSimulationSize()
    {
        return new Vector2(
            Settings.AmountOfCells.x * Settings.CellSize - Settings.CellSize,
            Settings.AmountOfCells.y * Settings.CellSize - Settings.CellSize);
    }

    public Vector3 CalculateGridCenter()
    {
        return SimulationSize * 0.5f;
    }

    public Vector3 CalculateCellWorldPosition(int cellX, int cellY)
    {
        float x = cellX * (Settings.CellSize);
        float y = cellY * (Settings.CellSize);

        return new Vector3(x, y, 0f);
    }

    public bool GetWeightsOfSquare(int cellX, int cellY, float[,] squareWeights)
    {
        for (int xOffset = 0; xOffset < 2; xOffset++)
        {
            for (int yOffset = 0; yOffset < 2; yOffset++)
            {
                int positionX = cellX + xOffset;
                int positionY = cellY + yOffset;

                if (!IsValidCell(positionX, positionY))
                {
                    return false;
                }

                float weight = Data.GetWeight(positionX, positionY);

                squareWeights[xOffset, yOffset] = weight;
            }
        }

        return true;
    }

    public int CalculateSquareIndex(float[,] weightMap)
    {
        int index = 0;

        float isoLevel = Settings.IsoLevel;

        if (weightMap[0, 1] >= isoLevel)
        {
            index |= 1;
        }

        if (weightMap[1, 1] >= isoLevel)
        {
            index |= 2;
        }

        if (weightMap[1, 0] >= isoLevel)
        {
            index |= 4;
        }

        if (weightMap[0, 0] >= isoLevel)
        {
            index |= 8;
        }

        return index;
    }

    public bool IsCellWeightAboveIsoLevel(int cellX, int cellY)
    {
        return Data.GetWeight(cellX, cellY) >= Settings.IsoLevel;
    }

    public void LoopThroughtAllCells(Action<int, int> actionToPerform)
    {
        for (int x = 0; x < Settings.AmountOfCells.x; x++)
        {
            for (int y = 0; y < Settings.AmountOfCells.y; y++)
            {
                actionToPerform.Invoke(x, y);
            }
        }
    }
}
