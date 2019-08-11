using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
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
        btnNewGame.SetActive(true);
        btnResume.SetActive(false);
        menuScreen.SetActive(true);
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

    public TextMeshProUGUI txtTimer;
        
    public void UpdateTimeIndicator(string timeStr)
    {
        txtTimer.text = timeStr;
    }

    private TimeManager ttm => SingletonUtils<TimeManager>.Instance;
    
    public GameObject introScreen;
    public GameObject menuScreen;
    public GameObject gameoverScreen;
    public void BtnMenu()
    {
        ttm.SetPaused(true);
        menuScreen.SetActive(true);
        
        btnNewGame.SetActive(false);
        btnResume.SetActive(true);
    }

    public GameObject btnNewGame;
    public GameObject btnResume;

    public void BtnNewGame()
    {
        SingletonUtils<SessionManager>.Instance.StartNewGame();
        menuScreen.SetActive(false);
    }
    
    public void BtnResume()
    {
        ttm.SetPaused(false);
        menuScreen.SetActive(false);
    }

    public TextMeshProUGUI audBtnText;
    public void BtnSound()
    {
        var aud = SingletonUtils<SoundManager>.Instance;
        aud.SetAudio(!aud.IsAudioOn);
        audBtnText.text = $"AUDIO {(aud.IsAudioOn ? "ON" : "OFF")}";
    }

    public void BtnQuit()
    {
        if (!Application.isEditor)
            Application.Quit();
    }

    public void ShowGameoverScreen()
    {
        gameoverScreen.SetActive(true);
        
        //stats
    }

    public void BtnRestart()
    {
        DOTween.KillAll(false);
        ttm.SetPaused(false);
        SceneManager.LoadScene(0);
    }
}
