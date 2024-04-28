using UnityEngine;
using Zenject;

public class SquaresPerlinNoiseGenerator : ISquaresWeightGenerator
{
    [Inject]
    private MarchingSquares m_marchingSquares;

    private SquaresPerlinNoiseSettings m_settings;

    public SquaresPerlinNoiseGenerator(SquaresPerlinNoiseSettings settings)
    {
        m_settings = settings;
    }

    public void Initialize()
    {
        Generate();
    }

    public void Tick()
    {
        
    }

    public void Generate()
    {
        for (int x = 0; x < m_marchingSquares.Settings.AmountOfCells.x; x++)
        {
            for (int y = 0; y < m_marchingSquares.Settings.AmountOfCells.y; y++)
            {
                float noisevalue = Mathf.PerlinNoise(x * m_settings.NoiseScale, y * m_settings.NoiseScale);

                m_marchingSquares.Data.SetWeight(x, y, noisevalue);
            }
        }
    }
}
