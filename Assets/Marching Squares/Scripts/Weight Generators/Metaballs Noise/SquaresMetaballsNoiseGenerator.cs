using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static BallSimulation;

public class BallSimulation
{
    public class Ball
    {
        public Ball(Vector2 position, Vector2 velocity, float radius)
        {
            Position = position;
            Velocity = velocity;
            Radius = radius;
        }

        public Vector2 Position;
        public Vector2 Velocity;
        public float Radius;
    }

    public List<Ball> BallList { get; private set; } = new List<Ball>();

    public Action<Ball> OnBallAdded;
    public Action<Ball> OnBallRemoved;

    public void OnTick(Vector2 simulationSize)
    {
        foreach (Ball ball in BallList)
        {
            ball.Position += ball.Velocity * Time.deltaTime;

            if (ball.Position.x > simulationSize.x - ball.Radius || ball.Position.x < ball.Radius)
            {
                float clampedX = Mathf.Clamp(ball.Position.x, ball.Radius, simulationSize.x - ball.Radius);
                ball.Position = new Vector2(clampedX, ball.Position.y);

                ball.Velocity = new Vector2(ball.Velocity.x * -1f, ball.Velocity.y);
            }

            if (ball.Position.y > simulationSize.y - ball.Radius || ball.Position.y < ball.Radius)
            {
                float clampedY = Mathf.Clamp(ball.Position.y, ball.Radius, simulationSize.y - ball.Radius);
                ball.Position = new Vector2(ball.Position.x, clampedY);

                ball.Velocity = new Vector2(ball.Velocity.x, ball.Velocity.y * -1f);
            }
        }
    }

    public void AddBall(Vector2 position, Vector2 velocity, float radius)
    {
        Ball ball = new Ball(position, velocity, radius);

        BallList.Add(ball);

        OnBallAdded?.Invoke(ball);
    }

    public void RemoveBall()
    {
        if (BallList.Count == 0)
            return;

        int indexToRemove = BallList.Count - 1;

        OnBallRemoved?.Invoke(BallList[indexToRemove]);

        BallList.RemoveAt(BallList.Count - 1);
    }
}

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
            float velocityX = UnityEngine.Random.Range(m_settings.VelocityXRange.x, m_settings.VelocityXRange.y);
            float velocityY = UnityEngine.Random.Range(m_settings.VelocityYRange.x, m_settings.VelocityYRange.y);
            float radius = UnityEngine.Random.Range(m_settings.RadiusRange.x, m_settings.RadiusRange.y);

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

                foreach (Ball ball in BallSimulation.BallList)
                {
                    sum += (ball.Radius * ball.Radius) / ((cellX - ball.Position.x) * (cellX - ball.Position.x) + (cellY - ball.Position.y) * (cellY - ball.Position.y));
                }

                m_marchingSquares.Data.SetWeight(cellX, cellY, sum);
            });
    }
}
