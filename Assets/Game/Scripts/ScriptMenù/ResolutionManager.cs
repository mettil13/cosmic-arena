using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

//Codice che gestisce il setting delle impostazioni del gioco
public class ResolutionManager : MonoBehaviour
{
    //Oggetti Risoluzione
    [Header("Resolution")]
    [SerializeField] private TextMeshProUGUI resolutionText;
    [SerializeField] private TextMeshProUGUI screenModeText;
    //Oggetti FPS
    [Header("FPS")]
    [SerializeField] private Slider fpsSlider;
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private Toggle vSyncToggle;

    private Resolution[] resolutions;
    private FullScreenMode fullscreenMode;
    private List<FullScreenMode> screenModes;
    private List<Resolution> filteredResolutions;
    private List<string> screenOptions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;
    private int currentScreenModeIndex = 0;

    private float MAX_SCREEN_FPS;
    private float MIN_SCREEN_FPS = 30;
    void Start()
    {
        //Lo Start si divide in:
        // - Risoluzione, che rende disponibili negli oggetti di UI le opzioni con le quali può cambiare risoluzione del gioco
        // - ScreenMode, che permette di decidere se visualizzare il gioco a Schermo Intero, Finestra o Finestra senza bordi
        // - FPS, che ti permette di scegliere il numero di FPS desiderato o attivare il V-Sync
        // - Volume, Da fare
        // - Language, Da fare
        #region Resolution
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if ((float)resolutions[i].refreshRateRatio.value == currentRefreshRate)
                filteredResolutions.Add(resolutions[i]);
        }

        for(int i = 0; i < filteredResolutions.Count; i++) 
        { 
            if (filteredResolutions[i].width == Screen.width && 
                filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
                resolutionText.SetText(filteredResolutions[i].width +"x"+ filteredResolutions[i].height);
            }
        }
        #endregion

        #region ScreenMode
        screenOptions = new List<string>();
        screenModes = new List<FullScreenMode>();

        string windowOption = "Windowed";
        string fullScreen = "Full Screen";
        string noBorderWindow = "No border window";

        
        screenOptions.Add(fullScreen);
        screenOptions.Add(windowOption);
        screenOptions.Add(noBorderWindow);

        screenModes.Add(FullScreenMode.FullScreenWindow);
        screenModes.Add(FullScreenMode.Windowed);
        screenModes.Add(FullScreenMode.ExclusiveFullScreen);

        for (int i = 0; i<screenModes.Count; i++) {
            if (screenModes[i] == Screen.fullScreenMode)
            {
                currentScreenModeIndex = i;
                fullscreenMode = screenModes[i];
                screenModeText.SetText(screenOptions[i]);
            }
        }
        #endregion

        #region Fps
        MAX_SCREEN_FPS = (float)Math.Truncate(Screen.currentResolution.refreshRateRatio.value);
        fpsSlider.maxValue = MAX_SCREEN_FPS;
        fpsSlider.minValue = MIN_SCREEN_FPS;
        fpsSlider.value = currentRefreshRate;

        #endregion
    }

    //Metodi che cambiano la risoluzione
    #region ResolutionMethods
    private void SetResolution()
    {
        Resolution resolution = filteredResolutions[currentResolutionIndex];
        resolutionText.SetText(resolution.width + "x" + resolution.height);
        Screen.SetResolution(resolution.width, resolution.height, fullscreenMode);
    }

    public void ChangeResolutionLeft()
    {
        currentResolutionIndex--;
        if(currentResolutionIndex < 0)
            currentResolutionIndex = filteredResolutions.Count - 1;
        SetResolution();
    }

    public void ChangeResolutionRight()
    {
        currentResolutionIndex++;
        if(currentResolutionIndex == filteredResolutions.Count)
            currentResolutionIndex = 0;
        SetResolution();
    }
    #endregion

    //Metodi che cambiano il metodo di visualizzazione della schermata di gioco
    #region ScreenMode
    private void ChangeFullScreenMode()
    {
        Resolution resolution = filteredResolutions[currentResolutionIndex];
        fullscreenMode = screenModes[currentScreenModeIndex];
        screenModeText.SetText(screenOptions[currentScreenModeIndex]);
        Screen.SetResolution(resolution.width, resolution.height, fullscreenMode);
    }

    public void ChangeScreenModeLeft()
    {
        currentScreenModeIndex--;
        if( currentScreenModeIndex < 0)
            currentScreenModeIndex = screenModes.Count - 1;
        fullscreenMode = screenModes[currentScreenModeIndex];
        ChangeFullScreenMode();
    }
    public void ChangeScreenModeRight()
    {
        currentScreenModeIndex++;
        if (currentScreenModeIndex == screenModes.Count)
            currentScreenModeIndex = 0;
        fullscreenMode = screenModes[currentScreenModeIndex];
        ChangeFullScreenMode();
        
    }
    #endregion

    //Metodi FPS
    #region FPS
    //Metodo che cambia il numero degli fps visualizzabili per secondo

    public void ChangeFps()
    {
        if (!vSyncToggle.isOn)
        {
            RefreshRate refreshRate = new RefreshRate();
            refreshRate.numerator = (uint)fpsSlider.value;
            refreshRate.denominator = 1;
            Resolution resolution = filteredResolutions[currentResolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, fullscreenMode, refreshRate);
            fpsText.SetText(Math.Truncate(fpsSlider.value) + "");
        }
    }

    //Metodo che attiva il V-Sync
    public void SetVSync()
    {
        RefreshRate refreshRate = new RefreshRate();
        if (vSyncToggle.isOn)
        {
            refreshRate.numerator = (uint)MAX_SCREEN_FPS;
            refreshRate.denominator = 1;
            Resolution resolution = filteredResolutions[currentResolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, fullscreenMode, refreshRate);

        }
        else
        {
            refreshRate.numerator = (uint)fpsSlider.value;
            refreshRate.denominator = 1;
            Resolution resolution = filteredResolutions[currentResolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, fullscreenMode, refreshRate);
        }
    }
    #endregion
}
