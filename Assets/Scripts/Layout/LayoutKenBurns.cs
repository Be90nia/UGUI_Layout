using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class LayoutKenBurns : LayoutBase
{
    public enum ZoomType
    {
        ZoomIn,
        ZoomOut,
    }

    [SerializeField] 
    private ZoomType _zoomType = ZoomType.ZoomIn;
    [SerializeField]
    private KenBurnsZoomIn _kenBurnsZoomIn = new KenBurnsZoomIn();
    [SerializeField]
    private KenBurnsZoomOut _kenBurnsZoomOut = new KenBurnsZoomOut();
    [SerializeField] 
    private float _displayDuration = 4;
    [SerializeField] 
    private float _transitionDuration = 2;

    private bool _isRunEffect = false;
    private bool _isPlay = true;

    private int _curItemIdx = 0;
    private int _nextItemIdx = 0;

    [System.NonSerialized] 
    private new List<RectTransform> _rectChildren = new List<RectTransform>();

    public new List<RectTransform> RectChildren => _rectChildren;

    public int CurItemID => _curItemIdx;

    public ZoomType ZoomValue
    {
        get => _zoomType;
        set => _zoomType = value;
    }

    public float DisplayDuration
    {
        get => _displayDuration;
        set => _displayDuration = value;
    }

    public float TransitionDuration
    {
        get => _transitionDuration;
        set => _transitionDuration = value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _curItemIdx = 0;
        Init();
    }

    public override void Init()
    {
        _rectChildren.Clear();
        for (int i = 0; i < rectTransform.childCount; i++)
        {
            var rect = rectTransform.GetChild(i) as RectTransform;
            if (rect != null)
            {
                rect.anchoredPosition3D = Vector3.zero;
                rect.eulerAngles = Vector3.zero;
                rect.gameObject.SetActive(_rectChildren.Count == 0);
                _rectChildren.Add(rect);
                SetAlpha(i, 1);
            }
        }
    }

    protected override void Start()
    {
        _curItemIdx = 0;
        Init();
    }

    protected override void OnDisable()
    {
        for (int i = 0; i < _rectChildren.Count; i++)
        {
            _rectChildren[i].localScale = Vector3.one;
            _rectChildren[i].localPosition = Vector3.zero;
            _rectChildren[i].transform.SetSiblingIndex(i);
            _rectChildren[i].gameObject.SetActive(true);
            SetAlpha(i, 1);
        }
        _rectChildren.Clear();
    }

    public override void CalculateLayoutInputVertical()
    {
        if (_isPlay)
        {
            if (!_isRunEffect && _rectChildren.Count > 0)
            {
                if (_zoomType == ZoomType.ZoomIn)
                {
                    OnZoomIn();
                }
                else if (_zoomType == ZoomType.ZoomOut)
                {
                    OnZoomOut();
                }

                _isRunEffect = true;
            }
        }
    }

    void Update()
    {
        if (_isPlay)
        {
            if (!_isRunEffect && _rectChildren.Count > 0)
            {
                if (_zoomType == ZoomType.ZoomIn)
                    OnZoomIn();
                else if (_zoomType == ZoomType.ZoomOut)
                    OnZoomOut();

                _isRunEffect = true;
            }
        }
    }

    private void OnBeforeTransition()
    {
        _nextItemIdx = _curItemIdx + 1;
        _nextItemIdx = _nextItemIdx >= _rectChildren.Count ? 0 : _nextItemIdx;

        _rectChildren[_nextItemIdx].gameObject.SetActive(true);
        SetAlpha(_nextItemIdx, 0);
    }

    private void OnAfterTransition()
    {
        _rectChildren[_curItemIdx].gameObject.SetActive(false);
        _curItemIdx = _nextItemIdx;
        _rectChildren[_nextItemIdx].gameObject.SetActive(true);
        SetAlpha(_nextItemIdx, 1);
        _isRunEffect = false;
    }

    private void OnCompleteZoomInAnimation()
    {
        Vector3 nextItemEndScale = Vector3.one * _kenBurnsZoomIn.NextItemEndScale;
        _rectChildren[_nextItemIdx].localScale = Vector3.one * _kenBurnsZoomIn.NextItemStartScale;

        float curAlphaValue = _kenBurnsZoomIn.CurAlphaValue;
        DOTween.To(() => curAlphaValue, r => curAlphaValue = r, 0, _transitionDuration).OnUpdate(() =>
        {
            SetAlpha(_curItemIdx, curAlphaValue);
        });

        Vector3 curItemEndScale = Vector3.one * _kenBurnsZoomIn.CurItemStartScale;
        DOTween.To(() => _rectChildren[_curItemIdx].localScale, r => _rectChildren[_curItemIdx].localScale = r,
            curItemEndScale, _transitionDuration);

        float nextAlphaValue = _kenBurnsZoomIn.NextAlphaValue;
        DOTween.To(() => nextAlphaValue, r => nextAlphaValue = r, 1, _transitionDuration).OnUpdate(() =>
        {
            SetAlpha(_nextItemIdx, nextAlphaValue);
        });

        DOTween.To(() => _rectChildren[_nextItemIdx].localScale, r => _rectChildren[_nextItemIdx].localScale = r,
            nextItemEndScale, _transitionDuration).OnComplete(() =>
        {
            SetAlpha(_curItemIdx, 1.0f);
            _rectChildren[_curItemIdx].localScale = Vector3.one;

            OnAfterTransition();
        });
    }

    private void OnCompleteZoomOutAnimation()
    {
        _rectChildren[_nextItemIdx].localScale = Vector3.one * _kenBurnsZoomOut.CurItemStartScale;

        float curAlphaValue = _kenBurnsZoomOut.CurAlphaValue;
        DOTween.To(() => curAlphaValue, r => curAlphaValue = r, 0, _transitionDuration).OnUpdate(() =>
        {
            SetAlpha(_curItemIdx, curAlphaValue);
        });

        Vector3 curItemEndScale = Vector3.one * _kenBurnsZoomOut.CurItemEndScale;
        DOTween.To(() => _rectChildren[_curItemIdx].localScale, r => _rectChildren[_curItemIdx].localScale = r,
            curItemEndScale, _transitionDuration);

        float nextAlphaValue = _kenBurnsZoomOut.NextAlphaValue;
        DOTween.To(() => nextAlphaValue, r => nextAlphaValue = r, 1, _transitionDuration).OnUpdate(() =>
        {
            SetAlpha(_nextItemIdx, nextAlphaValue);
        }).OnComplete(() =>
        {
            SetAlpha(_curItemIdx, 1.0f);
            _rectChildren[_curItemIdx].localScale = Vector3.one *_kenBurnsZoomOut.CurItemStartScale;
            OnAfterTransition();
        });
    }


    private void OnZoomIn()
    {
        OnBeforeTransition();

        Vector3 curItemEndScale = Vector3.one * _kenBurnsZoomIn.CurItemEndScale;
        DOTween.To(() => _rectChildren[_curItemIdx].localScale, r => _rectChildren[_curItemIdx].localScale = r,
            curItemEndScale, _displayDuration).OnComplete(OnCompleteZoomInAnimation);
    }

    private void OnZoomOut()
    {
        OnBeforeTransition();

        _rectChildren[_curItemIdx].localScale = Vector3.one * _kenBurnsZoomOut.CurItemStartScale;

        Vector3 curItemEndScale = Vector3.one;
        DOTween.To(() => _rectChildren[_curItemIdx].localScale, r => _rectChildren[_curItemIdx].localScale = r,
            curItemEndScale, _displayDuration).OnComplete(OnCompleteZoomOutAnimation);
    }

    protected void SetAlpha(int itemIdx, float alpha)
    {
        // 暂时仅支持图片
        Image image = _rectChildren[itemIdx].GetComponent<Image>();
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }

    public void Play()
    {
        _isPlay = true;
    }

    public void Pause()
    {
        _isPlay = false;
    }
}
[Serializable]
public class KenBurnsZoomIn
{
    public float CurItemEndScale = 1.3f;
    public float CurItemStartScale = 1.5f;

    public float NextItemEndScale = 1f;
    public float NextItemStartScale = 0.6f;
   
    public float CurAlphaValue = 1f;
    public float NextAlphaValue = 0f;
}

[Serializable]
public class KenBurnsZoomOut
{
    public float CurItemEndScale = 0.8f;
    public float CurItemStartScale = 1.3f;

    public float NextItemEndScale = 1f;
    public float NextItemStartScale = 1.3f;

    public float CurAlphaValue = 1f;
    public float NextAlphaValue = 0f;
}

