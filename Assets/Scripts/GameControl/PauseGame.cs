using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using HighlightingSystem;
using EvilScience.UI;

/// <summary>
/// Attached to the main camera to pause the game
/// </summary>
[RequireComponent(typeof(CameraControl))]
public class PauseGame : MonoBehaviour {
    #region Variables
    public static bool active = true;
    private bool paused = false;

    [SerializeField] GameObject m_pauseObject;
    [SerializeField] UC_UISystem m_uiGroup;
    [SerializeField] UC_UIScreen m_pauseMenu;
    [SerializeField] UC_UIScreen m_defaultScreen;
    [SerializeField] PostProcessingProfile defaultProcessingProfile;
    [SerializeField] PostProcessingProfile pauseProcessingProfile;

    private Component[] forceMenus;
   
    private CursorControl cursorControl;
    private CameraControl cameraControl;
    private HighlightingRenderer hlr;

    private Vector3 gravityDir;
    #endregion

    #region Main Methods
    private void Awake()
    {
        forceMenus = FindObjectsOfType<ForceMenuTarget>();
        cursorControl = FindObjectOfType<CursorControl>();
        cameraControl = GetComponent<CameraControl>();
        hlr = GetComponent<HighlightingRenderer>();
    }


    void Update () {
        if (Input.GetKeyUp(KeyCode.Escape) && active) {
            Pause();
        }
    }
    #endregion

    #region Helper Methods
    public void Pause() {
        paused = !paused;

        if (paused)
        {
            FreezeGame();
            m_uiGroup.SwitchScreen(m_pauseMenu);
        }
        else
        {
            UnFreezeGame();
            m_uiGroup.SwitchScreen(m_defaultScreen);
        }

        ToggleMenu();
    }

    public void ToggleMenu()
    {

        foreach (ForceMenuTarget forceMenu in forceMenus)
        {
            if (paused) {
                forceMenu.TurnOff();
            } else {
                forceMenu.TurnOn();
            }
        }

        m_pauseObject.SetActive(paused);
    }

    /// <summary>
    /// Switches the Processing Profile to the normal and resumes all controls
    /// If a lock has been instated by an animator, the cursor will stay locked
    /// </summary>
    private void UnFreezeGame()
    {
        Time.timeScale = 1;

        GetComponent<PostProcessingBehaviour>().profile = defaultProcessingProfile;
        Physics.gravity = gravityDir;

        if(cameraControl.lockState == CameraLockState.None)
        {
            cameraControl.TurnOn();
            hlr.enabled = true;
        }
        else
        {
            cursorControl.Disable(); //if we're locked, disable cursor until lock is released
        }
    }

    /// <summary>
    /// Freezes time and gravity. 
    /// To animate elements during pause, use Time.unscaledDeltaTime or "Always animate" on the animator
    /// </summary>
    private void FreezeGame()
    {
        Time.timeScale = 0;

        GetComponent<PostProcessingBehaviour>().profile = pauseProcessingProfile;
        gravityDir = Physics.gravity;
        Physics.gravity = -Camera.main.transform.up;

        if (cameraControl.lockState == CameraLockState.None)
        {
            cameraControl.TurnOff();
            hlr.enabled = false;
        }
        else
        {
            cursorControl.Enable();
        }
    }
    #endregion
}
