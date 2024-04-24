using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SquaresRenderer : MonoBehaviour
{
    [SerializeField]
    private MeshFilter m_meshFilter;
    [SerializeField]
    private bool m_applyLerp;

    [Inject]
    private MarchingSquares m_marchingSquares;
    private Coroutine m_refreshCoroutine;

    private void OnEnable()
    {
        m_marchingSquares.OnAnyWeightUpdate += Regenerate;
    }

    private void OnDisable()
    {
        m_marchingSquares.OnAnyWeightUpdate -= Regenerate;
    }

    [ContextMenu("Generate")]
    public void Generate()
    {
        Mesh mesh = CalucalteMesh();

        m_meshFilter.mesh = mesh;
    }

    public void Regenerate()
    {
        if (m_refreshCoroutine != null)
            return;

        m_refreshCoroutine = StartCoroutine(RegenerateAtEndOfFrame());
    }

    private Mesh CalucalteMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> verticiesList = new();
        List<int> indiciesList = new();

        m_marchingSquares.LoopThroughtAllCells(
            (cellX, cellY) =>
            {
                float[,] squareWeights = m_marchingSquares.GetWeightsOfSquareVerticles(cellX, cellY);

                if (squareWeights == null)
                    return;

                int squareIndex = m_marchingSquares.CalculateSquareIndex(squareWeights);

                if (squareIndex == 0)
                    return;

                int indiciesCount = verticiesList.Count;

                for (int i = 0; i < 6; i++)
                {
                    Vector3 verticle = SquaresLookupTable.VerticlesTable[squareIndex, i];

                    if (verticle.x == -1f || verticle.y == -1f)
                        break;

                    if (m_applyLerp)
                    {
                        m_marchingSquares.ApplyLerpToVericle(ref verticle, squareWeights);
                    }

                    verticle.x += cellX;
                    verticle.y += cellY;

                    verticiesList.Add(verticle);
                }

                for (int i = 0; i < 9; i++)
                {
                    int index = SquaresLookupTable.IndiciesTable[squareIndex, i];

                    if (index == SquaresLookupTable.IndiciesInvalidIndex)
                        break;

                    indiciesList.Add(indiciesCount + index);
                }
            });

        mesh.vertices = verticiesList.ToArray();
        mesh.triangles = indiciesList.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }

    private IEnumerator RegenerateAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        Generate();
        m_refreshCoroutine = null;
    }
}