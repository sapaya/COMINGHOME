using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorSpeedControl : MonoBehaviour
{
    [Range(1f,2f)]
    [SerializeField] float speedFactor = 1f;
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SpeedUp(float length)
    {
        if (animator != null)
        {
            StartCoroutine(SpeedUpAnimator(length));
        }
    }

    private IEnumerator SpeedUpAnimator(float length)
    {
        float oldspeed = animator.speed;
        animator.speed = animator.speed * speedFactor;
        yield return new WaitForSecondsRealtime(length);
        animator.speed = oldspeed;
    }
}
