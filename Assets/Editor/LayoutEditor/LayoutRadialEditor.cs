using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LayoutRadial), true)]
[CanEditMultipleObjects]

public class LayoutRadialEditor : Editor
{
    LayoutRadial _layoutRadialLayout;
    SerializedProperty radialRadius, radialItemSize, cellSize;
    SerializedProperty angleOffset;
    SerializedProperty radialType, spaceAngle, itemAngle;

    protected virtual void OnEnable()
    {
        _layoutRadialLayout = (LayoutRadial)target;

        radialRadius = serializedObject.FindProperty("_radialRadius"); ;
        radialItemSize = serializedObject.FindProperty("_radialItemSize");
        angleOffset = serializedObject.FindProperty("_angleOffset");
        cellSize = serializedObject.FindProperty("_cellSize");
        radialType = serializedObject.FindProperty("_radialType");
        spaceAngle = serializedObject.FindProperty("_spaceAngle");
        itemAngle = serializedObject.FindProperty("_itemAngle");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(radialType, new GUIContent("RadialType", "Radial Type"));

        if (radialType.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(spaceAngle, new GUIContent("SpaceAngle", "Space Angle"));
            EditorGUILayout.PropertyField(itemAngle, new GUIContent("ItemAngle", "Item Angle"));
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(cellSize, new GUIContent("CellSize", "Radial item size"));
        EditorGUILayout.Slider(radialItemSize, 0.0f, 5.0f, new GUIContent("Radial ItemSize", "The distance that the buttons will be from the center of the menu."));
        EditorGUILayout.Slider(radialRadius, 0.0f, 5.0f, new GUIContent("Radial Radius", "The size of the radial buttons."));
        EditorGUILayout.Slider(angleOffset, -180, 180, new GUIContent("Angle Offset", "Offsets the buttons position from neutral."));

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    protected void OnSceneGUI()
    {

    }
}

