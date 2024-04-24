using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDisplay : MonoBehaviour
{
    public Ball _ball;

    private void Update()
    {
        Vector3 position = new Vector3(_ball.Position.x, _ball.Position.y, 0f);

        transform.position = position;
    }

    public void SetBall(Ball ball)
    {
        _ball = ball;

        Vector3 scale = new Vector3(_ball.Radius * 2f, _ball.Radius * 2f, 1f);
        transform.localScale = scale;
    }
}
