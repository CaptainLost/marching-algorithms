using CptLost.ObjectPool;
using UnityEngine;

public class SquaresWeightDisplay : BaseSquaresDisplay
{
    [SerializeField]
    private SquaresWeightText m_textDisplayTemplate;
    [SerializeField]
    private Gradient m_weightColor;

    private ComponentPool<SquaresWeightText> m_textPool;

    private void Start()
    {
        m_textPool = new ComponentPool<SquaresWeightText>(m_textDisplayTemplate, transform);
    }

    public override void Generate()
    {
        m_textPool.Reset();

        m_marchingSquares.LoopThroughtAllCells(CreateTextForCell);
    }

    private void CreateTextForCell(int cellX, int cellY)
    {
        SquaresWeightText textObject = m_textPool.DequeueObject();

        textObject.SetWeight(m_marchingSquares.Data.GetWeight(cellX, cellY));
        textObject.SetSize(new Vector2(m_marchingSquares.Settings.CellSize, m_marchingSquares.Settings.CellSize));
        textObject.SetColor(m_weightColor.Evaluate(m_marchingSquares.Data.GetWeight(cellX, cellY)));

        textObject.transform.position = m_marchingSquares.CalculateCellWorldPosition(cellX, cellY);
    }
}
