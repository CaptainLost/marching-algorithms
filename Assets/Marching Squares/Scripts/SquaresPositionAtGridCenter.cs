using UnityEngine;
using Zenject;

public class SquaresPositionAtGridCenter : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_offset;

    [Inject]
    private MarchingSquares m_marchingSquares;

    private void Start()
    {
        transform.position = m_marchingSquares.CalculateGridCenter() + m_offset;
    }
}
