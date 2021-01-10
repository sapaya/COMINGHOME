using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    private PauseGame m_pauseGame;

    private void Awake()
    {
        m_pauseGame = FindObjectOfType<PauseGame>();
    }

    public void OnContinue()
    {
        m_pauseGame.Pause();
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
