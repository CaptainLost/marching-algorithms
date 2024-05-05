using System.Collections;
using UnityEngine;
using Zenject;

public abstract class BaseSquaresDisplay : MonoBehaviour
{
    [Inject]
    protected MarchingSquares m_marchingSquares;

    private Coroutine m_refreshCoroutine;

    protected virtual void OnEnable()
    {
        m_marchingSquares.Data.OnWeightUpdate += OnWeightUpdate;
    }

    protected virtual void OnDisable()
    {
        m_marchingSquares.Data.OnWeightUpdate -= OnWeightUpdate;
    }

    public abstract void Generate();

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
}
