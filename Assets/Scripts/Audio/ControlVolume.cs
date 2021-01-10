using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlVolume : MonoBehaviour {

    private SceneControl sceneControl;    // Reference to the SceneController to actually do the loading and unloading of scenes.
    private AudioSource thisChannel;
    public bool music;
    private float originalVolume;

    void Start()
    {
        sceneControl = FindObjectOfType<SceneControl>();
        thisChannel = GetComponent<AudioSource>();

        originalVolume = thisChannel.volume;
	}
	
	// Update is called once per frame
	void Update () {
        if (music)
            thisChannel.volume = AudioManager.GetNewVolume(originalVolume, sceneControl.audioManager.mainVolumePercentage);
        else
            thisChannel.volume = AudioManager.GetNewVolume(originalVolume, sceneControl.audioManager.sfxVolumePercentage);
    }
}
