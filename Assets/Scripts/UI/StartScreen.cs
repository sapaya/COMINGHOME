using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    private SceneControl sceneControl;

    public void OnNewGame()
    {
        CursorControl cc = FindObjectOfType<CursorControl>();
        cc.SlowDown();

        sceneControl = PersistentScripts.persistent.GetComponentInChildren<SceneControl>();

        if (!sceneControl)
            throw new UnityException("Scene Control missing!");

        sceneControl.FadeAndLoadScene("Game");
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
