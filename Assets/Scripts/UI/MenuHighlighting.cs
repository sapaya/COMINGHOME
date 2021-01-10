using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class MenuHighlighting : MonoBehaviour {

    private CursorControl cc;

	// Use this for initialization
	void Start () {
        cc = FindObjectOfType<CursorControl>();

	}

    public void OnMouseEnter() {
        cc.SpeedUp();
    }

    public void OnMouseExit() {
        cc.SlowDown();
    }
}
