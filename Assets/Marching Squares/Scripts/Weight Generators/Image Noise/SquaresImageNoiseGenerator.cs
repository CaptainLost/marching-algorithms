using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SquaresImageNoiseGenerator : ISquaresWeightGenerator
{
    [Inject]
    private MarchingSquares m_marchingSquares;

    private SquaresImageNoiseSettings m_settings;

    public SquaresImageNoiseGenerator(SquaresImageNoiseSettings settings)
    {
        m_settings = settings;
    }

    public void Generate()
    {
        for (int x = 0; x < m_settings.NoiseTexture.width; x++)
        {
            for (int y = 0; y < m_settings.NoiseTexture.height; y++)
            {
                m_marchingSquares.Data.SetWeight(x, y, m_settings.NoiseTexture.GetPixel(x, y).grayscale);
            }
        }
    }

    public void Initialize()
    {
        Generate();
    }

    public void Tick()
    {
        
    }
}
