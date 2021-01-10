using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SceneControl : MonoBehaviour {

	public event Action BeforeSceneUnload;
	public event Action AfterSceneLoad;

	public CanvasGroup faderCanvasGroup;
    public Image fadeIcon;
    public bool showFadeIcon = false;
	public float fadeDuration = 1.0f;

    [HideInInspector]
    public SaveData mainSaveData = new SaveData(); // TODO: this needs editing
    [HideInInspector]
    public string savePath;

    public AudioManager audioManager;

	public const string loadedSceneKey = "LoadedScene";
	public const string startingPositionKey = "StartingPosition";

	private bool isFading;
    private CursorControl cc;

    private void Start()
    {
        isFading = false;
        cc = FindObjectOfType<CursorControl>();
    }
	
	public void FadeAndLoadScene (string sceneName){
        cc.Disable();
        PauseGame.active = false;
        if (!isFading){ //if a fade isn't happening, fade and switch scenes
			StartCoroutine(FadeAndSwitchScenes(sceneName));
		}
        else
            StartCoroutine(FadeFinish(sceneName));
    }

	private IEnumerator FadeAndSwitchScenes(string sceneName){
		//fade to black and wait for that to finish
		yield return StartCoroutine(Fade(1f));

		//if this event has subscribers, call it
		if(BeforeSceneUnload != null)
			BeforeSceneUnload();

        int oldScene = SceneManager.GetActiveScene().buildIndex;

		yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        //unload current scene and wait for that to finish
        yield return SceneManager.UnloadSceneAsync(oldScene);

        yield return StartCoroutine(Fade(0f)); //fade to new scene after scene is loaded
        cc.Enable();
        PauseGame.active = true;

        //if this event has any subscribers, call it.
        if (AfterSceneLoad != null)
			AfterSceneLoad();
	}


    private IEnumerator FadeFinish(string sceneName)
    {   
        //TODO: insert "loading" pic
        //wait until previous Fade is done
        yield return new WaitUntil(() => isFading == false);
        yield return StartCoroutine(FadeAndSwitchScenes(sceneName));
        if (showFadeIcon == false)
        {
            fadeIcon.canvasRenderer.SetAlpha(1);
        }
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName){
		//load over several frames
		yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

		Scene freshScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1); // start at index 0

		//mark fresh scene as active, making it the next in the queue to be unloaded
		SceneManager.SetActiveScene(freshScene);

	}

	private IEnumerator Fade (float finalAlpha){
        if(showFadeIcon == false)
        {
            fadeIcon.canvasRenderer.SetAlpha(0);
        }
		isFading = true;

		faderCanvasGroup.blocksRaycasts = true; // no clicking allowed while fading

		float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

		while(!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha)){
			faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

			yield return null; // wait for 1 frame
		}

		isFading = false;
        faderCanvasGroup.blocksRaycasts = false; // unlock again
	}

    public void SetFadeIcon(bool val)
    {
        showFadeIcon = val;
    }

    /*private void OnDestroy()
    {
        mainSaveData.Serialize(savePath);
    }*/
}
