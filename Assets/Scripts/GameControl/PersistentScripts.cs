using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentScripts : MonoBehaviour
{
    public static PersistentScripts persistent;

    // Keep Persistent Scene
    private void Awake()
    {
        if (persistent == null)
        {
            DontDestroyOnLoad(gameObject);
            persistent = this;
        }
        else if (persistent != this)
        {
            Destroy(gameObject);
            Debug.Log("Destroyed old Persistents");
        }
    }

}
