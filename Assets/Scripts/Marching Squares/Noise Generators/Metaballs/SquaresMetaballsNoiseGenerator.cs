using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Ball
{
    public Vector2 Position { get; private set; }
    public float Radius { get; private set; }
    public Vector2 Velocity { get; private set; }

    public Ball(Vector2 position, float radius, Vector2 velocity)
    {
        Position = position;
        Radius = radius;
        Velocity = velocity;
    }

    public void OnTick()
    {
        Position += Velocity * Time.deltaTime;
    }

    public void CheckBound(Vector2 simulationSize)
    {
        if (Position.x > simulationSize.x - Radius || Position.x < Radius)
        {
            float clampedX = Mathf.Clamp(Position.x, Radius, simulationSize.x - Radius);
            Position = new Vector2(clampedX, Position.y);

            Velocity = new Vector2(Velocity.x * -1f, Velocity.y);
        }

        if (Position.y > simulationSize.y - Radius || Position.y < Radius)
        {
            float clampedY = Mathf.Clamp(Position.y, Radius, simulationSize.y - Radius);
            Position = new Vector2(Position.x, clampedY);

            Velocity = new Vector2(Velocity.x, Velocity.y * -1f);
        }
    }
}

public class SquaresMetaballsNoiseGenerator : ISquaresNoiseGenerator
{
    public List<Ball> BallList = new List<Ball>();

    [Inject]
    private MarchingSquares m_marchingSquares;

    private SquaresMetaballsNoiseSettings m_settings;

    public SquaresMetaballsNoiseGenerator(SquaresMetaballsNoiseSettings settings)
    {
        m_settings = settings;
    }

    public void Initialize()
    {
        for (int i = 0; i < m_settings.AmountOfBalls; i++)
        {
            float posX = Random.Range(0f, m_marchingSquares.Settings.GridSize.x);
            float posY = Random.Range(0f, m_marchingSquares.Settings.GridSize.y);
            float radius = Random.Range(m_settings.MinBallRadius, m_settings.MaxBallRadius);

            float velX = Random.Range(m_settings.MinBallVelocityX, m_settings.MaxBallVelocityX);
            float velY = Random.Range(m_settings.MinBallVelocityY, m_settings.MaxBallVelocityY);

            Ball ball = new Ball(new Vector2(posX, posY), radius, new Vector2(velX, velY));

            BallDisplay display = Object.Instantiate(m_settings.BallDisplayTemplate);
            display.SetBall(ball);

            BallList.Add(ball);
        }
    }

    public void Tick()
    {
        m_marchingSquares.SquaresData.ClearWeights();

        m_marchingSquares.LoopThroughtAllCells(
            (cellX, cellY) =>
            {
                float sum = 0f;

                foreach (Ball ball in BallList)
                {
                    sum += (ball.Radius * ball.Radius) / ((cellX - ball.Position.x) * (cellX - ball.Position.x) + (cellY - ball.Position.y) * (cellY - ball.Position.y));
                }

                m_marchingSquares.SetWeight(cellX, cellY, sum);
            });

        foreach (Ball ball in BallList)
        {
            ball.OnTick();
            ball.CheckBound(m_marchingSquares.Settings.GridSize);
        }
    }

    public void Generate()
    {
        
    }
}
