using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SquaresDebugGizmos : MonoBehaviour
{
    [SerializeField]
    private float m_cellSizeScale = 1f;
    [SerializeField]
    private Gradient m_colorGradient;

    [Inject]
    private MarchingSquares m_marchingSquares;

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        for (int x = 0; x < m_marchingSquares.Settings.AmountOfCells.x; x++)
        {
            for (int y = 0; y < m_marchingSquares.Settings.AmountOfCells.y; y++)
            {
                Gizmos.color = m_colorGradient.Evaluate(m_marchingSquares.Data.GetWeight(x, y));
                Gizmos.DrawCube(m_marchingSquares.CalculateCellWorldPosition(x, y), Vector3.one * m_marchingSquares.Settings.CellSize * m_cellSizeScale);
            }
        }
    }
}
