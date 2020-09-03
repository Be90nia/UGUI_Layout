using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LayoutPinBoard), true)]
public class LayoutPinBoardEditor : LayoutBaseEditor
{
    private LayoutPinBoard _layoutPinBoard;
    private SerializedProperty _allowMove;
    private SerializedProperty _allowRotate;
    private SerializedProperty _allowAnimation;

    protected override void OnEnable()
    {
        base.OnEnable();
        _layoutPinBoard = (LayoutPinBoard) target;
        _allowMove = serializedObject.FindProperty("_allowMove");
        _allowRotate = serializedObject.FindProperty("_allowRotate");
        _allowAnimation = serializedObject.FindProperty("_allowAnimation");

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_allowMove, new GUIContent("Allow Move", "_allowMove"));
        EditorGUILayout.PropertyField(_allowRotate, new GUIContent("Allow Rotate", "_allowRotate"));
        EditorGUILayout.PropertyField(_allowAnimation, new GUIContent("Allow Animation", "_allowAnimation"));
        if (_layoutPinBoard.AllowMove)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_pinBoardMove"),
                new GUIContent("Move Setting", "_pinBoardMove"), true);
        if (_layoutPinBoard.AllowRotate)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_pinBoardRotate"),
                new GUIContent("Rotate Setting", "_pinBoardRotate"), true);
        if (_layoutPinBoard.AllowAnimation)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_pinBoardScale"),
                new GUIContent("Scale Setting", "_pinBoardScale"), true);
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

    }
}
