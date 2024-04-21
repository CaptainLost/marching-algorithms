using System;
using UnityEngine;
using Zenject;

public class MarchingSquares : ITickable
{
    public MarchingSquaresSettings Settings {  get; private set; }
    public MarchingSquaresData SquaresData { get; private set; }

    public MarchingSquares(MarchingSquaresSettings settings)
    {
        Settings = settings;

        SquaresData = new MarchingSquaresData(settings.GridSize.x, settings.GridSize.y);
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

    // Might be usefull for chunks in future
    public float GetWeight(int cellX, int cellY)
    {
        return SquaresData.GetWeight(cellX, cellY);
    }

    public void SetWeight(int cellX, int cellY, float newWeight)
    {
        SquaresData.SetWeight(cellX, cellY, newWeight);
    }
}