using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class LayoutPinBoard : LayoutBase
{
    public enum MoveDirection
    {
        All = 0,
        Horizontal = 1,
        Vertical
    }

    [SerializeField] 
    private bool _allowMove = true;
    [SerializeField] 
    private PinBoardMove _pinBoardMove = new PinBoardMove();
    [SerializeField]
    private bool _allowRotate = true;
    [SerializeField]
    private PinBoardRotate _pinBoardRotate = new PinBoardRotate();
    [SerializeField] 
    private bool _allowAnimation = true;
    [SerializeField] 
    private PinBoardScale _pinBoardScale = new PinBoardScale();

    private RectTransform _rectTransform;
    private Vector3 _oldLocalScale;

    private bool _isDragging = false;

    private Vector2 _dragStartPose;
    private Vector2 _dragEndPose;

    public bool AllowMove
    {
        get => _allowMove;
        set => _allowMove = value;
    }
    public bool AllowRotate
    {
        get => _allowRotate;
        set => _allowRotate = value;
    }
    public bool AllowAnimation
    {
        get => _allowAnimation;
        set => _allowAnimation = value;
    }


    public PinBoardMove PinBoardMove => _pinBoardMove;

    public PinBoardRotate PinBoardRotate => _pinBoardRotate;

    public PinBoardScale PinBoardScale => _pinBoardScale;
    // Start is called before the first frame update
    protected override void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _oldLocalScale = _rectTransform.localScale;

        // 旋转范围暂定为矩形外侧
        float x = _rectTransform.rect.x + _rectTransform.rect.width * _pinBoardRotate.RotateX;
        float y = _rectTransform.rect.y + _rectTransform.rect.height * _pinBoardRotate.RotateY;
        float w = _rectTransform.rect.width * (1 - 2 * _pinBoardRotate.RotateX);
        float h = _rectTransform.rect.height * (1 - 2 * _pinBoardRotate.RotateY);
        _pinBoardMove.MoveRect = new Rect(x, y, w, h);

        Vector3 destScale = _oldLocalScale * _pinBoardScale.AnimationScale;
        _pinBoardScale.ScaleTweener = InitTweener(DOTween
            .To(() => _rectTransform.localScale, r => _rectTransform.localScale = r, destScale,
                _pinBoardScale.AnimationScale)
            .SetEase(Ease.Linear));

        _pinBoardMove.MoveTweener = InitTweener(DOTween
            .To(() => _rectTransform.anchoredPosition, r => _rectTransform.anchoredPosition = r, _dragEndPose,
                _pinBoardMove.MoveTime)
            .SetEase(Ease.OutCirc));

    }

    // Update is called once per frame
    void Update()
    {
        bool buttonDown = false;
        bool buttonUp = false;
        Vector2 screenPos = Vector2.zero;

        screenPos = Input.mousePosition;
        buttonDown = Input.GetMouseButtonDown(0);
        buttonUp = Input.GetMouseButtonUp(0);

        UpdateInput(screenPos, buttonDown, buttonUp);
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

    }

    public override void SetLayoutVertical()
    {
    }

    private void UpdateInput(Vector2 screenPos, bool buttonDown, bool buttonUp)
    {
        Vector2 localPoint = Vector2.zero;
        if (_isDragging)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform.parent.GetComponent<RectTransform>(),
                screenPos, null, out localPoint);

            if (_pinBoardMove.IsMoving)
            {
                if (localPoint != _dragStartPose)
                {
                    Vector2 dir = localPoint - _dragStartPose;
                    if (_pinBoardMove.MoveDirection == MoveDirection.Horizontal)
                    {
                        dir.y = 0;
                    }
                    else if (_pinBoardMove.MoveDirection == MoveDirection.Vertical)
                    {
                        dir.x = 0;
                    }

                    _dragEndPose += dir;
                    _pinBoardMove.MoveTweener.ChangeValues(_rectTransform.anchoredPosition, _dragEndPose,
                        _pinBoardMove.MoveTime);
                    _pinBoardMove.MoveTweener.PlayForward();
                    _dragStartPose = localPoint;
                }
            }
            else if (_pinBoardRotate.IsRotation)
            {
                Vector2 from = _dragStartPose - _rectTransform.anchoredPosition;
                Vector2 to = localPoint - _rectTransform.anchoredPosition;
                float angle = Vector2.SignedAngle(from, to);
                Vector3 eulerAngles = _rectTransform.eulerAngles + new Vector3(0, 0, angle);
                _rectTransform.eulerAngles = eulerAngles;
                _dragStartPose = localPoint;
            }
        }

        if (!_isDragging && buttonDown)
        {
            for (int i = 0; i < _rectTransform.childCount; i++)
            {
                var rect = _rectTransform.GetChild(i) as RectTransform;
                if (rect == null || !rect.gameObject.activeInHierarchy)
                {
                    continue;
                }

                if (RectTransformUtility.RectangleContainsScreenPoint(rect, screenPos))
                {
                    _isDragging = true;

                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        _rectTransform.parent.GetComponent<RectTransform>(), screenPos, null, out localPoint);
                    if (_allowRotate)
                    {
                        if (!_pinBoardMove.MoveRect.Contains(localPoint - _rectTransform.anchoredPosition))
                        {
                            _pinBoardRotate.IsRotation = true;
                        }
                    }

                    if (_allowMove && !_pinBoardRotate.IsRotation)
                    {
                        _pinBoardMove.IsMoving = true;
                    }

                    _dragStartPose = localPoint;
                    _dragEndPose = _rectTransform.anchoredPosition;

                    if (_allowAnimation)
                    {
                        _pinBoardScale.IsAnimation = true;
                        _pinBoardScale.ScaleTweener.PlayForward();
                    }

                    break;
                }
            }
        }

        if (_isDragging && buttonUp)
        {
            if (_pinBoardScale.IsAnimation)
            {
                _pinBoardScale.ScaleTweener.PlayBackwards();
            }

            _isDragging = false;
            _pinBoardMove.IsMoving = false;
            _pinBoardRotate.IsRotation = false;
            _pinBoardScale.IsAnimation = false;
        }
    }

    private Tweener InitTweener(Tweener tweener, bool isPlay = false, bool isKill = false)
    {
        tweener.SetAutoKill(isKill);
        if (!isPlay)
            tweener.Pause();
        return tweener;
    }
}

[Serializable]
public class PinBoardMove
{
    public LayoutPinBoard.MoveDirection MoveDirection = LayoutPinBoard.MoveDirection.All;
    public bool IsMoving = false;
    [HideInInspector]
    public Rect MoveRect; // 移动范围
    public float MoveTime = 0.6f;
    public Tweener MoveTweener;
}
[Serializable]
public class PinBoardRotate
{
    public float RotateX = 1f / 6f; // 旋转水平范围
    public float RotateY = 1f / 8f; // 旋转垂直范围
    public bool IsRotation = false;
}
[Serializable]
public class PinBoardScale
{
    public float AnimationScale = 1.07f;
    public bool IsAnimation = false;
    public float AnimationTime = 0.4f;
    public Tweener ScaleTweener;
}
