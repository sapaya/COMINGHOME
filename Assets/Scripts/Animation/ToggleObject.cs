using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used in post scene to toggle different parts of the story
/// </summary>
public class ToggleObject : MonoBehaviour {

	public void HideObject(string objName){
		GameObject obj = GameObject.Find (objName);
		obj.SetActive (false);
	}

    public void ShowObject(string objName) {
        GameObject obj = GameObject.Find(objName);
        obj.SetActive(true);
    }

    public void UnhideObject(string objName){
		string[] objPath = objName.Split ('|');
		GameObject parent = GameObject.Find (objPath [0]);
		Transform objTransform = parent.transform.Find (objPath [1]);
		objTransform.gameObject.SetActive (true);
	}

}
