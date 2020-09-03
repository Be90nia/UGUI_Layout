using System;
using UnityEngine;
using UnityEngine.UI;

public class LayoutRadial : LayoutBase
{
    public enum RadialType
    {
        Circle = 0,     // 圆形
        Sector = 1      // 扇形
    }

    [SerializeField]
    protected RadialType _radialType = RadialType.Circle;

    // 间隔角度，类型为Sector时有效
    [SerializeField]
    protected float _spaceAngle = 0;

    // 
    [SerializeField]
    protected float _itemAngle = 45;

    [SerializeField]
    protected float _radialRadius = 1.0f;

    [SerializeField]
    protected float _radialItemSize = 1.0f;

    [SerializeField]
    protected float _angleOffset = 0;

    private float _anglePerItem;

    public float AnglePerItem
    {
        get { return _anglePerItem; }
    }

    public event Action OnRadialItemChanged;

    /// <summary>
    /// Called by the layout system
    /// Also see ILayoutElement
    /// </summary>
    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
        if (rectChildren.Count > 0)
        {
            SetCellsAlongAxis();
            OnRadialItemChanged?.Invoke();
        }
    }

    private void CalcAnglePerItem()
    {
        if (_radialType == RadialType.Circle)
        {
            _anglePerItem = 360f / rectChildren.Count;
        }
        else
        {
            _anglePerItem = _itemAngle;
        }
    }

    private void SetCellsAlongAxis()
    {
        CalcAnglePerItem();

        for (int i = 0; i < rectChildren.Count; i++)
        {
            RectTransform rect = rectChildren[i];

            m_Tracker.Add(this, rect,
                DrivenTransformProperties.Anchors |
                DrivenTransformProperties.AnchoredPosition |
                DrivenTransformProperties.SizeDelta);

            Vector2 cellSize = CellSize * _radialItemSize;
            //float radialRadius = (cellSize.y / 2f) *_radialRadius;
            //float radialRadius = ((rectTransform.sizeDelta.x / 2f) - (cellSize.y / 2f)) * _radialRadius;
            float angleInRadians = -AnglePerItem * Mathf.Deg2Rad;
            float spaceAngleInRadians = -_spaceAngle * Mathf.Deg2Rad;
            float radialRadius = (cellSize.x / 2f / Mathf.Sin(-angleInRadians /2f) - (cellSize.y / 2f)) * _radialRadius;

            Vector2 anchoredPosition = Vector2.zero;
            Vector3 eulerAngles = Vector3.zero;

            float spaceIdx = (_radialType == RadialType.Sector) ? i : 0;

            anchoredPosition.x += Mathf.Cos(angleInRadians * i + spaceAngleInRadians * spaceIdx + 90 * Mathf.Deg2Rad + _angleOffset * Mathf.Deg2Rad) * radialRadius;
            anchoredPosition.y += Mathf.Sin(angleInRadians * i + spaceAngleInRadians * spaceIdx + 90 * Mathf.Deg2Rad + _angleOffset * Mathf.Deg2Rad) * radialRadius;

            eulerAngles = new Vector3(0, 0, (-AnglePerItem * i - _spaceAngle * spaceIdx) + _angleOffset);

            rect.anchoredPosition = anchoredPosition;
            rect.eulerAngles = eulerAngles;

            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = cellSize;
        }
    }
}
