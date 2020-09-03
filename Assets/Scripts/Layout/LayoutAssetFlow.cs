using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LayoutAssetFlow : LayoutBase
{
    [SerializeField]
    protected AssetFlowEnum _assetFlowEnum = AssetFlowEnum.Loop;
    [SerializeField]
    private AssetFlowLoop _assetFlowLoop = new AssetFlowLoop();
    [SerializeField]
    private AssetFlowNull _assetFlowNull = new AssetFlowNull();
    [SerializeField]
    private AssetFlowClamp _assetFlowClamp = new AssetFlowClamp();
    [SerializeField]
    private AnimationCurve _alphaCurve = new AnimationCurve(new Keyframe() { time = 0, value = 0.9f }, new Keyframe() { time = 0.5f, value = 0.95f }, new Keyframe() { time = 1, value = 0.9f });
    [SerializeField]
    private float _posCurveFactorZ = 1;
    [SerializeField]
    private float _posCurveFactorX = 500f;
    [SerializeField]
    private float _yPos = 0f;

    private float _increment = 0;
    [SerializeField]
    private bool _autoPlay = false;

    [SerializeField]
    private bool _isPlay = false;

    [SerializeField]
    private float _startValue = 1f;
    [SerializeField]
    private float _endValue = 0f;
    [SerializeField]
    private float _duration = 3f;

    private new Dictionary<string, RectTransform> _rectChildren = new Dictionary<string, RectTransform>();
    [SerializeField]
    private List<AssetFlowData> _assetFlowDatas = new List<AssetFlowData>();

    public float StartValue
    {
        get => _startValue;
        set => _startValue = value;
    }

    public float EndValue
    {
        get => _endValue;
        set => _endValue = value;
    }

    public float Duration
    {
        get => _duration;
        set => _duration = value;
    }

    public bool IsPlay => _isPlay;

    public bool AutoPlay
    {
        get => _autoPlay;
        set => _autoPlay = value;
    }

    public float PosCurveFactorX
    {
        get => _posCurveFactorX;
        set => _posCurveFactorX = value;
    }

    public float PosCurveFactorZ
    {
        get => _posCurveFactorZ;
        set => _posCurveFactorZ = value;
    }
    public float ChildrenPosY
    {
        get => _yPos;
        set => _yPos = value;
    }


    public float Value
    {
        get
        {
            float _value = 0;
            switch (_assetFlowEnum)
            {
                case AssetFlowEnum.Null:
                    _value = _assetFlowNull.Value;
                    break;
                case AssetFlowEnum.Loop:
                    _value = _assetFlowLoop.Value;
                    break;
                case AssetFlowEnum.Clamp:
                    _value = _assetFlowClamp.Value;
                    break;
            }
            return _value;
        }
        set
        {
            switch (_assetFlowEnum)
            {
                case AssetFlowEnum.Null:
                    _assetFlowNull.Value = value;
                    break;
                case AssetFlowEnum.Loop:
                    _assetFlowLoop.Value = value;
                    break;
                case AssetFlowEnum.Clamp:
                    _assetFlowClamp.Value = value;
                    break;
            }
        }
    }

    public AnimationCurve ScaleCurve
    {
        get
        {
            AnimationCurve _scaleCurve = new AnimationCurve();
            switch (_assetFlowEnum)
            {
                case AssetFlowEnum.Null:
                    _scaleCurve = _assetFlowNull.ScaleCurve;
                    break;
                case AssetFlowEnum.Loop:
                    _scaleCurve = _assetFlowLoop.ScaleCurve;
                    break;
                case AssetFlowEnum.Clamp:
                    _scaleCurve = _assetFlowClamp.ScaleCurve;
                    break;
            }
            return _scaleCurve;
        }
    }

    public AnimationCurve PositionCurveX
    {
        get
        {
            AnimationCurve _positionCurve = new AnimationCurve();
            switch (_assetFlowEnum)
            {
                case AssetFlowEnum.Null:
                    _positionCurve = _assetFlowNull.PositionCurveX;
                    break;
                case AssetFlowEnum.Loop:
                    _positionCurve = _assetFlowLoop.PositionCurveX;
                    break;
                case AssetFlowEnum.Clamp:
                    _positionCurve = _assetFlowClamp.PositionCurveX;
                    break;
            }
            return _positionCurve;
        }
    }

    public AnimationCurve PositionCurveZ
    {
        get
        {
            AnimationCurve _positionCurve = new AnimationCurve();
            switch (_assetFlowEnum)
            {
                case AssetFlowEnum.Null:
                    _positionCurve = _assetFlowNull.PositionCurveZ;
                    break;
                case AssetFlowEnum.Loop:
                    _positionCurve = _assetFlowLoop.PositionCurveZ;
                    break;
                case AssetFlowEnum.Clamp:
                    _positionCurve = _assetFlowClamp.PositionCurveZ;
                    break;
            }
            return _positionCurve;
        }
    }

    public AnimationCurve AlphaCurve
    {
        get => _alphaCurve;
    }

    public AssetFlowEnum AssetFlowType => _assetFlowEnum;

    protected override void OnEnable()
    {
        Init();
        if (_rectChildren.Count > 0)
            _increment = 1 / (float)_rectChildren.Count;
        Init(0);
//        SetUIMaterial();
        SortDepth();
    }

    public override void Init()
    {
        var childs = gameObject.GetComponentsInChildren<RectTransform>();
        if (_rectChildren.Count == 0)
        {
            _rectChildren.Clear();
            _assetFlowDatas.Clear();
            for (int i = 0; i < childs.Length; i++)
            {
                if (childs[i] != rectTransform)
                {
                    if (!_rectChildren.ContainsKey(childs[i].name))
                    {
                        _rectChildren.Add(childs[i].name, childs[i]);
                        AssetFlowData data = new AssetFlowData();
                        data.LocalScale = childs[i].localScale;
                        data.Name = childs[i].name;
                        _assetFlowDatas.Add(data);
                    }
                    else
                    {
                        var name = childs[i].name + i;
                        _rectChildren.Add(name, childs[i]);
                        AssetFlowData data = new AssetFlowData();
                        data.LocalScale = childs[i].localScale;
                        data.Name = name;
                        _assetFlowDatas.Add(data);
                    }

                }
            }

        }
    }

    private void Init(float fValue)
    {
        UpdateScrollView(fValue);
    }

    /// <summary>
    /// 缩放曲线模拟当前缩放值
    /// </summary>
    private float GetScaleValue(float sliderValue, float added)
    {
        var value = sliderValue + added;
        float scaleValue = ScaleCurve.Evaluate(value);
        return scaleValue;
    }

    private float GetAlphaValue(float sliderValue, float added)
    {
        var value = sliderValue + added;
        float scaleValue = AlphaCurve.Evaluate(value);
        return scaleValue;
    }


    /// <summary>
    /// 位置曲线模拟当前x轴位置
    /// </summary>
    private float GetXPosValue(float sliderValue, float added)
    {
        var value = sliderValue + added;
        float evaluateValue = PositionCurveX.Evaluate(value) * _posCurveFactorX;
        return evaluateValue;
    }

    /// <summary>
    /// 位置曲线模拟当前x轴位置
    /// </summary>
    private float GetZPosValue(float sliderValue, float added)
    {
        var value = sliderValue + added;
        float evaluateValue = PositionCurveZ.Evaluate(value) * _posCurveFactorZ;
        return evaluateValue;
    }

    void Start()
    {
        if (_autoPlay)
            PlayAnimation(_startValue, _endValue, _duration);
    }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
    }
#endif

    protected override void OnDisable()
    {
        Disable();
    }

    protected override void Disable()
    {
        for (int i = 0; i < _rectChildren.Keys.Count; i++)
        {
            var value = _rectChildren.Values.ElementAt(i);
            value.localScale = Vector3.one;
            value.localPosition = Vector3.zero;
            value.transform.SetSiblingIndex(i);
        }
        _rectChildren.Clear();
        _assetFlowDatas.Clear();
    }

    public override void CalculateLayoutInputVertical()
    {
        if (_rectChildren.Count > 0)
        {
            UpdateScrollView(Value);
            SortDepth();
        }
    }

    void Update()
    {
        if (_rectChildren.Count > 0)
        {
            UpdateScrollView(Value);
            SortDepth();
        }

    }

    private void UpdateScrollView(float fValue)
    {
        float i = 0;
        foreach (var key in _rectChildren.Keys)
        {
            float add = i / (float)_rectChildren.Count;
            float scale = GetScaleValue(fValue, add);
            _rectChildren[key].localScale = new Vector3(scale, scale, scale);
            _rectChildren[key].localPosition = new Vector3(GetXPosValue(fValue, add), _yPos, GetZPosValue(fValue, add));

            var color = _rectChildren[key].GetComponent<Image>().color;
            color.a = GetAlphaValue(fValue, add);
            _rectChildren[key].GetComponent<Image>().color = color;

            i++;
            for (int j = 0; j < _assetFlowDatas.Count; j++)
            {
                if (_assetFlowDatas[j].Name == key)
                    _assetFlowDatas[j].LocalScale = _rectChildren[key].localScale;
            }
        }

    }

    public override void SetLayoutHorizontal()
    {

    }

    public override void SetLayoutVertical()
    {
    }

    public void PlayAnimation(float from, float to, float time)
    {
        DOTween.To(() => from, r => from = r, to, time).SetEase(Ease.Linear).OnUpdate(() =>
        {
            Value = from;
            _isPlay = true;
        })
            .OnComplete(() => { _isPlay = false; });
    }

    public void SortDepth()
    {
        _assetFlowDatas.Sort(new CompareDepthMethod());

        for (int i = 0; i < _assetFlowDatas.Count; i++)
            _rectChildren[_assetFlowDatas[i].Name].transform.SetSiblingIndex(i);
    }
}

/// <summary>
/// 用于层级对比接口
/// </summary>
public class CompareDepthMethod : IComparer<AssetFlowData>
{
    public int Compare(AssetFlowData left, AssetFlowData right)
    {
        if (left.LocalScale.x > right.LocalScale.x)
            return 1;
        else if (left.LocalScale.x < right.LocalScale.x)
            return -1;
        else
            return 0;
    }
}

[Serializable]
public class AssetFlowData
{
    public Vector3 LocalScale = Vector3.zero;
    public string Name = "";
}

[Serializable]
public class AssetFlowNull
{
    public AnimationCurve ScaleCurve = new AnimationCurve();
    public AnimationCurve PositionCurveX = new AnimationCurve();
    public AnimationCurve PositionCurveZ = new AnimationCurve();
    public float Value = 0;
}
[Serializable]
public class AssetFlowLoop
{
    public AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe() { time = 0, value = 0 }, new Keyframe() { time = 0.5f, value = 1 }, new Keyframe() { time = 1, value = 0 });
    public AnimationCurve PositionCurveX = AnimationCurve.Linear(0, -0.5f, 1, 0.5f);
    public AnimationCurve PositionCurveZ = new AnimationCurve(new Keyframe() { time = 0, value = 0 }, new Keyframe() { time = 0.5f, value = -1 }, new Keyframe() { time = 1, value = 0 });
    [Range(0, 1)]
    public float Value = 0;
}
[Serializable]
public class AssetFlowClamp
{
    public AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe() { time = 0, value = 0 }, new Keyframe() { time = 0.5f, value = 1 }, new Keyframe() { time = 1, value = 0 });
    public AnimationCurve PositionCurveX = AnimationCurve.Linear(0, -0.5f, 1, 0.5f);
    public AnimationCurve PositionCurveZ = new AnimationCurve(new Keyframe() { time = 0, value = 0 }, new Keyframe() { time = 0.5f, value = -1 }, new Keyframe() { time = 1, value = 0 });
    [Range(-1, 1)]
    public float Value = 0;
}