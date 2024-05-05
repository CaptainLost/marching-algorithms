using UnityEngine;
using Zenject;

public class SquaresPerlinNoiseGenerator : ISquaresWeightGenerator
{
    [Inject]
    private MarchingSquares m_marchingSquares;

    private SquaresPerlinNoiseSettings m_settings;

    private float m_xOffset = 0f;

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
        m_xOffset += Time.deltaTime;

        Generate();
    }

    public void Generate()
    {
        for (int x = 0; x < m_marchingSquares.Settings.AmountOfCells.x; x++)
        {
            for (int y = 0; y < m_marchingSquares.Settings.AmountOfCells.y; y++)
            {
                float noisevalue = Mathf.PerlinNoise(x * m_settings.NoiseScale + m_xOffset, y * m_settings.NoiseScale);

                m_marchingSquares.Data.SetWeight(x, y, noisevalue);
            }
        }
    }
}
