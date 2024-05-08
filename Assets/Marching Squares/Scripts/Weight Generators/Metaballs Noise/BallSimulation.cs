using System;
using System.Collections.Generic;
using UnityEngine;

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
