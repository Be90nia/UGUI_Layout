
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LayoutSwap), true)]
public class LayoutSwapEditor : LayoutBaseEditor
{
    private LayoutSwap _layoutSwap;
    protected SerializedProperty _transitionType;
    protected SerializedProperty _swapTime;
    protected SerializedProperty _curItemIdx;
    protected SerializedProperty _rectChildren;


    protected virtual void OnEnable()
    {
        _layoutSwap = (LayoutSwap) target;
        _transitionType = serializedObject.FindProperty("_transitionType");
        _swapTime = serializedObject.FindProperty("_swapTime");
        _curItemIdx = serializedObject.FindProperty("_curItemIdx");
        _rectChildren = serializedObject.FindProperty("_rectChildren");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(_transitionType, new GUIContent("Transition Type", "_transitionType"));
        switch (_layoutSwap.TransitionType)
        {
            case Transition.Flip:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_swapFlip"), new GUIContent("Flip Setting", "_swapFlip"),true);
                break;
            case Transition.Fade:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_swapFade"), new GUIContent("Fade Setting", "_swapFade"), true);
                break;
            case Transition.Side:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_swapSide"), new GUIContent("Side Setting", "_swapSide"),true);
                break;
            case Transition.ZoomIn:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_swapZoomIn"), new GUIContent("Zoom In Setting", "_swapZoomIn"), true);
                break;
            case Transition.ZoomOut:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_swapZoomOut"), new GUIContent("Zoom Out Setting", "_swapZoomOut"), true);
                break;
            case Transition.CardStack:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_swapCardStack"), new GUIContent("Card Stack Setting", "_swapCardStack"), true);
                break;
        }
        EditorGUILayout.PropertyField(_swapTime, new GUIContent("Swap Time", "_swapTime"));
        EditorGUILayout.PropertyField(_curItemIdx, new GUIContent("Cur Item ID", "_curItemIdx"));
        EditorGUILayout.PropertyField(_rectChildren, new GUIContent("Rect Child Items", "_rectChildren"),true);

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
