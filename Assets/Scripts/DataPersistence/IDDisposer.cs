using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the ID-Savable mapping for all Savables.
/// </summary>
public class IDDisposer : MonoBehaviour {

    [System.Serializable]
    public class MappedID {
        public Savable savable;
        public int ID;
    }

    public MappedID[] disposedIDs;
    private Dictionary<int, Savable> IDsToSavables;
    private Dictionary<Savable, int> savablesToIDs;

    private void Awake() {

        this.IDsToSavables = new Dictionary<int, Savable>();
        this.savablesToIDs = new Dictionary<Savable, int>();

        foreach (MappedID pair in disposedIDs) {
            this.IDsToSavables.Add(pair.ID, pair.savable);
            this.savablesToIDs.Add(pair.savable, pair.ID);
        }
    }

    /// <summary>
    /// Returns the Savable corresponding to the given ID.
    /// </summary>
    /// <param name="ID">The ID of the desired Savable.</param>
    /// <returns>The Savable corresponding to the given ID.</returns>
    public Savable GetSavable(int ID) {
        return this.IDsToSavables[ID];
    }

    /// <summary>
    /// Returns the ID of the given Savable.
    /// </summary>
    /// <param name="savable">The Savable, whose ID is desired.</param>
    /// <returns>The ID of the given Savable.</returns>
    public int GetID(Savable savable) {
        return this.savablesToIDs[savable];
    }
    
}
