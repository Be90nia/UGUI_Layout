using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LayoutKenBurns), true)]
public class LayoutKenBurnsEditor : LayoutBaseEditor
{
    private LayoutKenBurns _layoutKenBurns;
    private SerializedProperty _zoomType;
    private SerializedProperty _displayDuration, _transitionDuration;

    protected override void OnEnable()
    {
       //base.OnEnable();
       _layoutKenBurns = (LayoutKenBurns) target;
        _zoomType = serializedObject.FindProperty("_zoomType");
        _displayDuration = serializedObject.FindProperty("_displayDuration");
        _transitionDuration = serializedObject.FindProperty("_transitionDuration");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(_zoomType, new GUIContent("Zoom Type", "Type of Zoom"));
        EditorGUILayout.PropertyField(_displayDuration, new GUIContent("Display Duration", "Display Duration"));
        EditorGUILayout.PropertyField(_transitionDuration, new GUIContent("TransitionDuration", "Transition Duration"));

        switch (_layoutKenBurns.ZoomValue)
        {
            case LayoutKenBurns.ZoomType.ZoomIn:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_kenBurnsZoomIn"), new GUIContent("Ken Burns Zoom In", "_kenBurnsZoomIn"),true);
                break;
            case LayoutKenBurns.ZoomType.ZoomOut:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_kenBurnsZoomOut"), new GUIContent("Ken Burns Zoom Out", "_kenBurnsZoomOut"),true);
                break;
        }
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}

