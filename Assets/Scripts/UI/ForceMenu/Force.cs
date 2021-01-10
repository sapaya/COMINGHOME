using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]
public class Force : MonoBehaviour
{
    #region Variables
    public Interactable.ForceType forceType;

    bool forceEnabled = false;
    bool open = false;
    public bool interactionAllowed { get { return forceEnabled; } }
    #endregion

    #region Methods 
    public void Reposition(Vector3 newPos)
    {
        GetComponent<Image>().transform.position = newPos;
    }

    public void Close()
    {
        if (!open)
            return;
        open = false;
        SetTrigger("close");
    }

    public void Open()
    {
        if (open)
            return;
        open = true;
        SetTrigger("open");
    }

    public void Activate()
    {
        SetTrigger("activate");
        forceEnabled = true;
    }

    public void Deactivate()
    {
        SetTrigger("reset");
        forceEnabled = false;
    }

    public void Apply()
    {
        SetTrigger("click");
    }

    public void SetTrigger(string trigger)
    {
        Animator anim = GetComponent<Animator>();
        if (anim == null)
            return;

        anim.SetTrigger(trigger);
    }
    #endregion
}
