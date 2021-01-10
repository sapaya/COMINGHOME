using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSaver : Saver {
    
    protected override string SetKey (){
        saveData = sceneControl.mainSaveData;
        return identifier;
	}

    protected override void Save()
    {
        /*
        saveData.Save(key, transformToSave.position);
        Debug.Log(key + " saved with " + transformToSave.position.ToString());
        */
        
        writeFile();
    }

    protected override void Load()
    {
        /*
        Vector3 position = Vector3.zero;

        if (saveData.Load(key, ref position))
            transformToSave.position = position; // if we've already got a position saved for this object, use it. 
        */
        loadFile();
    }

    private void OnDestroy()
    {
        writeFile(); // in case we destroy without saving
    }
}
