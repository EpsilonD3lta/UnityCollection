using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NonRequiredAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(NonRequiredAttribute))]
public class NonRequiredAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var previousColor = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.8f, 1, 0.8f, 1f);
        EditorGUI.PropertyField(position, property, label);
        GUI.backgroundColor = previousColor;
    }
}
#endif