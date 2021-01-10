using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System;
using System.IO;

public class AudioManager : MonoBehaviour
{
    [HideInInspector]
    public AudioSource ambient1Channel;
    [HideInInspector]
    public AudioSource ambient2Channel;
    [HideInInspector]
    public AudioSource sfxChannel;
    [HideInInspector]
    public AudioSource sfxChannel2;

    private SceneControl sceneControl;    // Reference to the SceneController to actually do the loading and unloading of scenes.
    private int mainVolume;
    private int sfxVolume;

    private AudioClip forceSound;          // General sound that is played on force activation

    public float mainVolumePercentage;
    public float sfxVolumePercentage;

    void Start()
    {
        sceneControl = FindObjectOfType<SceneControl>();
        ambient1Channel = gameObject.AddComponent<AudioSource>();
        ambient1Channel.loop = true;
        ambient2Channel = gameObject.AddComponent<AudioSource>();
        ambient2Channel.loop = true;
        sfxChannel = gameObject.AddComponent<AudioSource>();
        sfxChannel2 = gameObject.AddComponent<AudioSource>();

        if (!sceneControl.mainSaveData.Load("musicVolume", ref mainVolume))
            mainVolume = 100;
        if(!sceneControl.mainSaveData.Load("sfxVolume", ref sfxVolume))
            sfxVolume = 100;

        mainVolumePercentage = (float)mainVolume / 100f;
        sfxVolumePercentage = (float)sfxVolume / 100f;
    }

    public void PlaySound(AudioClip clip)
    {
        sfxChannel.clip = clip;
        sfxChannel.Play();
    }

    public void PlaySound(AudioClip clip, bool isOverlay)
    {
        if (isOverlay)
        {
            sfxChannel2.clip = clip;
            sfxChannel2.Play();
        }
        else
            PlaySound(clip);
    }

    public void StopSound()
    {
        sfxChannel.Stop();
    }

    public void StopAmbient(int channel)
    {
        if(channel == 1)
        {
            ambient1Channel.Stop();
        }else if(channel == 2)
        {
            ambient2Channel.Stop();
        }
    }

    public void PauseSound()
    {
        if (sfxChannel.isPlaying)
        {
            sfxChannel.Pause();
        }
        else
        {
            sfxChannel.Play();
        }
    }

    public void PlayAmbient(AudioClip clip, int channel, float volumeAudjustment)
    {
        AudioSource targetChannel = (channel == 1 ? ambient1Channel : ambient2Channel);
        targetChannel.clip = clip;
        targetChannel.volume = GetNewVolume(volumeAudjustment, (channel == 1 ? mainVolumePercentage : sfxVolumePercentage));
        targetChannel.Play();
    }
    
    public static float GetNewVolume(float oldVol, float percentage)
    {
        if (percentage == 0)
            return 0; // don't divide by 0

        float ppercent = 1f / percentage;
        return oldVol / ppercent;
    }
}
