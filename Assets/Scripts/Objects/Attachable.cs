using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(ParentConstraint))]
public class Attachable : MonoBehaviour
{
    #region Variables
    private bool isAttached = false;
    private ParentConstraint constraint;
    [SerializeField] Vector3 offset;
    #endregion

    #region Main Methods
    private void Start()
    {
        constraint = GetComponent<ParentConstraint>();
    }
    #endregion

    #region Helper Methods
    public void AttachIf(Condition condition)
    {
        if (condition.satisfied)
            Attach();
    }

    public void Attach()
    {
        isAttached = true;

        // Set offset to attached source and set lock
        Vector3 targetPosInv = constraint.GetSource(0).sourceTransform.position * -1.0f;
        Vector3 currentPos = this.transform.position;

        constraint.translationAtRest = currentPos;
        constraint.SetTranslationOffset(0, targetPosInv + currentPos + offset);
        constraint.locked = true;

        // activate constraints
        constraint.constraintActive = true;
    }

    public void Detach(bool resetPosition)
    {
        isAttached = false;
        constraint.constraintActive = false;

        if (resetPosition)
        {
            this.transform.position = Vector3.zero;
            this.transform.rotation = Quaternion.identity;
        }
    }

    public void ChangeTarget(Transform t)
    {
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = t;
        source.weight = 1;
        constraint.SetSource(0, source);

        Attach();
    }
    #endregion
}