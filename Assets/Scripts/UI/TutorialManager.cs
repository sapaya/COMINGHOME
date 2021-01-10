using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles display of tutorial/control info
[RequireComponent(typeof(Animator))]
public class TutorialManager : MonoBehaviour
{
    public GameSaver save;
    private Animator anim;

    // hide self if game has been saved previously.
    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (save.SavegameExists())
        {
            anim.SetTrigger("inactive");
        }
    }

    public void HasClicked()
    {
        anim.SetTrigger("clicked");
    }

    public void HasInteracted()
    {
        anim.SetBool("interacted", true);
    }
}
