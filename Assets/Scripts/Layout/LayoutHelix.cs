using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayoutHelix : LayoutBase
{
    [SerializeField]
    private float _radius = 200;
    [SerializeField]
    private float _itemOffset = 0.5f;
    [SerializeField]
    private float _itemAngle = 20;
    [SerializeField]
    private float _angleOffset = 0;

    private int _focusIndex = 0;


    public float Radius
    {
        get => _radius;
        set => _radius = value;
    }
    public float ItemOffset
    {
        get => _itemOffset;
        set => _itemOffset = value;
    }

    public float ItemAngle
    {
        get => _itemAngle;
        set => _itemAngle = value;
    }

    public float AngleOffset
    {
        get => _angleOffset;
        set => _angleOffset = value;
    }


    protected override void OnEnable()
    {
        base.Init();
//        SetUIMaterial();
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

    public override void SetLayoutHorizontal()
    {

    }

    public override void SetLayoutVertical()
    {

    }

    void Update()
    {
        if (_rectChildren.Count > 0)
            SetCellsAlongAxis(AngleOffset);
    }
    private void SetCellsAlongAxis(float value)
    {
//        if (Mathf.Abs(value) >= _itemAngle)
//        {
//            if (value > 0)
//            {
//                if (_focusIndex > 0)
//                {
//                    _focusIndex = _focusIndex - 1;
//                    value -= _itemAngle;
//                }
//            }
//            else
//            {
//                if (_focusIndex + 1 < _rectChildren.Count)
//                {
//                    _focusIndex = _focusIndex + 1;
//                    value += _itemAngle;
//                }
//            }
//        }

        for (int i = 0; i < _rectChildren.Count; i++)
        {
            Vector3 anchoredPosition = Vector3.zero;

            float curT = Mathf.Deg2Rad * (_itemAngle * i + value);
            anchoredPosition += new Vector3(_radius * Mathf.Sin(curT), _radius * _itemOffset * curT, -_radius * Mathf.Cos(curT));
            _rectChildren[i].anchoredPosition3D = anchoredPosition;

            float angle = _itemAngle *  - i - value;
            Vector3 eulerAngles = new Vector3(0, angle, 0);
            _rectChildren[i].eulerAngles = eulerAngles;
        }
    }
}
