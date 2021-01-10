using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows you to control all the force menus in a given level
/// </summary>
public class Level : MonoBehaviour
{
    #region Variables
    [SerializeField] bool m_StartUnlocked = true;
    [SerializeField] Component[] m_ForceMenuTargets = new Component[0]; 
    #endregion

    #region Main Methods
    // Start is called before the first frame update
    void Start()
    {
        m_ForceMenuTargets = GetComponentsInChildren<ForceMenuTarget>(true);
        if (!m_StartUnlocked)
            TurnOffLevel();
    }
    #endregion

    #region Helper Methods
    public void TurnOffLevel()
    {
        foreach(ForceMenuTarget fm in m_ForceMenuTargets)
        {
            fm.TurnOff();
        }
    }

    public void TurnOnLevel()
    {
        foreach (ForceMenuTarget fm in m_ForceMenuTargets)
        {
            fm.TurnOn();
        }
    }
    #endregion
}
