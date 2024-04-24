using System;
using UnityEngine;
using Zenject;

public class MarchingSquares : ITickable
{
    public Action OnAnyWeightUpdate;

    public MarchingSquaresSettings Settings {  get; private set; }
    public MarchingSquaresData SquaresData { get; private set; }

    public MarchingSquares(MarchingSquaresSettings settings)
    {
        Settings = settings;
        SquaresData = new MarchingSquaresData(settings.GridSize.x, settings.GridSize.y);

        SquaresData.OnWeightUpdate += OnCellWeightUpdate;
    }

    ~MarchingSquares()
    {
        SquaresData.OnWeightUpdate -= OnCellWeightUpdate;
    }

    public void Tick()
    {
        
    }

    public void LoopThroughtAllCells(Action<int, int> actionToPerform)
    {
        for (int x = 0; x < Settings.GridSize.x; x++)
        {
            for (int y = 0; y < Settings.GridSize.y; y++)
            {
                actionToPerform.Invoke(x, y);
            }
        }
    }

    public Vector3 GetCellCenterWorld(Vector3Int cellPosition)
    {
        return Settings.SquaresGrid.GetCellCenterWorld(cellPosition);
    }

    public bool IsValidCell(int cellX, int cellY)
    {
        return SquaresData.IsValidCell(cellX, cellY);
    }

    public float GetWeight(int cellX, int cellY)
    {
        return SquaresData.GetWeight(cellX, cellY);
    }

    public void SetWeight(int cellX, int cellY, float newWeight)
    {
        SquaresData.SetWeight(cellX, cellY, newWeight);
    }

    public float[,] GetWeightsOfSquareVerticles(int cellX, int cellY)
    {
        float[,] squareWeights = new float[2, 2];

        for (int xOffset = 0; xOffset < 2; xOffset++)
        {
            for (int yOffset = 0; yOffset < 2; yOffset++)
            {
                int positionX = cellX + xOffset;
                int positionY = cellY + yOffset;

                if (!IsValidCell(positionX, positionY))
                {
                    //squareWeights[xOffset, yOffset] = GetWeight(cellX, cellY);

                    //continue;

                    return null;
                }

                float weight = GetWeight(positionX, positionY);

                squareWeights[xOffset, yOffset] = weight;
            }
        }

        return squareWeights;
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

    public void ApplyLerpToVericle(ref Vector3 verticle, float[,] squareWeights)
    {
        // In the lookup table, there is no case where x and y are equal to 0.5
        if (verticle.x == 0.5f)
        {
            int y = (int)verticle.y;

            float weightA = squareWeights[0, y];
            float weightB = squareWeights[1, y];
            float distance = (Settings.IsoLevel - weightA) / (weightB - weightA);

            verticle.x = Mathf.Lerp(0f, 1f, distance);
        }
        else if (verticle.y == 0.5f)
        {
            int x = (int)verticle.x;

            float weightA = squareWeights[x, 0];
            float weightB = squareWeights[x, 1];
            float distance = (Settings.IsoLevel - weightA) / (weightB - weightA);

            verticle.y = Mathf.Lerp(0f, 1f, distance);
        }
    }

    private void OnCellWeightUpdate()
    {
        OnAnyWeightUpdate?.Invoke();
    }
}