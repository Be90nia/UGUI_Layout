using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutCarousel : LayoutBase
{
    [SerializeField]
    private float _carouselRadius = 150;
    [SerializeField]
    private float _angleOffset = 0;

    private int _focusIndex = 0;

    public float CarouseRadius
    {
        get => _carouselRadius;
        set => _carouselRadius = value;
    }

    public float AngleOffset
    {
        get => _angleOffset;
        set => _angleOffset = value;
    }

    public List<RectTransform> RectChildren => _rectChildren;

    protected float AnglePerItem
    {
        get
        {
            if (_rectChildren.Count > 0)
                return 360f / (float) _rectChildren.Count;
            return 0;
        }
    }
    protected override void OnEnable()
    {
        _focusIndex = 0;
        base.Init();
        if (_rectChildren.Count > 0)
            SetCellsAlongAxis(AngleOffset);

    }

    protected override void OnDisable()
    {
        base.Disable();
    }

    public override void CalculateLayoutInputVertical()
    {
        if (_rectChildren.Count > 0)
            SetCellsAlongAxis(AngleOffset);
    }

    public override void SetLayoutVertical()
    {
    }

    public override void SetLayoutHorizontal()
    {
    }

    void Update()
    {
        if (_rectChildren.Count > 0)
            SetCellsAlongAxis(AngleOffset);
    }
    public void SetCellsAlongAxis(float value)
    {
//        if (Mathf.Abs(value) >= AnglePerItem)
//        {
//            if (value > 0)
//            {
//                _focusIndex = _focusIndex - 1;
//                _focusIndex = (_focusIndex < 0) ? _rectChildren.Count - 1 : _focusIndex;
//
//                value -= AnglePerItem;
//            }
//            else
//            {
//                _focusIndex = _focusIndex + 1;
//                _focusIndex = (_focusIndex >= _rectChildren.Count) ? 0 : _focusIndex;
//
//                value += AnglePerItem;
//            }
//        }
        for (int i = 0; i < _rectChildren.Count; i++)
        {
            RectTransform rect = _rectChildren[i];

            Vector3 anchoredPosition = Vector3.zero;
            float itemAngle = i * AnglePerItem;

            anchoredPosition.x += Mathf.Cos((itemAngle - 90 + value) * Mathf.Deg2Rad) * _carouselRadius;
            anchoredPosition.z += Mathf.Sin((itemAngle - 90 + value) * Mathf.Deg2Rad) * _carouselRadius;
            rect.anchoredPosition3D = anchoredPosition;

            Vector3 eulerAngles = new Vector3(0, -itemAngle - value, 0);
            rect.eulerAngles = eulerAngles;
        }
    }
}
