using UnityEngine;
using Zenject;

public class SquaresCamera : MonoBehaviour
{
    [SerializeField]
    private float m_oversize;

    [Inject]
    private MarchingSquares m_marchingSquares;

    private Camera m_camera;

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
    }

    private void Start()
    {
        Vector2 simulationSize = m_marchingSquares.SimulationSize + new Vector2(m_oversize, m_oversize);

        float ortoHeight = simulationSize.y * 0.5f;
        float ortoWidth = (simulationSize.x / m_camera.aspect) * 0.5f;

        m_camera.orthographicSize = Mathf.Max(ortoWidth, ortoHeight);
    }
}
