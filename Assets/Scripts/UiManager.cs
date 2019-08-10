using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Texture2D cursorTex;
    public Vector2 hotspot;

    private float _performance = 0.5f;

    public float Performance
    {
        get => _performance;
        set
        {
            if (value < _performance)
                imgPerformanceBlink.color = Color.Lerp(Color.white, Color.red, .5f);
            else if (value > _performance)
                imgPerformanceBlink.color = Color.Lerp(Color.white, Color.green, .5f);
            
            _performance = Mathf.Clamp01(value);
            sldPerformance.value = _performance;
            UpdatePP(_performance);
        }
    }

    public Slider sldPerformance;
    public Image imgPerformanceBlink;
    
    private void Awake()
    {
        //Cursor.SetCursor(cursorTex, hotspot, CursorMode.Auto);
        //Performance = 0.5f;
    }

    public float blinkLerp = 100f;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Cursor.SetCursor(cursorTex, hotspot, CursorMode.Auto);

        imgPerformanceBlink.color = Color.Lerp(imgPerformanceBlink.color, Color.white, Time.deltaTime * blinkLerp);
    }

    public Image evacProgress;

    public void UpdateEvac(float newVal)
    {
        if (null != evacProgress)
            evacProgress.fillAmount = newVal;
    }

    public PostProcessVolume ppv;

    void UpdatePP(float happiness)
    {
        if (null != ppv)
        {
            var pp = ppv.sharedProfile;
            pp.TryGetSettings<ColorGrading>(out var cg);
            cg.ldrLutContribution.value = 1f - happiness;   
        }
    }
}
