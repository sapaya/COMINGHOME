using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ForceMenu : MonoBehaviour
{
    #region Variables
    [SerializeField] AudioClip forceSFX;

    bool active = false;
    public static ForceMenu instance = null;
    public static bool firstInteraction = false;

    // force information
    ForceMenuTarget currentTarget = null;
    Interactable.ForceType chosenForce = Interactable.ForceType.NONE;
    Component[] forces = new Component[0];

    // selection details
    private const int minMouseMovement = 5; //min mouse movement distance to chose a force
    private Vector3 mousePosition;
    private bool highlighted = false; // indicates if the currently chosen force is highlighted in the menu

    private AudioManager audioManager = null;
    #endregion

    #region Main Methods

    // Update is called once per frame
    void Update()
    {
        if (active && currentTarget.turnedOn)
        {
            FirstInteractionCheck();
            CheckInteraction();
        }
    }

    private void OnDestroy()
    {
        currentTarget.Clear();
        currentTarget = null;
        active = false;
        chosenForce = Interactable.ForceType.NONE;

        foreach(Force f in forces)
        {
            f.Deactivate();
        }

        Destroy(gameObject);
    }
    #endregion

    #region Helper Methods
    private static void FirstInteractionCheck()
    {
        if (!firstInteraction)
        {
            firstInteraction = true;
            FindObjectOfType<TutorialManager>().HasInteracted();
        }
    }

    private void InitForces(Vector3 screenPoint, bool wind, bool water, bool freeze, bool electricity)
    {
        forces = GetComponentsInChildren<Force>();
        foreach(Force f in forces)
        {
            f.Reposition(screenPoint);
            if (f.forceType == Interactable.ForceType.WIND && wind)
                f.Activate();
            else if (f.forceType == Interactable.ForceType.WATER && water)
                f.Activate();
            else if (f.forceType == Interactable.ForceType.ICE && freeze)
                f.Activate();
            else if (f.forceType == Interactable.ForceType.ELECTRICITY && electricity)
                f.Activate();
        }
    }
    public void InitMenu(ForceMenuTarget target, Vector3 screenPoint, bool wind, bool water, bool freeze, bool electricity)
    {
        // make sure we only have one instance of the force menu open at a time
        if (instance != null && instance != this)
        {
            instance.Close();
        }

        instance = this;

        currentTarget = target;
        InitForces(screenPoint, wind, water, freeze, electricity);
        Open();

        active = true;
    }

    private void CheckInteraction()
    {
        // activate chosen force
        if (Input.GetMouseButtonDown(0) && chosenForce != Interactable.ForceType.NONE)
        {
            ApplyForce();
        }

        // cancel interaction
        if (Input.GetMouseButton(1))
        {
            currentTarget.Clear();
            Close();
        }

        UpdateForce();
        mousePosition = Input.mousePosition;
    }

    private void UpdateForce()
    {
        if (!active || !currentTarget.turnedOn)
            return;

        Vector3 mouseDelta = Input.mousePosition - mousePosition;

        float deltaX = Mathf.Abs(mouseDelta.x);
        float deltaY = Mathf.Abs(mouseDelta.y);

        if (deltaX > deltaY && deltaX > minMouseMovement)
        {
            //mouse moved in x-Axis
            if (mouseDelta.x > 0)
            {
                //right
                AttemptForceChange(Interactable.ForceType.WATER);
            }
            else
            {
                AttemptForceChange(Interactable.ForceType.WIND);
            }
        }
        else if (deltaX < deltaY && deltaY > minMouseMovement)
        {
            //moved in y axis
            if (mouseDelta.y > 0)
            {
                //up
                AttemptForceChange(Interactable.ForceType.ELECTRICITY);
            }
            else
            {
                //down
                AttemptForceChange(Interactable.ForceType.ICE);
            }
        }
    }

    void AttemptForceChange(Interactable.ForceType newForce)
    {
        if (chosenForce == newForce)
            return;

        highlighted = false;
        foreach (Force f in forces)
        {
            if (!f.interactionAllowed)
                continue;
            else if (f.forceType == newForce)
            {
                chosenForce = newForce;
                f.Open();
                highlighted = true;
            }
            else
                f.Close();
        }
        if (!highlighted)
            chosenForce = Interactable.ForceType.NONE;
    }

    private void ApplyForce()
    {
        foreach (Force f in forces)
        {
            if (!f.interactionAllowed)
                continue;
            else if (f.forceType == chosenForce) {

                if (audioManager == null) {
                    audioManager = FindObjectOfType<AudioManager>();
                }

                f.Apply();
                audioManager.PlaySound(forceSFX, true);

            }
        }

        Close();
        currentTarget.ApplyForce(chosenForce);
        currentTarget.Clear();
    }

    #region Animation Stuff
    private void Open()
    {
        if(!active)
            SetTrigger("open");
    }

    private void Close()
    {
        if(active)
            SetTrigger("close");
    }

    private void SetTrigger(string t)
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger(t);
    }

    public void Destroy()
    {
        Destroy(this);
    }
    #endregion
    #endregion
}
