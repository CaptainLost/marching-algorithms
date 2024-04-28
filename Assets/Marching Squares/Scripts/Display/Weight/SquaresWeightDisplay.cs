using CptLost.ObjectPool;
using System.Collections;
using UnityEngine;
using Zenject;

public class SquaresWeightDisplay : MonoBehaviour
{
    [SerializeField]
    private SquaresWeightText m_textDisplayTemplate;
    [SerializeField]
    private Gradient m_weightColor;

    [Inject]
    private MarchingSquares m_marchingSquares;

    private ComponentPool<SquaresWeightText> m_textPool;
    private Coroutine m_refreshCoroutine;

    private void Start()
    {
        m_textPool = new ComponentPool<SquaresWeightText>(m_textDisplayTemplate, transform);
    }

    private void OnEnable()
    {
        m_marchingSquares.Data.OnWeightUpdate += OnWeightUpdate;
    }

    private void OnDisable()
    {
        m_marchingSquares.Data.OnWeightUpdate -= OnWeightUpdate;
    }

    public void Generate()
    {
        m_textPool.Reset();

        m_marchingSquares.LoopThroughtAllCells(CreateTextForCell);
    }

    public void Regenerate()
    {
        if (m_refreshCoroutine != null)
            return;

        m_refreshCoroutine = StartCoroutine(RegenerateAtEndOfFrame());
    }

    private void OnWeightUpdate()
    {
        Regenerate();
    }

    private IEnumerator RegenerateAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        Generate();
        m_refreshCoroutine = null;
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
