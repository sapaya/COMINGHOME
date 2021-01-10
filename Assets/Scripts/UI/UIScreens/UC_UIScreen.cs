using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace EvilScience.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UC_UIScreen : MonoBehaviour
    {
        #region Variables
        [Header("ScreenEvents")]
        public UnityEvent onScreenStart = new UnityEvent();
        public UnityEvent onScreenClose = new UnityEvent();

        private Animator animator;
        #endregion
        

        #region Helper Methods
        public virtual void StartScreen()
        {
            if(onScreenStart != null)
            {
                onScreenStart.Invoke();
            }

            HandleAnimator("show");
        }

        public virtual void CloseScreen()
        {
            if (onScreenClose != null)
            {
                onScreenClose.Invoke();
            }

            HandleAnimator("hide");
        }

        void HandleAnimator(string trigger)
        {
            animator = GetComponent<Animator>();
            if (animator)
            {
                animator.SetTrigger(trigger);
            }
        }
        #endregion
    }
}

