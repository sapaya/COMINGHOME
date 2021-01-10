using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to call more complex Event in an animation. 
/// Requires an animator because it is only meant to be used in conjunction with
/// the animator to circumvent Unity's limited Animator Events
/// </summary>
[RequireComponent(typeof(Animator))]
public class RerouteAnimationEvent : MonoBehaviour
{
    Component[] animationEvents = new Component[0];

    private void Start()
    {
        animationEvents = GetComponentsInChildren<AnimatorEvent>();
    }

    /// <summary>
    /// Main function
    /// </summary>
    /// <param name="callNumber">Index of Game Object with an Animator Event call on it</param>
    public void MakeEventCall(int callNumber)
    {
        try {
            AnimatorEvent call = (AnimatorEvent)animationEvents[callNumber];
            call.RunEvent();
        } catch (System.IndexOutOfRangeException e) {
            Debug.LogError(e.ToString());
            Debug.Log(gameObject.name);
        }
    }
}
