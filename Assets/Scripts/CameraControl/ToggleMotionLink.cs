using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;

/// <summary>
/// Catch-all system to turn off all camera controls and highlighting
/// </summary>
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(CameraControl))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HighlighterRenderer))]
public class ToggleMotionLink : MonoBehaviour {
    #region Variables
    private CameraControl motionLink;
    private CursorControl cursorControl;
    private Animator cameraAnimator;
    private HighlightingRenderer highlightLink;
    private bool isEnabled = true;
    #endregion

    #region Main Methods
    private void Start()
    {
        motionLink = GetComponent<CameraControl>();
        highlightLink = GetComponent<HighlightingRenderer>();
        cameraAnimator = GetComponent<Animator>();
        cursorControl = FindObjectOfType<CursorControl>();
    }
    #endregion

    #region Helper Methods
    public void ToggleLink() {
        if (!isEnabled) {
            isEnabled = true;

            motionLink.active = true;
            motionLink.lockState = CameraLockState.None;
            highlightLink.enabled = true;
            cameraAnimator.enabled = false; //this needs to be off or the motion controls will break
            cursorControl.Enable();
        }
        else
        {
            isEnabled = false;

            motionLink.StopCamera();
            motionLink.active = false;
            motionLink.lockState = CameraLockState.Locked;
            highlightLink.enabled = false;
            cursorControl.Disable();
        }
    }

    public void ToggleCursor()
    {
        if (!isEnabled)
        {
            isEnabled = true;
            cursorControl.Enable();
        }
        else
        {
            isEnabled = false;
            cursorControl.Disable();
        }
    }
    #endregion
}
