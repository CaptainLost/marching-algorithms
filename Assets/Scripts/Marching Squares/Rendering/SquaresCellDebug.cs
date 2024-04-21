using UnityEngine;
using Zenject;

public class SquaresCellDebug : MonoBehaviour
{
    [SerializeField]
    private float m_sizeScale = 0.1f;
    [SerializeField]
    private Gradient m_debugColor;

    [Inject]
    private MarchingSquares m_marchingSquares;

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        m_marchingSquares.LoopThroughtAllCells(DrawDebugForCell);
    }

    private void DrawDebugForCell(int cellX, int cellY)
    {
        Vector3 worldPosition = m_marchingSquares.GetCellCenterWorld(new Vector3Int(cellX, cellY, 0));
        float weight = m_marchingSquares.GetWeight(cellX, cellY);

        Color gizmoColor = m_debugColor.Evaluate(weight);

        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(worldPosition, Vector3.one * m_sizeScale);
    }
}
