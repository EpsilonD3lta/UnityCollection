using System;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MinMaxRangeAttribute : Attribute
{
    public MinMaxRangeAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
    public float Min { get; private set; }
    public float Max { get; private set; }
}

[Serializable]
public struct MinMaxFloat
{
    public MinMaxFloat(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public float min;
    public float max;

    public float GetRandom() => UnityEngine.Random.Range(min, max);
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MinMaxFloat), true)]
public class MinMaxRangeDrawer : PropertyDrawer
{
    private const float floatFieldRectWidth = 50f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        SerializedProperty minProp = property.FindPropertyRelative(nameof(MinMaxFloat.min));
        SerializedProperty maxProp = property.FindPropertyRelative(nameof(MinMaxFloat.max));

        float min = minProp.floatValue;
        float max = maxProp.floatValue;

        float rangeMin = 0;
        float rangeMax = 1;

        var range = (MinMaxRangeAttribute)fieldInfo.GetCustomAttribute(typeof(MinMaxRangeAttribute), true);
        if (range != null)
        {
            rangeMin = range.Min;
            rangeMax = range.Max;
        }

        EditorGUI.BeginChangeCheck();

        // Min value
        var minFloatFieldRect = new Rect(position);
        minFloatFieldRect.width = floatFieldRectWidth;
        min = EditorGUI.DelayedFloatField(minFloatFieldRect, min);
        min = Mathf.Min(min, max);
        position.xMin += floatFieldRectWidth + 5;

        // Max value
        var maxFloatFieldRect = new Rect(position);
        maxFloatFieldRect.xMin = maxFloatFieldRect.xMax - floatFieldRectWidth;
        max = EditorGUI.DelayedFloatField(maxFloatFieldRect, max);
        max = Mathf.Max(min, max);
        position.xMax -= floatFieldRectWidth + 5;

        //Slider
        EditorGUI.MinMaxSlider(position, ref min, ref max, rangeMin, rangeMax);

        if (EditorGUI.EndChangeCheck())
        {
            minProp.floatValue = min;
            maxProp.floatValue = max;
        }

        EditorGUI.EndProperty();
    }
}
#endif
