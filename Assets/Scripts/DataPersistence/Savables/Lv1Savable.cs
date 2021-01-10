using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv1Savable : Savable {

    public Transform Lv1_Raven;

    protected override JSONObject GetSpecializedSavingInformation() {

        JSONObject specificInformation = new JSONObject();

        // Raven position, rotation
        JSONObject raven = new JSONObject();

        JSONObject pos = new JSONObject();
        pos.AddField("x", this.Lv1_Raven.position.x);
        pos.AddField("y", this.Lv1_Raven.position.y);
        pos.AddField("z", this.Lv1_Raven.position.z);

        JSONObject rot = new JSONObject();
        rot.AddField("x", this.Lv1_Raven.eulerAngles.x);
        rot.AddField("y", this.Lv1_Raven.eulerAngles.y);
        rot.AddField("z", this.Lv1_Raven.eulerAngles.z);

        raven.AddField("position", pos);
        raven.AddField("rotation", rot);

        specificInformation.AddField("raven", raven);

        return specificInformation;
    }

    protected override void RestoreSpecializedSavingInformation(JSONObject specializedInformation) {

        // apply raven position + rotation
        if (specializedInformation.HasField("raven")) {
            JSONObject position = specializedInformation.GetField("raven").GetField("position");

            float x, y, z;
            position.GetField(out x, "x", Lv1_Raven.position.x);
            position.GetField(out y, "y", Lv1_Raven.position.y);
            position.GetField(out z, "z", Lv1_Raven.position.z);

            JSONObject rotation = specializedInformation.GetField("raven").GetField("rotation");

            float rx, ry, rz;
            position.GetField(out rx, "x", Lv1_Raven.rotation.eulerAngles.x);
            position.GetField(out ry, "y", Lv1_Raven.rotation.eulerAngles.y);
            position.GetField(out rz, "z", Lv1_Raven.rotation.z);

            Lv1_Raven.position = new Vector3(x, y, z);
            Lv1_Raven.eulerAngles = new Vector3(rx, ry, rz);
        }      
    }

}

