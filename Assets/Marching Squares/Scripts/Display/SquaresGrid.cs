using UnityEngine;
using Zenject;

public class SquaresGrid : MonoBehaviour
{
    [Inject]
    private MarchingSquares m_marchingSquares;

    private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        transform.position = m_marchingSquares.CalculateGridCenter();

        Vector2 simulationSize = m_marchingSquares.CalculateSimulationSize();
        transform.localScale = new Vector3(simulationSize.x, simulationSize.y, 1f);

        m_spriteRenderer.material.SetVector("_AmountOfCells", (Vector2)m_marchingSquares.Settings.AmountOfCells - Vector2.one);
    }
}
