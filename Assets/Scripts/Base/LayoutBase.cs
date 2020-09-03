using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutBase : LayoutGroup
{
    [SerializeField] private Vector2 _maxAnchors = new Vector2(0.5f, 0.5f);
    [SerializeField] private Vector2 _minAnchors = new Vector2(0.5f, 0.5f);
    [SerializeField] private Vector2 _pivot = new Vector2(0.5f, 0.5f);
    [SerializeField] private Vector2 _cellSize = new Vector2(100, 100);
    [SerializeField] protected List<RectTransform> _rectChildren = new List<RectTransform>();

    public List<RectTransform> RectChildren => _rectChildren;


    protected Vector2 CellSize
    {
        get => _cellSize;
    }

    public Vector2 MaxAnchors
    {
        get
        {
            if (_maxAnchors.x < 0)
                _maxAnchors.x = 0;
            else if (_maxAnchors.x > 1)
                _maxAnchors.x = 1;
            if (_maxAnchors.y < 0)
                _maxAnchors.y = 0;
            else if (_maxAnchors.y > 1)
                _maxAnchors.y = 1;
            return _maxAnchors;
        }
        set
        {
            if ( value.x < 0)
                _maxAnchors.x = 0;
            else if (value.x > 1)
                _maxAnchors.x = 1;
            if (value.y < 0)
                _maxAnchors.y = 0;
            else if (value.y > 1)
                _maxAnchors.y = 1;
            _maxAnchors = value;

        }
    }

    public Vector2 MinAnchors
    {
        get
        {
            if (_minAnchors.x < 0)
                _minAnchors.x = 0;
            else if (_minAnchors.x > 1)
                _minAnchors.x = 1;
            if (_minAnchors.y < 0)
                _minAnchors.y = 0;
            else if (_minAnchors.y > 1)
                _minAnchors.y = 1;
            return _minAnchors;
        }
        set
        {
            if (value.x < 0)
                _minAnchors.x = 0;
            else if (value.x > 1)
                _minAnchors.x = 1;
            if (value.y < 0)
                _minAnchors.y = 0;
            else if (value.y > 1)
                _minAnchors.y = 1;
            _minAnchors = value;

        }
    }

    public virtual void Init()
    {
        var childs = gameObject.GetComponentsInChildren<RectTransform>();
        if (_rectChildren.Count == 0)
        {
            _rectChildren.Clear();
            for (int i = 0; i < childs.Length; i++)
            {
                if (childs[i] != rectTransform)
                {
                    _rectChildren.Add(childs[i]);
                }
            }
        }
    }

    protected virtual void Disable()
    {
        for (int i = 0; i < _rectChildren.Count; i++)
        {
            _rectChildren[i].localScale = Vector3.one;
            _rectChildren[i].localPosition = Vector3.zero;
            _rectChildren[i].localEulerAngles = Vector3.zero;
            _rectChildren[i].transform.SetSiblingIndex(i);
            _rectChildren[i].gameObject.SetActive(true);
        }
        _rectChildren.Clear();
    }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
    }
#endif
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void CalculateLayoutInputVertical()
    {

    }

    public override void SetLayoutHorizontal()
    {
        SetChildRectTracker();
    }

    public override void SetLayoutVertical()
    {

    }
    /// <summary>
    /// set children rect tracker
    /// </summary>
    protected virtual void SetChildRectTracker()
    {
        for (int i = 0; i < rectChildren.Count; i++)
        {
            RectTransform rect = rectChildren[i];
            m_Tracker.Add(this, rect,
                DrivenTransformProperties.Anchors |
                DrivenTransformProperties.AnchoredPosition3D |
                DrivenTransformProperties.SizeDelta);
            rect.anchorMin = MinAnchors;
            rect.anchorMax = MaxAnchors;
            rect.sizeDelta = _cellSize;
            rect.pivot = _pivot;
        }
    }
}
