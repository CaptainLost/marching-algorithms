using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MinMaxAttribute))]
public class MinMaxAttributeDrawer : PropertyDrawer
{
    private readonly static float s_floatFieldWidthScale = 0.2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.Vector2)
            return;

        MinMaxAttribute attibuteObject = attribute as MinMaxAttribute;

        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();

        Vector2 minMax = property.vector2Value;

        position = EditorGUI.PrefixLabel(position, label);

        float fieldWidth = position.width * s_floatFieldWidthScale;

        Rect minPosition = new Rect(position.x, position.y, fieldWidth, position.height);
        Rect sliderPosition = new Rect(position.x + fieldWidth, position.y, position.width - fieldWidth * 2f, position.height);
        Rect maxPosition = new Rect(sliderPosition.x + sliderPosition.width, position.y, fieldWidth, position.height);

        minMax.x = EditorGUI.FloatField(minPosition, (float)System.Math.Round(minMax.x, 2));
        minMax.y = EditorGUI.FloatField(maxPosition, (float)System.Math.Round(minMax.y, 2));

        EditorGUI.MinMaxSlider(sliderPosition, ref minMax.x, ref minMax.y, attibuteObject.Min, attibuteObject.Max);

        if (EditorGUI.EndChangeCheck())
        {
            minMax.x = Mathf.Clamp(minMax.x, attibuteObject.Min, attibuteObject.Max);
            minMax.y = Mathf.Clamp(minMax.y, attibuteObject.Min, attibuteObject.Max);

            property.vector2Value = minMax;
        }

        EditorGUI.EndProperty();
    }
}
