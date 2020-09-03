using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LayoutAssetFlow), true)]
public class LayoutAssetFlowEditor : LayoutBaseEditor
{
    private LayoutAssetFlow _layoutAssetFlow;
    private SerializedProperty _assetFlowEnum;
    private SerializedProperty _posCurveFactorX;
    private SerializedProperty _posCurveFactorZ;
    private SerializedProperty _yPos;
    private SerializedProperty _autoPlay;
    private SerializedProperty _startValue;
    private SerializedProperty _endValue;
    private SerializedProperty _duration;

    private SerializedProperty _uiDepth;

    private SerializedProperty _alphaCurve;
    //    private SerializedProperty _zPos;
    //    private SerializedProperty _spacing;

    protected virtual void OnEnable()
    {
        if(target != null)
            _layoutAssetFlow = (LayoutAssetFlow) target;
        _assetFlowEnum = serializedObject.FindProperty("_assetFlowEnum");
        _posCurveFactorX = serializedObject.FindProperty("_posCurveFactorX");
        _posCurveFactorZ = serializedObject.FindProperty("_posCurveFactorZ");
        _yPos = serializedObject.FindProperty("_yPos");
        _uiDepth = serializedObject.FindProperty("_uiDepth");
        _autoPlay = serializedObject.FindProperty("_autoPlay");
        _startValue = serializedObject.FindProperty("_startValue");
        _endValue = serializedObject.FindProperty("_endValue");
        _duration = serializedObject.FindProperty("_duration");
        _alphaCurve = serializedObject.FindProperty("_alphaCurve");
//        _spacing = serializedObject.FindProperty("_spacing");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_assetFlowEnum, new GUIContent("Aasset Flow Enum", "_assetFlowEnum"));
        EditorGUILayout.PropertyField(_posCurveFactorX, new GUIContent("Position Curve Factor X", "_posCurveFactorX"));
        EditorGUILayout.PropertyField(_posCurveFactorZ, new GUIContent("Position Curve Factor Z", "_posCurveFactorZ"));
        EditorGUILayout.PropertyField(_yPos, new GUIContent("Children Pos Y", "_yPos"));
//        EditorGUILayout.PropertyField(_uiDepth, new GUIContent("Children Material", "_uiDepth"));
        EditorGUILayout.PropertyField(_autoPlay, new GUIContent("Auto Play", "_autoPlay"));
        EditorGUILayout.PropertyField(_startValue, new GUIContent("Start Value", "_startValue"));
        EditorGUILayout.PropertyField(_endValue, new GUIContent("End Value", "_endValue"));
        EditorGUILayout.PropertyField(_duration, new GUIContent("Duration", "_duration"));
        EditorGUILayout.PropertyField(_alphaCurve, new GUIContent("Alpha Curve", "_alphaCurve"));

        switch (_layoutAssetFlow.AssetFlowType)
        {
            case AssetFlowEnum.Null:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_assetFlowNull"), new GUIContent("Asset Flow Setting", "_assetFlowNull"),true);
                break;
            case AssetFlowEnum.Loop:
                _layoutAssetFlow.ScaleCurve.postWrapMode = WrapMode.Loop;
                _layoutAssetFlow.ScaleCurve.preWrapMode = WrapMode.Loop;
                _layoutAssetFlow.PositionCurveX.postWrapMode = WrapMode.Loop;
                _layoutAssetFlow.PositionCurveX.preWrapMode = WrapMode.Loop;           
                _layoutAssetFlow.PositionCurveZ.postWrapMode = WrapMode.Loop;
                _layoutAssetFlow.PositionCurveZ.preWrapMode = WrapMode.Loop;

                _layoutAssetFlow.AlphaCurve.postWrapMode = WrapMode.Loop;
                _layoutAssetFlow.AlphaCurve.preWrapMode = WrapMode.Loop;

                for (int i = 0; i < _layoutAssetFlow.ScaleCurve.keys.Length; i++)
                {
                    AnimationUtility.SetKeyLeftTangentMode(_layoutAssetFlow.AlphaCurve, i, AnimationUtility.TangentMode.Linear);
                    AnimationUtility.SetKeyRightTangentMode(_layoutAssetFlow.AlphaCurve, i, AnimationUtility.TangentMode.Linear);
                }

                for (int i = 0; i < _layoutAssetFlow.ScaleCurve.keys.Length; i++)
                {
                    AnimationUtility.SetKeyLeftTangentMode(_layoutAssetFlow.ScaleCurve, i, AnimationUtility.TangentMode.Linear);
                    AnimationUtility.SetKeyRightTangentMode(_layoutAssetFlow.ScaleCurve, i, AnimationUtility.TangentMode.Linear);
                }

                for (int i = 0; i < _layoutAssetFlow.PositionCurveZ.keys.Length; i++)
                {
                    AnimationUtility.SetKeyLeftTangentMode(_layoutAssetFlow.PositionCurveZ, i, AnimationUtility.TangentMode.Linear);
                    AnimationUtility.SetKeyRightTangentMode(_layoutAssetFlow.PositionCurveZ, i, AnimationUtility.TangentMode.Linear);
                }
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_assetFlowLoop"), new GUIContent("Asset Flow Setting", "_assetFlowLoop"), true);
                break;
            case AssetFlowEnum.Clamp:

                _layoutAssetFlow.AlphaCurve.postWrapMode = WrapMode.Loop;
                _layoutAssetFlow.AlphaCurve.preWrapMode = WrapMode.Loop;

                for (int i = 0; i < _layoutAssetFlow.ScaleCurve.keys.Length; i++)
                {
                    AnimationUtility.SetKeyLeftTangentMode(_layoutAssetFlow.AlphaCurve, i, AnimationUtility.TangentMode.Linear);
                    AnimationUtility.SetKeyRightTangentMode(_layoutAssetFlow.AlphaCurve, i, AnimationUtility.TangentMode.Linear);
                }

                for (int i = 0; i < _layoutAssetFlow.ScaleCurve.keys.Length; i++)
                {
                    AnimationUtility.SetKeyLeftTangentMode(_layoutAssetFlow.ScaleCurve, i, AnimationUtility.TangentMode.Linear);
                    AnimationUtility.SetKeyRightTangentMode(_layoutAssetFlow.ScaleCurve, i, AnimationUtility.TangentMode.Linear);
                }

                for (int i = 0; i < _layoutAssetFlow.PositionCurveZ.keys.Length; i++)
                {
                    AnimationUtility.SetKeyLeftTangentMode(_layoutAssetFlow.PositionCurveZ, i, AnimationUtility.TangentMode.Linear);
                    AnimationUtility.SetKeyRightTangentMode(_layoutAssetFlow.PositionCurveZ, i, AnimationUtility.TangentMode.Linear);
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("_assetFlowClamp"), new GUIContent("Asset Flow Setting", "_assetFlowClamp"), true);
                break;
        }

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}
