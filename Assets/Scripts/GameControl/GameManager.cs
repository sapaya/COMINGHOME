using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A central hub to target global actions in the game to, replaces MonoReactions
/// For Events specific to an Object component, select that 
/// component in the Event Editor
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Variables
    SceneControl sceneControl;
    float audioLevel = 1f;
    ForceMenuTarget[] menus;

    static GameManager _instance;
    #endregion

    #region Main Methods
    // Keep Persistent Scene
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning("There can only be one GameManager");
        }
    }

    private void Start()
    {
        sceneControl = PersistentScripts.persistent.GetComponentInChildren<SceneControl>();
        menus = GetComponentsInChildren<ForceMenuTarget>();
    }
    #endregion

    #region Helper Methods
    #region General Game Flow
    public void CallScene(string sceneName)
    {
        sceneControl.FadeAndLoadScene(sceneName);
    }

    public void LockInteractions()
    {
        foreach (ForceMenuTarget fm in menus)
        {
            fm.Lock();
        }
    }

    public void UnlockInteractions()
    {
        foreach (ForceMenuTarget fm in menus)
        {
            fm.Unlock();
        }
    }
    #endregion

    #region Conditions
    public void CheckCondition(Condition co)
    {
        co.satisfied = true;
    }

    public void UncheckCondition(Condition co)
    {
        co.satisfied = false;
    }
    #endregion

    #region Sounds
    public void AdjustAmbientLevel(float level)
    {
        audioLevel = level;
    }

    public void SwitchAmbientMusic(AudioClip soundFile)
    {
        sceneControl.audioManager.PlayAmbient(soundFile, 2, audioLevel);
    }

    public void PlaySound(AudioClip soundFile)
    {
        sceneControl.audioManager.PlaySound(soundFile);
    }
    #endregion

    #region Object Flow
    public void DisableAnimator(Animator target)
    {
        target.enabled = false;
    }

    public void EnableAnimator(Animator target)
    {
        target.enabled = true;
    }
    #endregion
    #endregion
}
