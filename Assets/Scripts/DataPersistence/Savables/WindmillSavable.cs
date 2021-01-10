using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillSavable : Savable {




    protected override JSONObject GetSpecializedSavingInformation() {

        JSONObject saveData = new JSONObject();


        return saveData;
    }


    protected override void RestoreSpecializedSavingInformation(JSONObject specializedInformation) {

    }

    }
