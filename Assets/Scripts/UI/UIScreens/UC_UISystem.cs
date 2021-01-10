using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace EvilScience.UI
{
    public class UC_UISystem : MonoBehaviour
    {
        #region Variables
        [Header("Main Properties")]
        public UC_UIScreen m_StartScreen;

        [Header("System Events")]
        public UnityEvent onSwitchedScreen = new UnityEvent();

        private Component[] screens = new Component[0];
        private UC_UIScreen previousScreen;
        public UC_UIScreen PreviousScreen
        {
            get { return previousScreen; }
        }

        private UC_UIScreen currentScreen;
        public UC_UIScreen CurrentScreen
        {
            get { return currentScreen;  }
        }
        #endregion

        #region Main Methods
        // Start is called before the first frame update
        void Start()
        {
            screens = GetComponentsInChildren<UC_UIScreen>(true); // get disabled components as well
            InitializeScreens();

            if (m_StartScreen)
            {
                SwitchScreen(m_StartScreen);
            }
       
        }
        #endregion

        #region Helper Methods
    
        public void SwitchScreen(UC_UIScreen newScreen)
        {
            if (newScreen)
            {
                if (currentScreen)
                {
                    currentScreen.CloseScreen();
                    previousScreen = currentScreen;
                }

                currentScreen = newScreen;
                currentScreen.gameObject.SetActive(true);
                currentScreen.StartScreen();

                if(onSwitchedScreen != null)
                {
                    onSwitchedScreen.Invoke();
                }
            }
        }

        public void GoToPreviousScreen()
        {
            if (previousScreen)
            {
                SwitchScreen(previousScreen);
            }
        }


        void InitializeScreens()
        {
            foreach(var screen in screens)
            {
                screen.gameObject.SetActive(true);
            }
        }
        #endregion
    }
}
