using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Lock UI layout
/// </summary>
public class LayoutLock : UIBehaviour, ILayoutElement, ILayoutGroup
{
    public float minWidth { get; }
    public float preferredWidth { get; }
    public float flexibleWidth { get; }
    public float minHeight { get; }
    public float preferredHeight { get; }
    public float flexibleHeight { get; }
    public int layoutPriority { get; }

    [SerializeField] private bool _isLayoutLock = true;
    [SerializeField] private bool _isLayouChildLock = false;

    protected DrivenRectTransformTracker _tacker;
    private RectTransform _rect;
    private List<RectTransform> _rectChildren = new List<RectTransform>();

    protected bool IsLayoutLock
    {
        get => _isLayoutLock;
        set => SetProperty(ref _isLayoutLock, value);
    }

    protected bool IsLayouChildLock
    {
        get => _isLayouChildLock;
        set => SetProperty(ref _isLayouChildLock, value);
    }


    protected RectTransform _rectTransform
    {
        get
        {
            if (_rect == null)
                _rect = GetComponent<RectTransform>();
            return _rect;
        }
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        SetDirty();
    }
    protected override void OnDisable()
    {
        _tacker.Clear();
        LayoutRebuilder.MarkLayoutForRebuild(_rectTransform);
        base.OnDisable();
    }


    public void CalculateLayoutInputHorizontal()
    {
        GetRectChildren();
    }

    public void CalculateLayoutInputVertical()
    {
        
    }


    public void SetLayoutHorizontal()
    {
        SetRectTracker();
        SetChildRectTracker();
    }

    public void SetLayoutVertical()
    {

    }

    /// <summary>
    /// Get Children Transform
    /// </summary>
    private void GetRectChildren()
    {
        if (_isLayouChildLock)
        {
            _rectChildren.Clear();
            for (int i = 0; i < _rectTransform.childCount; i++)
            {
                var rect = _rectTransform.GetChild(i) as RectTransform;
                if (rect == null || !rect.gameObject.activeInHierarchy)
                    continue;
                _rectChildren.Add(rect);
            }
        }
        _tacker.Clear();
    }
    /// <summary>
    /// lock rect transform  ui layout
    /// </summary>
    protected void SetRectTracker()
    {
        if (IsLayoutLock)
        {
            _tacker.Add(this, _rectTransform,
                DrivenTransformProperties.Anchors |
                DrivenTransformProperties.AnchoredPosition3D |
                DrivenTransformProperties.SizeDelta | 
                DrivenTransformProperties.Pivot);
        }
    }
    /// <summary>
    /// lock rect chlidren transfrom ui layout
    /// </summary>
    protected void SetChildRectTracker()
    {
        if (IsLayouChildLock)
        {
            for (int i = 0; i < _rectChildren.Count; i++)
            {
                var rect = _rectChildren[i];
                _tacker.Add(this, rect, 
                    DrivenTransformProperties.Anchors |
                DrivenTransformProperties.AnchoredPosition3D |
                DrivenTransformProperties.SizeDelta |
                DrivenTransformProperties.Pivot);
            }
        }
    }

    /// <summary>
    /// Mark the LayoutGroup as dirty.
    /// </summary>
    protected void SetDirty()
    {
        if (!IsActive())
            return;

        if (!CanvasUpdateRegistry.IsRebuildingLayout())
            LayoutRebuilder.MarkLayoutForRebuild(_rectTransform);
        else
            StartCoroutine(DelayedSetDirty(_rectTransform));
    }

    IEnumerator DelayedSetDirty(RectTransform rectTransform)
    {
        yield return null;
        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
    }

    /// <summary>
    /// Helper method used to set a given property if it has changed.
    /// </summary>
    /// <param name="currentValue">A reference to the member value.</param>
    /// <param name="newValue">The new value.</param>
    protected void SetProperty<T>(ref T currentValue, T newValue)
    {
        if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
            return;
        currentValue = newValue;
        SetDirty();
    }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        SetDirty();
    }
#endif
}
