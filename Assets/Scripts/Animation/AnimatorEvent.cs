using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// creates a new Unity Event to run a batch of functions at once. To be called with 
/// Animation rerouter
/// </summary>
public class AnimatorEvent : MonoBehaviour
{
    [SerializeField]
    UnityEvent onFrameCalled = new UnityEvent();

    /// <summary>
    /// Invoked via animator
    /// </summary>
    public void RunEvent()
    {
        if(onFrameCalled != null)
        {
            onFrameCalled.Invoke();
        }
    }
}
