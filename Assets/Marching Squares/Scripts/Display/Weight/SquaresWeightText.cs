using TMPro;
using UnityEngine;

public class SquaresWeightText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_weightText;

    private RectTransform m_rectTransform;

    private void Awake()
    {
        m_rectTransform = (RectTransform)transform;
    }

    public void SetWeight(float weight)
    {
        m_weightText.text = weight.ToString("F2");
    }

    public void SetColor(Color color)
    {
        m_weightText.color = color;
    }

    public void SetSize(Vector2 size)
    {
        m_rectTransform.sizeDelta = size;
    }
}
