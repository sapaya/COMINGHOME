using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillTwigProximityObserver : MonoBehaviour
{

    public Condition spikeCloseCondition;

    public float minDegree;
    public float maxDegree;

    private bool observing = false;
    private bool conditionSet = false;
    private float offset = 0f;

    private GameManager gm;

    private void Awake() {
        this.gm = FindObjectOfType<GameManager>();
    }

    private void Update() {
        if (observing) {
        float r = transform.parent.rotation.eulerAngles.x;

        // "rotate" wheel to 0
        r -= offset;

            // map r to [-180, 180]
            r = r < -180 ? r + 360 : r;
            r = r > 180 ? r - 360 : r;

            // check if in given interval
            if (r >= this.minDegree && r <= this.maxDegree) {
                // set spike close condition
                if (!this.conditionSet) {
                    this.conditionSet = true;
                    gm.CheckCondition(this.spikeCloseCondition);
                    Debug.Log("SPIKE CLOSE");
                }
            } else {
                if (this.conditionSet) {
                    this.conditionSet = false;
                    gm.UncheckCondition(this.spikeCloseCondition);
                    Debug.Log("SPIKE DISTANT");
                }
            }
       }
    }

    public void StartObserving() {
        this.observing = true;
        this.offset = transform.parent.rotation.eulerAngles.x;
    }

}
