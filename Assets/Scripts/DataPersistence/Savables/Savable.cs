using UnityEngine;

public class Savable : MonoBehaviour {

    public int order;       // not used yet

    public Condition[] conditions;
    public Animator[] animatedObjects;        
    public GameObject[] reparentedObjects;
    public ForceMenuTarget[] forceMenus;

    public Camera main;

    /// <summary>
    /// Collects and returns the data, which is necessary to completely save and load the corresponding object.
    /// Uses GetSpecializedSavingInformation to collect data which differs collectable, animation and parenting information.
    /// </summary>
    /// <returns>JSONObject containing all data that has to be stored.</returns>
    public JSONObject GetSavingInformation() {

        JSONObject savingInformation = new JSONObject();

        // add order indicator
        savingInformation.AddField("order", this.order);

        JSONObject condInformation = new JSONObject();

        // iterate the conditions and save the satsified ones 
        foreach (Condition c in this.conditions) {
            if (c.satisfied) {
                condInformation.AddField(c.name, c.satisfied);
            }
        }

        savingInformation.AddField("conditions", condInformation);

        JSONObject animInfo = new JSONObject();

        // iterate the animatedObjects and save their animationStateInfo
        foreach (Animator anim in this.animatedObjects) {

            JSONObject animatedObjectInformation = new JSONObject();

            // iterate all animator states and save state name and timestamp for each
            for (int i = 0; i < anim.layerCount; i++) {

                JSONObject layer = new JSONObject();

                AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(i);
                AnimatorStateInfo sInfo = anim.GetCurrentAnimatorStateInfo(i);
                
                string stateName = "";
                foreach (AnimatorClipInfo cInfo in clips) {
                    if (sInfo.IsName(cInfo.clip.name)) {
                        stateName = cInfo.clip.name;
                        break;
                    }
                }

                layer.AddField("state", stateName);
                layer.AddField("ts", sInfo.normalizedTime);

                animatedObjectInformation.AddField(i.ToString(), layer);
            }

            string saveName = this.GenerateSaveString(anim.gameObject, this.gameObject);
            animInfo.AddField(saveName, animatedObjectInformation);
        }

        savingInformation.AddField("animators", animInfo);

        JSONObject parenting = new JSONObject();

        // iterate the reparentedObjects and save their parent-names in combination with their hashes
        foreach (GameObject gObj in this.reparentedObjects) {
            string objString = this.GenerateSaveString(gObj, this.gameObject);
            string parentString = this.GenerateSaveString(transform.parent.gameObject, this.gameObject);
            parenting.AddField(objString, parentString);
        }

        savingInformation.AddField("parenting", parenting);

        if (this.main != null)
        {
            JSONObject pos = new JSONObject();
            pos.AddField("x", this.main.transform.position.x);
            pos.AddField("y", this.main.transform.position.y);
            pos.AddField("z", this.main.transform.position.z);
            savingInformation.AddField("camera", pos);
        }

        JSONObject forceMenuInformation = new JSONObject();

        // iterate the ForceMenus and save their state
        foreach (ForceMenuTarget fm in this.forceMenus) {
            JSONObject menuData = new JSONObject();

            menuData.AddField("enabled", fm.turnedOn);

            string saveName = this.GenerateSaveString(fm.gameObject, this.gameObject);
            forceMenuInformation.AddField(saveName, menuData);
        }

        savingInformation.AddField("fmData", forceMenuInformation);

        // specialized information
        savingInformation.AddField("specific", this.GetSpecializedSavingInformation());
        
        return savingInformation;
    }

    /// <summary>
    /// Collects and returns data, which differs the general saving information (conditions, animators, parenting). Should be overwritten by inheriting classes.
    /// </summary>
    /// <returns>JSONObject containing specialized saving information, if such information exists and an empty object otherwise.</returns>
    protected virtual JSONObject GetSpecializedSavingInformation() {
        // Nothing to do here as the parent class does not correspond to something specifically
        return new JSONObject();
    }

    /// <summary>
    /// Applies the given information to the corresponding object.
    /// </summary>
    /// <param name="savingInformation">The data that is used for the restoring process.</param>
    public void RestoreSavingInformation(JSONObject savingInformation)
    {

        // conditions
        JSONObject conditions = savingInformation.GetField("conditions");

        if (this.conditions != null && conditions.keys != null) {
            foreach (string key in conditions.keys) {

                foreach (Condition c in this.conditions) {
                    if (c.name.Equals(key)) {
                        bool satisfied;
                        conditions.GetField(out satisfied, key, false);
                        c.satisfied = satisfied;
                    }
                }
            }
        }

        // animations
        JSONObject animators = savingInformation.GetField("animators");

        if (this.animatedObjects != null && animators.keys != null) {
            foreach (string key in animators.keys) {

                Animator anim = this.GetGameObjectFromString(key, this.gameObject).GetComponent<Animator>();
                JSONObject animatedObjectInformation = animators.GetField(key);

                int i = 0;

                while (animatedObjectInformation.HasField(i.ToString())) {

                    JSONObject layer = animatedObjectInformation.GetField(i.ToString());

                    string stateName;
                    layer.GetField(out stateName, "state", "");

                    int layerIndex;
                    int.TryParse(key, out layerIndex);

                    float ts;
                    layer.GetField(out ts, "ts", -1);

                    anim.Play(stateName, layerIndex, ts);

                    i++;
                }
            }
        }

        // parenting
        JSONObject parenting = savingInformation.GetField("parenting");

        if (this.reparentedObjects != null && parenting.keys != null) {
            foreach (string key in parenting.keys) {

                GameObject child = this.GetGameObjectFromString(key, this.gameObject);

                // the parent is also encoded in the saveString (on 2nd place)
                // --> we can simply remove everything after the last ',' and use this.GetGameObjectFromString

                // there is probably a smarter solution
                string[] split = key.Split(',');
                string parentPath = split[0];

                for (int i = 1; i < split.Length - 1; i++) {
                    parentPath += ',' + split[i];
                }

                GameObject parent = this.GetGameObjectFromString(parentPath, this.gameObject);
                
                if (!child.transform.parent.Equals(parent.transform)) {
                    child.transform.parent = parent.transform;
                }
            }
        }

        // camera
        if (main != null)
        {
            JSONObject position = savingInformation.GetField("camera");

            float x, y, z;
            position.GetField(out x, "x", this.main.transform.position.x);
            position.GetField(out y, "y", this.main.transform.position.y);
            position.GetField(out z, "z", this.main.transform.position.z);

            main.transform.position = new Vector3(x, y, z);
        }

        // forceMenus
        JSONObject menuInformation = savingInformation.GetField("fmData");
        if (this.forceMenus != null && menuInformation.keys != null) {
            foreach (string key in menuInformation.keys) {

                ForceMenuTarget fm = this.GetGameObjectFromString(key, this.gameObject).GetComponent<ForceMenuTarget>();
                JSONObject menuData = menuInformation.GetField(key);

                bool fmEnabled, lvlUnlocked;
                menuData.GetField(out fmEnabled, "enabled", false);
                menuData.GetField(out lvlUnlocked, "unlocked", false);

                if (lvlUnlocked) {
                    fm.TurnOn();
                }

                if (fmEnabled) {
                    fm.TurnOn();
                } else {
                    fm.TurnOff();
                }
            }
        }

        // specialized information
        JSONObject specific = savingInformation.GetField("specific");
        if (specific.keys != null) {
            this.RestoreSpecializedSavingInformation(specific);
        }
    }

    /// <summary>
    /// Applies the given specialized information to the corresponding objects. Should be overwritten by inheriting classes.
    /// </summary>
    /// <param name="specializedInformation">The specialized information that is used for the restoring process.</param>
    protected virtual void RestoreSpecializedSavingInformation(JSONObject specializedInformation) {
        // Nothing to do here as the parent class does not correspond to something specifically
    }

    /// <summary>
    /// Generates a unique identifier, which can be used to save a gameobject. The identifier encodes the path in the parenting hierarchie from the root to the storedObject.
    /// </summary>
    /// <param name="storedObject">The object, the identifier is generated for.</param>
    /// <param name="root">The root of the corresponding hierarchi</param>
    /// <returns>Unique identifier to save stordeObject.</returns>
    protected string GenerateSaveString(GameObject storedObject, GameObject root) {

        string saveString = "";

        Transform parent = storedObject.transform;

        while (!parent.Equals(root.transform)) {
            saveString = parent.GetSiblingIndex().ToString() + "," + saveString;
            parent = parent.parent;
        }

        return saveString;
    }

    /// <summary>
    /// Returns the game object corresponding to the saveString, if the saveString was generated according this.GenerateSaveString(gameObject, root)
    /// </summary>
    /// <param name="saveString">The string, the game object should be restored from.</param>
    /// <param name="root">The root of the hierarchy, the saveString is applied to.</param>
    /// <returns>GameObject corresponding to saveString</returns>
    protected GameObject GetGameObjectFromString(string saveString, GameObject root) {

        string[] path = saveString.Split(',');
        GameObject parent = null;
        GameObject child = root;

        for (int i = 0; i < path.Length - 1; i++) {
            int childIndex;
            int.TryParse(path[i], out childIndex);

            parent = child;
            child = parent.transform.GetChild(childIndex).gameObject;
        }

        return child;
    }
}

