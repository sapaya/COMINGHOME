using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorBoolManager : MonoBehaviour
{
    #region Variables
    private Animator anim;
    #endregion

    #region Main Methods
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    #endregion

    #region Helper Methods
    public void CheckBool(string boolName)
    {
        anim.SetBool(boolName, true);
    }

    public void UncheckBool(string boolName)
    {
        anim.SetBool(boolName, false);
    }
    #endregion
}
