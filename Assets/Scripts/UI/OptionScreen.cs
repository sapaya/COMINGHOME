using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionScreen : MonoBehaviour
{
    [HideInInspector]
    public SceneControl sceneControl;
    private SaveData saveData;

    [SerializeField] Toggle m_InvertXToggle;
    [SerializeField] Toggle m_InvertYToggle;
    [SerializeField] Slider m_MusicVolume;
    [SerializeField] Slider m_SFXVolume;

    private bool invertX = false, invertY = false;
    private int musicVolume = 50, sfxVolume = 50;
    private CursorControl cc;

    private void Awake()
    {
        sceneControl = FindObjectOfType<SceneControl>();

        if (!sceneControl)
            throw new UnityException("Scene Control missing!");

        saveData = sceneControl.mainSaveData;

        cc = FindObjectOfType<CursorControl>();
    }

    // Use this for initialization
    void Start()
    {
        saveData.Load("invertX", ref invertX);
        saveData.Load("invertY", ref invertY);
        CursorControl.invX = invertX;
        CursorControl.invY = invertY;
        saveData.Load("musicVolume", ref musicVolume);
        saveData.Load("sfxVolume", ref sfxVolume);

        m_InvertXToggle.isOn = invertX;
        m_InvertYToggle.isOn = invertY;
        m_MusicVolume.value = musicVolume;
        m_SFXVolume.value = sfxVolume;
    }


    public void OnMusicVolumeChange()
    {
        musicVolume = (int)m_MusicVolume.value;
        saveData.Save("musicVolume", musicVolume);

        sceneControl.audioManager.mainVolumePercentage = (float)musicVolume / 100f;

        sceneControl.audioManager.ambient1Channel.volume = (float)musicVolume / 100f;

    }

    public void OnSFXVolumeChange()
    {
        sfxVolume = (int)m_SFXVolume.value;
        saveData.Save("sfxVolume", sfxVolume);

        sceneControl.audioManager.sfxVolumePercentage = (float)sfxVolume / 100f;

        sceneControl.audioManager.ambient2Channel.volume = (float)sfxVolume / 100f; //ambient 2 is for sfx
        sceneControl.audioManager.sfxChannel.volume = (float)sfxVolume / 100f;
    }

    public void OnInvertXAxis(bool newValue)
    {
        saveData.Save("invertX", newValue);
        CursorControl.invX = newValue;
    }

    public void OnInvertYAxis(bool newValue)
    {
        saveData.Save("invertY", newValue);
        CursorControl.invY = newValue;
    }
}
