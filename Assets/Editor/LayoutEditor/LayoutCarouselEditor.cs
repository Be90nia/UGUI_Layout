using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LayoutCarousel), true)]
public class LayoutCarouselEditor : LayoutBaseEditor
{
    private LayoutCarousel _layoutCarousel;
    private SerializedProperty _carouselRadius;
    private SerializedProperty _angleOffset;

    protected virtual void OnEnable()
    {
        _layoutCarousel = (LayoutCarousel) target;
        _carouselRadius = serializedObject.FindProperty("_carouselRadius");
        _angleOffset = serializedObject.FindProperty("_angleOffset");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(_carouselRadius, new GUIContent("Carousel Radius", "_carouselRadius"));
        EditorGUILayout.PropertyField(_angleOffset, new GUIContent("Carousel Angle Offset", "_angleOffset"));

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

    }
}
