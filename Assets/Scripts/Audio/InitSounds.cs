using UnityEngine;
using System.Collections;

public class InitSounds : MonoBehaviour {
	public AudioClip ambient1;
	public float ambient1Volume = 1;
	public AudioClip ambient2;
	public float ambient2Volume = 1;

    SceneControl sceneControl;
    

    // Use this for initialization
    void Start () {
        sceneControl = PersistentScripts.persistent.GetComponentInChildren<SceneControl>();
        Debug.Log("Initializing Sounds");

        sceneControl.audioManager.PlayAmbient(ambient1, 1, ambient1Volume);
        sceneControl.audioManager.PlayAmbient(ambient2, 2, ambient2Volume);
	}

}
