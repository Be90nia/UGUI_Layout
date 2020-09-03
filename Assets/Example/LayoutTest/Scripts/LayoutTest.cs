using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutTest : MonoBehaviour
{
    public LayoutSwap LayoutSwap;

    public LayoutKenBurns LayoutKenBurns;

    public LayoutAssetFlow LayoutAssetFlow;

    public LayoutHelix LayoutHelix;

    public LayoutCarousel LayoutCarousel;

    public Slider AsseFlowSlider;

    private Vector2 startPosHelix;
    private Vector2 startPosCarousel;

    // Start is called before the first frame update
    void Start()
    {
        if (LayoutSwap == null)
            LayoutSwap = GetComponentInChildren<LayoutSwap>();
        if (LayoutKenBurns == null)
            LayoutKenBurns = GetComponentInChildren<LayoutKenBurns>();
        if (LayoutAssetFlow == null)
            LayoutAssetFlow = GameObject.FindObjectOfType<LayoutAssetFlow>();
        if (LayoutAssetFlow != null)
        {
            if (LayoutAssetFlow.AssetFlowType == AssetFlowEnum.Loop)
            {
                AsseFlowSlider.minValue = 0;
                AsseFlowSlider.maxValue = 1;
            }
            else if(LayoutAssetFlow.AssetFlowType == AssetFlowEnum.Clamp)
            {
                AsseFlowSlider.minValue = -1;
                AsseFlowSlider.maxValue = 1;
            }
        }

        if (LayoutHelix == null)
            LayoutHelix = FindObjectOfType<LayoutHelix>();
        if (LayoutCarousel == null)
            LayoutCarousel = FindObjectOfType<LayoutCarousel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (LayoutSwap != null)
                LayoutSwap.SwapTransition(true);
            if (LayoutKenBurns != null)
                LayoutKenBurns.Pause();
            if (LayoutHelix != null)
            {
                startPosHelix = Input.mousePosition;
            }
            if (LayoutCarousel != null)
            {
                startPosCarousel = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (LayoutSwap != null)
                LayoutSwap.SwapTransition(false);
            if (LayoutKenBurns != null)
                LayoutKenBurns.Play();
        }

        if (Input.GetMouseButton(0))
        {
            if (LayoutHelix != null)
            {
               LayoutHelix.AngleOffset = Input.mousePosition.x - startPosHelix.x;
            }   
            if (LayoutCarousel != null)
            {
                LayoutCarousel.AngleOffset = Input.mousePosition.x - startPosCarousel.x;
            }
        }

    }

    public void AssetFlowValue(float value)
    {
        LayoutAssetFlow.Value = value;
    }
}
