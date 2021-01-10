using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReactionSet : MonoBehaviour
{
    [SerializeField] UnityEvent onEventInvoked = new UnityEvent();     // The Event to run

    public void React()
    {
        if (onEventInvoked != null)
        {
            onEventInvoked.Invoke();
        }
    }

}
