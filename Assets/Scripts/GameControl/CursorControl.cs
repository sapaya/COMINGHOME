using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControl : MonoBehaviour {

    public Texture2D cursor;
    public float cursorSizeX = 16.0f; 
    public float cursorSizeY = 16.0f; 
 
    public float speed = 120.0f;
    private float angle = 0.0f;

    private bool active = true;
    public bool isActive { get { return active; } set { active = isActive; } }

	public static bool invX = false;
	public static bool invY = false;
    

    void Start() {
        Cursor.visible = false;
    }

    void Update() {

        Cursor.visible = false;

        angle += Time.unscaledDeltaTime * speed;
        angle = angle % 360.0f;
    }

    void OnGUI() {
        if (active) {
            Matrix4x4 matx = GUI.matrix;
            float x = Event.current.mousePosition.x - cursorSizeX / 2.0f;
            float y = Event.current.mousePosition.y - cursorSizeY / 2.0f;
            var pivot = new Vector2(x + cursorSizeX / 2.0f, y + cursorSizeY / 2.0f);
            GUIUtility.RotateAroundPivot(angle, pivot);
            GUI.DrawTexture(new Rect(x, y, cursorSizeX, cursorSizeY), cursor);
            GUI.matrix = matx;
        }
    }

    public void Enable() {
        active = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Disable() {
        active = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public bool IsActive() {
        return active;
    }

    public void SpeedUp() {
        speed = 180;
    }

    public void SlowDown() {
        speed = 90;
    }
}
