using UnityEngine;
using Zenject;

public class SquaresPerlinNoiseGenerator : SquaresNoiseGenerator
{
    [Inject]
    private MarchingSquares m_marchingSquares;

    private SquaresPerlinNoiseSettings m_settings;

    public SquaresPerlinNoiseGenerator(SquaresPerlinNoiseSettings settings)
    {
        m_settings = settings;
    }

    public override void Initialize()
    {
        Generate();
    }

    public override void Generate()
    {
        m_marchingSquares.LoopThroughtAllCells((cellX, cellY) =>
        {
            float noisevalue = Mathf.PerlinNoise(cellX * m_settings.NoiseScale, cellY * m_settings.NoiseScale);

            m_marchingSquares.SetWeight(cellX, cellY, noisevalue);
        });
    }
}
