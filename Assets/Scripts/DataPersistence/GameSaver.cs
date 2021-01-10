using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Main save/load class. Collects the information of all savables, serializes it and writes it to a file.
/// </summary>
public class GameSaver: MonoBehaviour {

    public bool loadGame = false;

    public int autoSaveTimer = 300;

    public string path = "/savegame";
    public string saveGameFileName = "/savegame.data";
    private string saveGameFilePath;

    public bool printDataToTextFile = false;

    private IDDisposer disposer;
    

    private void Awake() {
        this.saveGameFilePath = Application.dataPath + this.path + this.saveGameFileName;

        if (!Directory.Exists(Application.dataPath + this.path)) {
            Directory.CreateDirectory(Application.dataPath + this.path);
        }
    }

    private void Start() {
        this.disposer = FindObjectOfType<IDDisposer>();

        if (this.loadGame) {
            this.LoadSavegame();
        }

        if (autoSaveTimer > 0) {
            InvokeRepeating("Save", autoSaveTimer, autoSaveTimer);
        }

    }

    /// <summary>
    /// Creates a snapshot of the current gamestate and saves it in a file.
    /// </summary>
    public void WriteSnapshot() {
        Invoke("Save", 0.0f);
    }

    /// <summary>
    /// The actual saving function. Collects data saves it. Needs to be an IEnumerator in order to automatically execute it periodically.
    /// </summary>
    private void Save() {

        // collect save data
        Savable[] savables = FindObjectsOfType<Savable>();
        SaveGame saveGame = new SaveGame();
        foreach (Savable s in savables) {
            JSONObject saveData = s.GetSavingInformation();
            saveData.AddField("ID", this.disposer.GetID(s));
            saveGame.AddData(saveData);
        }

        // delete existing savegame
        this.DeleteSavegame();

        // open binary stream and write savegame to file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(this.saveGameFilePath, FileMode.OpenOrCreate);
        bf.Serialize(file, saveGame);
        file.Close();

        // used to investigate saved data (debug purposes)
        if (this.printDataToTextFile) {
            using (StreamWriter f = new StreamWriter(Application.dataPath + "/debug.txt", false)) {
                foreach (JSONObject jObj in saveGame.GetGameData()) {
                    f.Write(jObj.Print(pretty: true));
                }
            }
        }

        Debug.Log("Saved Gamestate.");
    }
    
    /// <summary>
    /// Loads the savegame and forwards the loaded data to the corresponding savables.
    /// </summary>
    public void LoadSavegame() {
        
        if (this.SavegameExists()) {
            SaveGame saveGame = ReadSavegame();
            
            foreach (JSONObject saveData in saveGame.GetGameData()) {
                int ID = int.Parse(saveData.GetField("ID").ToString());
                saveData.RemoveField("ID");
                this.disposer.GetSavable(ID).RestoreSavingInformation(saveData);
            }
        }        
    }

    /// <summary>
    /// Deserializes the savegame and retunrs it as a SaveGame object.
    /// </summary>
    /// <returns>SaveGame object, holding the deserialized information.</returns>
    private SaveGame ReadSavegame() {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(this.saveGameFilePath, FileMode.Open);
        SaveGame saveGame = (SaveGame)bf.Deserialize(file);
        file.Close();

        return saveGame;
    }

    /// <summary>
    /// Checks if there is a savegame file at the configured location.
    /// </summary>
    /// <returns>True if there is a savegame file, false otherwise.</returns>
    public bool SavegameExists() {
        return File.Exists(this.saveGameFilePath);
    }

    /// <summary>
    /// Deletes an existing savegame at the configured location.
    /// </summary>
    public void DeleteSavegame() {
        if (SavegameExists()) {
            File.Delete(this.saveGameFilePath);
        }
    }

    /// <summary>
    /// Container class to serialize and deserialize the save information.
    /// </summary>
    [System.Serializable]
    public class SaveGame {

        private List<JSONObject> gameData;

        public SaveGame() {
            gameData = new List<JSONObject>();
        }

        public void AddData(JSONObject saveData) {
            gameData.Add(saveData);
        }

        public List<JSONObject> GetGameData() {
            return this.gameData;
        }
    }
}
