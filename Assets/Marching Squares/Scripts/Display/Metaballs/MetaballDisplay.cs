using UnityEngine;

public class MetaballDisplay : MonoBehaviour
{
    [SerializeField]
    private float m_ballThickness;

    private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(BallSimulation.Ball ball)
    {
        float diameter = ball.Radius * 2f;

        transform.localScale = new Vector3(diameter, diameter, transform.localScale.z);

        m_spriteRenderer.material.SetFloat("_Thickness", m_ballThickness);
    }

    public void UpdateBall(BallSimulation.Ball ball)
    {
        transform.position = ball.Position;
    }
}
