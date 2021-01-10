using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parenting : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;

    [SerializeField] Transform targetParent;

    public void SetParent(Transform t)
    {
        targetParent = t;
    }

    public void AttachToParentIf(Condition condition)
    {
        if (condition.satisfied)
            AttachToParent();
    }

    public void AttachToParent() {
        startPos = transform.position;
        startRot = transform.rotation;

        transform.parent = targetParent;
    }

    public void UnParent(bool reset)
    {
        transform.parent = null;

        if (reset)
        {
            transform.position = startPos;
            transform.rotation = startRot;
        }
    }
}
