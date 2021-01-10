using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera Controls for the presentation Scene ForestTree_STATIC
/// </summary>
public class StaticCameraControl : MonoBehaviour {

	public float turnSpeed = 1.0f;

	void Start(){
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update () {
		float rotation = Input.GetAxis ("Mouse X") * turnSpeed;
		transform.Rotate (0, rotation, 0);

	}
}
