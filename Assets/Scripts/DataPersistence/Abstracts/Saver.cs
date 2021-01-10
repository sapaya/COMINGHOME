using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public abstract class Saver : MonoBehaviour {

    public string identifier;
    protected SaveData saveData;
    protected string filePath;

    protected string key; // what is being saved?

    protected SceneControl sceneControl;
    private void Awake()
    {
        sceneControl = FindObjectOfType<SceneControl>();

        if (!sceneControl)
            throw new UnityException("Scene Control missing!");

        key = SetKey();
        filePath = Application.persistentDataPath + "/geist_" + key + ".dat";

        loadFile();
    }
    

    private void OnEnable() {
        sceneControl.BeforeSceneUnload += Save;
        sceneControl.AfterSceneLoad += Load;
    }

    private void OnDisable() {
        sceneControl.BeforeSceneUnload -= Save;
        sceneControl.AfterSceneLoad -= Load;
    }

    // returns intended key during awake
    protected abstract string SetKey();

    // will be called just before scene is unloaded
    protected abstract void Save();

    // will be called after scene has finished loading
    protected abstract void Load();

    protected void writeFile() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        bf.Serialize(file, saveData);
        file.Close();
    }

    protected void loadFile() {
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            
            saveData.boolKeyValuePairLists = data.boolKeyValuePairLists;
            saveData.intKeyValuePairLists = data.intKeyValuePairLists;
            saveData.quaternionKeyValuePairLists = data.quaternionKeyValuePairLists;
            saveData.vector3KeyValuePairLists = data.vector3KeyValuePairLists;
            saveData.stringKeyValuePairLists = data.stringKeyValuePairLists;
        }
    }
}