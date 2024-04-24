using UnityEngine;
using Zenject;

public class SquaresPerlinNoiseGenerator : ISquaresNoiseGenerator
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
        m_marchingSquares.LoopThroughtAllCells((cellX, cellY) =>
        {
            float noisevalue = Mathf.PerlinNoise(cellX * m_settings.NoiseScale, cellY * m_settings.NoiseScale);

            m_marchingSquares.SetWeight(cellX, cellY, noisevalue);
        });

        m_marchingSquares.SetWeight(30, 49, 1f);
    }
}
