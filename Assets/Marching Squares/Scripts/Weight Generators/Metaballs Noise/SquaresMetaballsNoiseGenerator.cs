using UnityEngine;
using Zenject;

public class SquaresMetaballsNoiseGenerator : ISquaresWeightGenerator
{
    public BallSimulation BallSimulation { get; private set; }

    [Inject]
    private MarchingSquares m_marchingSquares;

    private SquaresMetaballsNoiseSettings m_settings;

    public SquaresMetaballsNoiseGenerator(SquaresMetaballsNoiseSettings settings)
    {
        m_settings = settings;

        BallSimulation = new BallSimulation();
    }

    public void Initialize()
    {
        for (int i = 0; i < m_settings.AmountOfBalls; i++)
        {
            float velocityX = Random.Range(m_settings.VelocityXRange.x, m_settings.VelocityXRange.y);
            float velocityY = Random.Range(m_settings.VelocityYRange.x, m_settings.VelocityYRange.y);
            float radius = Random.Range(m_settings.RadiusRange.x, m_settings.RadiusRange.y);

            BallSimulation.AddBall(m_marchingSquares.SimulationSize * 0.5f, new Vector2(velocityX, velocityY), radius);
        }
    }

    public void Tick()
    {
        Generate();
    }

    public void Generate()
    {
        BallSimulation.OnTick(m_marchingSquares.SimulationSize);

        m_marchingSquares.Data.ClearWeights();

        m_marchingSquares.LoopThroughtAllCells(
            (cellX, cellY) =>
            {
                float sum = 0f;

                foreach (BallSimulation.Ball ball in BallSimulation.BallList)
                {
                    sum += (ball.Radius * ball.Radius) / ((cellX - ball.Position.x) * (cellX - ball.Position.x) + (cellY - ball.Position.y) * (cellY - ball.Position.y));
                }

                m_marchingSquares.Data.SetWeight(cellX, cellY, sum);
            });
    }
}
