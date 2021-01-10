using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondAnimTrigger : MonoBehaviour {

    public Condition condition;
    public Animator animator;
    public string trigger;

    public void SetTriggerIf() {
        if (AllConditions.CheckCondition(this.condition)) {
            this.animator.SetTrigger(this.trigger);
        }
    }

}
