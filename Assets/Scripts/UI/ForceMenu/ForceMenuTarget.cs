using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HighlightingSystem;

public enum MenuLockState
{
    None = 0,
    Locked = 1
}

public class ForceMenuTarget : MonoBehaviour {
    #region Variables
    #region Component Settings
    [Header("Allowed Forces")]
    //the forces which can be applied to this game object
    public bool water = false;
    public bool freeze = false;
    public bool wind = false;
    public bool electricity = false;

    [Header("Menu Status Settings")]
    public bool turnedOn = true;            // indicates if the ForceMenu can be used
    [SerializeField] MenuLockState lockState = MenuLockState.None;

    [Header("Highlighting Settings")]
    public GameObject meshObj;
    public Color glowColor = Color.white;
    public bool blink;                          // indicates if the highlighter should blink

    public enum TriggerType
    {
        Persistent,
        Callback,
        Timer
    }

    public TriggerType trigger = TriggerType.Callback;
    public float timerLimit;
    public float blinkTime = 6f;
    #endregion

    #region Highlighting
    // Variables for blink timer
    private float timer;
    private float blinker;
    private bool runTimer = false;
    private bool runBlinker = false;

    protected Highlighter h;
    private Color blinkCol;
    #endregion

    #region Menu Controls
    private bool active;                    // indicates if the ForceMenu is active (so if it's visible atm)
    private bool interacted = false;
    private bool userHovers = false;
    
	private Interactable inta;

    private Canvas menuCanvas; // the menu object
    ForceMenu forceMenu;
    private Camera GUIcam;

    private CursorControl cc;
    private CameraControl cam;
    #endregion
    #endregion

    #region Main Methods
    void Awake()
    {
        h = meshObj.GetComponent<Highlighter>();
        if (h == null) { h = meshObj.AddComponent<Highlighter>(); }
    }

    // Use this for initialization
    void Start () {
        active = false;

        inta = gameObject.GetComponent<Interactable>();
        cc = FindObjectOfType<CursorControl>();
        cam = FindObjectOfType<CameraControl>();
        GUIcam = GameObject.Find("GUICamera").GetComponent<Camera>();

        if (trigger == TriggerType.Persistent)
            Glow();

        if (trigger == TriggerType.Timer)
        {
            timer = timerLimit;
            runTimer = true;
        }

        blinkCol = glowColor;
        blinkCol.a = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        if(turnedOn && lockState == MenuLockState.None)
            CheckHighlighter();
    }

    private void OnMouseEnter()
    {
        if (!turnedOn || !cam.active || lockState == MenuLockState.Locked)
            return;
        
        cc.SpeedUp();
        Glow();

        userHovers = true;
    }

    void OnMouseExit()
    {
        userHovers = false;
        if (!turnedOn || lockState == MenuLockState.Locked)
            return;
        
        cc.SlowDown();
        // do not unglow if the user is currently selecting
        if (!active)
        {
            UnGlow();
        }
    }

    private void OnMouseDown()
    {
        // user clicks on highlighted object
        if (turnedOn && !active && lockState == MenuLockState.None)
        {
            // initialize clone of ForceMenu
            menuCanvas = Instantiate(Resources.Load("GUI/Menu", typeof(Canvas))) as Canvas;
            menuCanvas.worldCamera = GUIcam;
            forceMenu = menuCanvas.gameObject.GetComponent<ForceMenu>();

            Vector3 screenPoint = GUIcam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f));
            forceMenu.InitMenu(this, screenPoint, wind, water, freeze, electricity);

            // disable CameraControl and Cursor visibility
            cam.TurnOff();
            cc.isActive = false;

            active = true;
        }
    }
    #endregion

    #region Helper Methods
    #region Force Menu Handling
    public void ApplyForce(Interactable.ForceType force)
    {
        inta.Interact(force);
        interacted = true;
    }

    public void Clear()
    {
        if(!userHovers || !turnedOn)
            UnGlow();

        cam.TurnOn();
        cc.isActive = true;

        active = false;
        menuCanvas = null;
    }
    #endregion

    #region State Handling
    public void TurnOn() {
        turnedOn = true;
        if (userHovers) {
            Glow();
        }
    }

    public void TurnOff() {
        turnedOn = false;
        UnGlow();
        Destroy(forceMenu);
    }

    public bool HasInteracted() {
        return interacted;
    }

    public void Lock()
    {
        if (userHovers && turnedOn)
        {
            UnGlow();
        }
        Destroy(forceMenu);
        lockState = MenuLockState.Locked;
    }

    public void Unlock()
    {
        if (userHovers && turnedOn)
        {
            Glow();
        }
        lockState = MenuLockState.None;
    }
    #endregion

    #region Highlight Methods
    private void CheckHighlighter()
    {
        if (runTimer)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                runTimer = false;
                if (blink) StartBlinking();
            }
        }

        if (runBlinker)
        {
            blinker -= Time.deltaTime;
            if (blinker < 0)
            {
                StopBlinking();
                timer = timerLimit;
                runTimer = true;
            }
        }
    }

    private void StartBlinking()
    {
        blinker = blinkTime;
        runBlinker = true;
        h.FlashingOn(glowColor, blinkCol);
    }

    private void StopBlinking()
    {
        h.FlashingOff();
        runBlinker = false;
    }

    public void Glow()
    {
        runTimer = false;

        if (runBlinker)
        {
            StopBlinking();
        }

        h.ConstantOn(glowColor);
    }

    public void UnGlow()
    {
        if (h != null)
        {
            h.ConstantOff();
        }
    }
    #endregion
    #endregion
}