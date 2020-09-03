using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LayoutBase), true)]
public class LayoutBaseEditor : Editor
{
    protected SerializedProperty inputType;
    protected SerializedProperty cellSize;

    protected virtual void OnEnable()
    {
        inputType = serializedObject.FindProperty("_inputType");
        cellSize = serializedObject.FindProperty("_cellSize");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

//        EditorGUILayout.PropertyField(inputType, new GUIContent("InputType", "inputType"));
        EditorGUILayout.PropertyField(cellSize, new GUIContent("CellSize", "cellSize"));

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}

