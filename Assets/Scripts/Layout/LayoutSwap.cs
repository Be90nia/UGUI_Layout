using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class LayoutSwap : LayoutBase
{
    [SerializeField] 
    private Transition _transitionType = Transition.Flip;

    #region Flip

    [SerializeField]
    protected SwapFlip _swapFlip = new SwapFlip();
    public SwapFlip SwapFlip => _swapFlip;

    #endregion

    #region Side
    [SerializeField]
    protected SwapSide _swapSide = new SwapSide();
    public SwapSide SwapSide => _swapSide;
    #endregion

    #region Fade
    [SerializeField]
    protected SwapFade _swapFade = new SwapFade();
    public SwapFade SwapFade => _swapFade;
    #endregion

    #region Zoom In
    [SerializeField]
    protected SwapZoom _swapZoomIn = new SwapZoom();
    public SwapZoom SwapZoomIn => _swapZoomIn;
    #endregion

    #region Zoom Out
    [SerializeField]
    protected SwapZoom _swapZoomOut = new SwapZoom(Vector3.one, new Vector3(1.3f, 1.3f, 1.3f),1,0);
    public SwapZoom SwapZoomOut => _swapZoomOut;
    #endregion

    #region Card Stack
    [SerializeField]
    protected SwapCardStack _swapCardStack = new SwapCardStack();
    public SwapCardStack SwapCardStack => _swapCardStack;
    #endregion

    [SerializeField]
    private int _curItemIdx = 0;
    private int _nextItemIdx = 0;
    [SerializeField] 
    private float _swapTime = 0.5f;
    private float _curTransValue = 0;
    protected Vector3 _originPos = Vector3.zero;

    /// <summary>
    /// 是否正在播放动画
    /// </summary>
    protected bool _isPlay = false;
    /// <summary>
    /// 当前序列的ID
    /// </summary>
    public int CurItemID
    {
        get => _curItemIdx;
        set => _curItemIdx = value;
    }
    /// <summary>
    /// 动画时间
    /// </summary>
    public float SwapTime
    {
        get => _swapTime;
        set => _swapTime = value;
    }
    /// <summary>
    /// 下一个序列的ID
    /// </summary>
    public int NextItemIdx => _nextItemIdx;

    public Transition TransitionType
    {
        get => _transitionType;
        set
        {
            _transitionType = value;
            RestAll();
        }
    }

    protected override void Start()
    {
    }

//    private void Update()
//    {
//        if (Input.GetMouseButtonDown(0)) OnTransition(true);
//    }


    protected override void OnEnable()
    {
        Init();
    }

    public override void Init()
    {
        var childs = gameObject.GetComponentsInChildren<RectTransform>();
        if (_rectChildren.Count == 0)
        {
            _rectChildren.Clear();
            for (var i = 0; i < childs.Length; i++)
                if (childs[i] != rectTransform)
                {
                    _rectChildren.Add(childs[i]);
                    childs[i].anchoredPosition3D = _originPos;
                    childs[i].eulerAngles = Vector3.zero;
                    childs[i].gameObject.SetActive(i - 1 == _curItemIdx);
                }
        }
    }

    protected override void OnDisable()
    {
        Disable();
    }

    protected override void Disable()
    {
        for (var i = 0; i < _rectChildren.Count; i++)
            _rectChildren[i].gameObject.SetActive(true);
        _rectChildren.Clear();
    }

    public override void CalculateLayoutInputHorizontal()
    {
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

    protected override void SetChildRectTracker()
    {
    }
    /// <summary>
    /// 切换上一张或者切换下一张
    /// </summary>
    /// <param name="bNext">true 切换下一张  false 切换上一张</param>
    public void SwapTransition(bool bNext = true)
    {
        OnTransition(bNext);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        //Debug.Log("OnValidate");
    }
#endif

    /// <summary>
    /// 切换效果执行前调用
    /// </summary>
    /// <param name="bNext">true 切换下一张  false 切换上一张</param>
    /// <param name="bShowNext">是否显示下/上一张</param>
    private void OnBeforeTransition(bool bNext, bool bShowNext = true)
    {
        _nextItemIdx = bNext ? _curItemIdx + 1 : _curItemIdx - 1;
        _nextItemIdx = _nextItemIdx >= _rectChildren.Count ? 0 : _nextItemIdx;
        _nextItemIdx = _nextItemIdx < 0 ? _rectChildren.Count - 1 : _nextItemIdx;

        if (bShowNext)
            _rectChildren[_nextItemIdx].gameObject.SetActive(true);
    }
    /// <summary>
    /// 隐藏上一张图
    /// </summary>
    private void OnAfterTransition()
    {
        _rectChildren[_curItemIdx].gameObject.SetActive(false);
        _curItemIdx = _nextItemIdx;
    }
    /// <summary>
    /// 切换下一张图
    /// </summary>
    /// <param name="bNext"></param>
    private void OnTransition(bool bNext)
    {
        if (_isPlay)
            return;
        switch (_transitionType)
        {
            case Transition.Flip:
                OnFlip(bNext);
                break;
            case Transition.Fade:
                OnFade(bNext);
                break;
            case Transition.Side:
                OnSide(bNext);
                break;
            case Transition.ZoomIn:
                OnZoomIn(bNext);
                break;
            case Transition.ZoomOut:
                OnZoomOut(bNext);
                break;
            case Transition.CardStack:
                OnCardStack(bNext);
                break;
        }
    }

    private void OnFlip(bool bNext)
    {
        OnBeforeTransition(bNext, false);
        var curAlphaValue = 1.0f;

        _isPlay = true;
        DOTween.To(() => curAlphaValue, r => curAlphaValue = r, _swapFlip.ImageAlpha, _swapTime)
            .OnUpdate(() => { SetAlpha(_curItemIdx, curAlphaValue); });

        _curTransValue = 0;
        DOTween.To(() => _curTransValue, r => _curTransValue = r, _swapFlip.ImageAngle, _swapTime).OnUpdate(OnUpdateFlipAnimation)
            .OnComplete(OnCompleteFlipAnimation);
    }

    private void OnSide(bool bNext)
    {
        OnBeforeTransition(bNext);
        _isPlay = true;
        float rightX = _swapSide.Width / 2;
        var leftX = -rightX;

        var curItemEndX = bNext ? leftX : rightX;
        var curEndPosition = new Vector2(curItemEndX, 0);

        var nextItemStartX = bNext ? rightX : leftX;
        var nextEndPosition = Vector2.zero;

        _rectChildren[_nextItemIdx].anchoredPosition = new Vector2(nextItemStartX, 0);
       SetAlpha(_nextItemIdx,1);

        DOTween.To(() => _rectChildren[_curItemIdx].anchoredPosition,
            r => _rectChildren[_curItemIdx].anchoredPosition = r, curEndPosition, _swapTime);
        DOTween.To(() => _rectChildren[_nextItemIdx].anchoredPosition,
            r => _rectChildren[_nextItemIdx].anchoredPosition = r, nextEndPosition, _swapTime).OnComplete(() =>
        {
            _rectChildren[_curItemIdx].anchoredPosition = Vector2.zero;
            OnAfterTransition();
            _isPlay = false;
        });
    }

    private void OnFade(bool bNext)
    {
        OnBeforeTransition(bNext);
        _isPlay = true;
        var curAlphaValue = _swapFade.EndAlpha;
        DOTween.To(() => curAlphaValue, r => curAlphaValue = r, _swapFade.StartAlpha, _swapTime)
            .OnUpdate(() => { SetAlpha(_curItemIdx, curAlphaValue); });

        var nextAlphaValue = _swapFade.StartAlpha;
        DOTween.To(() => nextAlphaValue, r => nextAlphaValue = r, _swapFade.EndAlpha, _swapTime)
            .OnUpdate(() => { SetAlpha(_nextItemIdx, nextAlphaValue); }).OnComplete(() =>
            {
                SetAlpha(_curItemIdx, 1.0f);
                OnAfterTransition();
                _isPlay = false;
            });
    }

    private void OnZoomIn(bool bNext)
    {
        OnBeforeTransition(bNext);
        _isPlay = true;
        Vector3 curItemEndScale = _swapZoomIn.StartScale;
        _rectChildren[_curItemIdx].localScale = new Vector3(1.0f, 1.0f, 1.0f);

        Vector3 nextItemEndScale = _swapZoomIn.EndScale;
        _rectChildren[_nextItemIdx].localScale = Vector3.zero;

        float curAlphaValue = _swapZoomIn.CurAlphaValue;
        DOTween.To(() => curAlphaValue, r => curAlphaValue = r, 0, _swapTime).OnUpdate(() => {
            SetAlpha(_curItemIdx, curAlphaValue);
        });
        DOTween.To(() => _rectChildren[_curItemIdx].localScale, r => _rectChildren[_curItemIdx].localScale = r, curItemEndScale, _swapTime);

        float nextAlphaValue = _swapZoomIn.NextAlphaValue;
        DOTween.To(() => nextAlphaValue, r => nextAlphaValue = r, 1, _swapTime).OnUpdate(() =>
        {
            SetAlpha(_nextItemIdx, nextAlphaValue);
        });
        DOTween.To(() => _rectChildren[_nextItemIdx].localScale, r => _rectChildren[_nextItemIdx].localScale = r, nextItemEndScale, _swapTime).OnComplete(() => {
            SetAlpha(_curItemIdx, 1.0f);
            _rectChildren[_curItemIdx].localScale = new Vector3(1.0f, 1.0f, 1.0f);
            OnAfterTransition();
            _isPlay = false;
        });
    }

    private void OnZoomOut(bool bNext)
    {
        OnBeforeTransition(bNext);
        _isPlay = true;
        var curItemEndScale = _swapZoomOut.StartScale;
        _rectChildren[_curItemIdx].localScale = Vector3.zero;

        var nextItemEndScale = _swapZoomOut.StartScale;
        _rectChildren[_nextItemIdx].localScale = _swapZoomOut.EndScale;

        var curAlphaValue = _swapZoomOut.CurAlphaValue;
        DOTween.To(() => curAlphaValue, r => curAlphaValue = r, 0, _swapTime)
            .OnUpdate(() => { SetAlpha(_curItemIdx, curAlphaValue); });
        DOTween.To(() => _rectChildren[_curItemIdx].localScale, r => _rectChildren[_curItemIdx].localScale = r,
            curItemEndScale, _swapTime);

        float nextAlphaValue = _swapZoomOut.NextAlphaValue;
        DOTween.To(() => nextAlphaValue, r => nextAlphaValue = r, 1, _swapTime).OnUpdate(() =>
        {
            SetAlpha(_nextItemIdx, nextAlphaValue);
        });
        DOTween.To(() => _rectChildren[_nextItemIdx].localScale, r => _rectChildren[_nextItemIdx].localScale = r,
            nextItemEndScale, _swapTime).OnComplete(() =>
        {
            SetAlpha(_curItemIdx, 1.0f);
            OnAfterTransition();
            _isPlay = false;
        });
    }

    private void OnCardStack(bool bNext)
    {
        OnBeforeTransition(bNext);
        _isPlay = true;
        var curIndex = _rectChildren[_curItemIdx].GetSiblingIndex();
        var nextIndex = _rectChildren[_nextItemIdx].GetSiblingIndex();
        _rectChildren[_nextItemIdx].SetSiblingIndex(curIndex - 1);

        var curAlphaValue = 1.0f;
        DOTween.To(() => curAlphaValue, r => curAlphaValue = r, _swapCardStack.EndAlpha, _swapTime)
            .OnUpdate(() => { SetAlpha(_curItemIdx, curAlphaValue); });

        float curTransValue = 0;
        DOTween.To(() => curTransValue, r => curTransValue = r, _swapCardStack.TransValue, _swapTime).OnUpdate(() =>
        {
            var curEulerAngles = _rectChildren[_curItemIdx].transform.eulerAngles;
            curEulerAngles.z = curTransValue;
            _rectChildren[_curItemIdx].transform.eulerAngles = curEulerAngles;
        });

        var curEndPosition = new Vector2(_swapCardStack.Width / 2f, _swapCardStack.Height / 2f);
        if (!_swapCardStack.IsChangeDirY)
            curEndPosition.y = -curEndPosition.y;
        if (_swapCardStack.IsChangeDirX)
            curEndPosition.x = -curEndPosition.x;


        DOTween.To(() => _rectChildren[_curItemIdx].anchoredPosition,
            r => _rectChildren[_curItemIdx].anchoredPosition = r, curEndPosition, _swapTime).OnComplete(() =>
        {
            SetAlpha(_curItemIdx, 1.0f);
            _rectChildren[_curItemIdx].eulerAngles = Vector3.zero;
            _rectChildren[_curItemIdx].anchoredPosition = Vector2.zero;
            _rectChildren[_nextItemIdx].SetSiblingIndex(nextIndex);

            OnAfterTransition();
            _isPlay = false;
        });
    }

    private void OnUpdateFlipAnimation()
    {
        var curEulerAngles = _rectChildren[_curItemIdx].transform.eulerAngles;
        curEulerAngles.y = _curTransValue;
        _rectChildren[_curItemIdx].transform.eulerAngles = curEulerAngles;
    }

    private void OnCompleteFlipAnimation()
    {
        _rectChildren[_curItemIdx].eulerAngles = Vector3.zero;

        OnAfterTransition();

        var curAlphaValue = 0.5f;
        var localScale = _rectChildren[_curItemIdx].localScale;
        localScale.x = -localScale.x;
        _curTransValue = -90;

        _rectChildren[_curItemIdx].eulerAngles = new Vector3(0, _curTransValue, 0);
        _rectChildren[_curItemIdx].localScale = localScale;
        SetAlpha(_curItemIdx, curAlphaValue);
        _rectChildren[_curItemIdx].gameObject.SetActive(true);

        DOTween.To(() => curAlphaValue, r => curAlphaValue = r, 1, _swapTime)
            .OnUpdate(() => { SetAlpha(_curItemIdx, curAlphaValue); });

        DOTween.To(() => _curTransValue, r => _curTransValue = r, -180, _swapTime).OnUpdate(OnUpdateFlipAnimation)
            .OnComplete(() =>
            {
                _rectChildren[_curItemIdx].eulerAngles = Vector3.zero;
                var scaleX = _rectChildren[_curItemIdx].localScale;
                scaleX.x = -scaleX.x;
                _rectChildren[_curItemIdx].localScale = scaleX;
                _isPlay = false;
            });
    }
    /// <summary>
    /// 设置图片的Alpha值
    /// </summary>
    /// <param name="itemIdx"></param>
    /// <param name="alpha"></param>
    protected void SetAlpha(int itemIdx, float alpha)
    {
        // 暂时仅支持图片
        var image = _rectChildren[itemIdx].GetComponent<Image>();
        if (image != null)
        {
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
    /// <summary>
    /// 重置
    /// </summary>
    public void RestAll()
    {
        for (int i = 0; i < _rectChildren.Count; i++)
        {
            SetAlpha(i, 1.0f);
            _rectChildren[i].anchoredPosition3D = _originPos;
            _rectChildren[i].eulerAngles = Vector3.zero;
            _rectChildren[i].gameObject.SetActive(i == _curItemIdx);
        }

    }
}
[Serializable]
public class SwapFlip
{
    [Range(0, 1)]
    public float ImageAlpha = 0.5f;
    [Range(-360, 360)]
    public float ImageAngle = -90;
}
[Serializable]
public class SwapSide
{
    public float Width = 1920f;
}
[Serializable]
public class SwapFade
{
    [Range(0,1)]
    public float EndAlpha = 1f;
    [Range(0,1)]
    public float StartAlpha = 0f;
}
[Serializable]
public class SwapZoom
{
    public Vector3 StartScale = Vector3.one;
    public Vector3 EndScale = new Vector3(1.3f, 1.3f, 1.3f);
    [Range(0,1)]
    public float CurAlphaValue = 1;
    [Range(0, 1)]
    public float NextAlphaValue = 0;

    public SwapZoom()
    {

    }

    public SwapZoom(Vector3 startScale, Vector3 endScale, float curAlpha, float nextAlpha)
    {
        StartScale = startScale;
        EndScale = endScale;
        CurAlphaValue = curAlpha;
        NextAlphaValue = nextAlpha;
    }
}
[Serializable]
public class SwapCardStack
{
    public float Width = 1920;
    public float Height = 1020;
    [Range(-360,360)]
    public float TransValue = -30;
    [Range(0,1)]
    public float EndAlpha = 0;
    public bool IsChangeDirX = false;
    public bool IsChangeDirY = false;
}
public enum Transition
{
    None,
    Flip,
    Side,
    Fade,
    ZoomIn,
    ZoomOut,
    CardStack,
}