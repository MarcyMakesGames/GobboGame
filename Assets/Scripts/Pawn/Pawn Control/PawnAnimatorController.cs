using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAnimatorController : MonoBehaviour
{
    [SerializeField] private PawnController controller;
    [SerializeField] private Animator anim;

    public delegate void AnimationComplete();
    public static event AnimationComplete OnAnimationComplete;

    private Action postAnimationAction = null;

    public void SetAnimation(AnimationEnums animationDirection, Action onAnimationComplete = null)
    {
        if(onAnimationComplete != null)
            postAnimationAction = onAnimationComplete;

        switch (animationDirection)
        {
            case AnimationEnums.Up:
                anim.SetTrigger("MoveUp");
                break;
            case AnimationEnums.Down:
                anim.SetTrigger("MoveDown");
                break;
            case AnimationEnums.Left:
                anim.SetTrigger("MoveLeft");
                break;
            case AnimationEnums.Right:
                anim.SetTrigger("MoveRight");
                break;
            case AnimationEnums.Celebration:
                anim.SetTrigger("Celebration");
                break;
            case AnimationEnums.Negative:
                break;
            default:
                Debug.Log("Couldn't find the corrresponding animation!");
                return;
        }
    }

    public void ResetTriggers()
    {
        anim.ResetTrigger("MoveUp");
        anim.ResetTrigger("MoveDown");
        anim.ResetTrigger("MoveLeft");
        anim.ResetTrigger("MoveRight");

        OnAnimationComplete?.Invoke();
        postAnimationAction?.Invoke();
        postAnimationAction = null;
    }
}
