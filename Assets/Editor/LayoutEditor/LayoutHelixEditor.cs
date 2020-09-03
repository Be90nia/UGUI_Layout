
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LayoutHelix), true)]
public class LayoutHelixEditor : LayoutBaseEditor
{
    private LayoutHelix _layoutHelix;
    protected SerializedProperty _radius;
    protected SerializedProperty _itemOffset;
    protected SerializedProperty _itemAngle;
    protected SerializedProperty _angleOffset;


    protected override void OnEnable()
    {
        _layoutHelix = (LayoutHelix)target;
        _radius = serializedObject.FindProperty("_radius");
        _itemOffset = serializedObject.FindProperty("_itemOffset");
        _itemAngle = serializedObject.FindProperty("_itemAngle");
        _angleOffset = serializedObject.FindProperty("_angleOffset");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_radius, new GUIContent("Helix Radius", "_radius"));
        EditorGUILayout.PropertyField(_itemOffset, new GUIContent("Helix Item Offset", "_itemOffset"));
        EditorGUILayout.PropertyField(_itemAngle, new GUIContent("Helix Item Angle", "_itemAngle"));
        EditorGUILayout.PropertyField(_angleOffset, new GUIContent("Helix Angle Offset", "_angleOffset"));

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
